using ClipboardEdit.Helpers;
using ClipboardEdit.Properties;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.UI.Shell;
using WindowsFormsAero.TaskDialog;

namespace ClipboardEdit
{
    public partial class Form1 : Form
    {
        private bool _paused = false;

        public Form1()
        {
            InitializeComponent();
            
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string path = args[1];

                if (!File.Exists(path))
                {
                    MessageBox.Show("The file you specified does not exist.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                string ext = Path.GetExtension(path);

                if (!ext.Equals(".CSNAP", StringComparison.OrdinalIgnoreCase) && !ext.Equals(".CSNAPZ", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("The file you specified is not a clipboard snapshot.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                OpenSnapshotFile(path);

                if (!_snapshotOpen)
                    Close();
            }

            _byteProvider = new(Handle);
            hexBox1.ByteProvider = _byteProvider;
            _byteProvider.Changed += byteProvider_Changed;

            menuItem10.Text = "S&ettings...\tCtrl+,";

            UpdateClipboard();

            _docData = DocHelper.LoadDocs();

            if (!_docData.Any())
                if (MessageBox.Show("In order to view documentation for clipboard formats, you must update the documentation" +
                    " index first. Do you want to update the index now?\n\nYou will only have to do this once.\n" +
                    "You can update the index later by selecting Update Documentation on the Help menu.",
                    "Update Documentation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    DoUpdateDocs(true);

            hexBox1.Font = Settings.Default.MonospaceFont;
        }

        private void byteProvider_Changed(object sender, EventArgs e)
        {
            toolBarButton9.Enabled = _byteProvider.HasChanges();
            menuItem31.Enabled = _byteProvider.HasChanges();
            menuItem40.Enabled = _byteProvider.HasChanges();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            PInvoke.AddClipboardFormatListener(new HWND(Handle));
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            PInvoke.RemoveClipboardFormatListener(new HWND(Handle));
            base.OnHandleDestroyed(e);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case (int)PInvoke.WM_CLIPBOARDUPDATE:
                    UpdateClipboard();
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Oemcomma))
            {
                ShowSettings();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ShowSettings()
        {
            var dlg = new SettingsDlg();
            dlg.ShowDialog();

            hexBox1.Font = Settings.Default.MonospaceFont;
            UpdateDocsView();
        }

        private void menuItem16_Click(object sender, EventArgs e)
        {
            var dlg = new TaskDialog
            {
                AllowDialogCancellation = true,
                CommonButtons = CommonButton.Cancel | TaskDialogSpecialButtons.Continue,
                CommonIcon = CommonIcon.SecurityWarning,
                Title = "Warning",
                Instruction = "Warning",
                Content = "This tool will repeatedly register clipboard formats until system limits are reached.\n\n" +
"This may cause system instability in the current user session, including:\n" +
"- Broken or unresponsive shell features\n" +
"- Drag-and-drop not working in some applications\n" +
"- Some applications failing to launch or behave correctly\n\n" +
"No files are modified, and the system will return to normal after restarting Windows.\n\n" +
"You should only run this in a virtual machine or test environment.\n\n" +
"Do you want to continue?",
                DefaultButton = TaskDialogSpecialButtons.ContinueResult
            };

            var result = dlg.Show(this);
            
            if (result.ButtonID == TaskDialogSpecialButtons.ContinueResult)
            {
                var d = new FormatFillerForm();
                d.ShowDialog();
            }
        }

        private SavePromptResult ShowSavePrompt(string text, string instruction = "Do you want to save changes?",
            string caption = "Unsaved changes", string saveButtonText = "Save", string ignoreButtonText = "Don't save",
            bool allowCancel = true)
        {
            TaskDialog dlg = new TaskDialog
            {
                Title = caption,
                Instruction = instruction,
                Content = text,
                AllowDialogCancellation = allowCancel,
                CommonButtons = allowCancel ? CommonButton.Cancel : 0,
                CustomButtons =
                [
                    new CustomButton(CommonButtonResult.OK, saveButtonText),
                    new CustomButton(CommonButtonResult.Ignore, ignoreButtonText)
                ]
            };

            var result = dlg.Show(this);

            return result.CommonButton switch
            {
                CommonButtonResult.OK => SavePromptResult.Save,
                CommonButtonResult.Cancel => SavePromptResult.None,
                CommonButtonResult.Ignore => SavePromptResult.Ignore,
                _ => SavePromptResult.None
            };
        }

        private void UpdateClipboard()
        {
            if (_byteProvider.Lock) return;
            if (_paused) return;
            if (_snapshotOpen) return;

            if (PInvoke.OpenClipboard(new HWND(Handle)))
            {
                try
                {
                    listView1.BeginUpdate();

                    uint? lastSelectedFormat = null;

                    if (listView1.SelectedItems.Count > 0)
                        lastSelectedFormat = (listView1.SelectedItems[0].Tag as ClipboardEntryInfo).Format;

                    listView1.Items.Clear();

                    uint format = 0;

                    while ((format = PInvoke.EnumClipboardFormats(format)) != 0)
                    {
                        string name;

                        if (format >= 0xC000)
                        {
                            Span<char> nameBuf = stackalloc char[256];

                            int len = PInvoke.GetClipboardFormatName(format, nameBuf);

                            name = len > 0
                                ? nameBuf.Slice(0, len).ToString()
                                : $"CF_{format}";
                        }
                        else
                        {
                            name = ClipboardHelper.GetPredefinedFormatName(format);
                        }

                        string type = "Unknown";

                        if (format >= 0xC000 && format <= 0xFFFF)
                            type = "Registered";
                        else
                            type = "Standard";

                        byte[] data = ClipboardHelper.GetData(format);

                        listView1.Items.Add(new ListViewItem
                        {
                            Tag = new ClipboardEntryInfo
                            {
                                Data = data,
                                Format = format,
                                Info = new()
                                {
                                    _size = (IntPtr)data.LongLength,
                                    _fmt = name,
                                    _tfmt = type,
                                    _ufmt = format,
                                    _wide = Encoding.Unicode.GetStringOrDefault(data, "<invalid>"),
                                    _ansi = Encoding.Default.GetStringOrDefault(data, "<invalid>"),
                                    _ascii = Encoding.ASCII.GetStringOrDefault(data, "<invalid>"),
                                    _byte = (sbyte)NumericHelper.InterpretSignedBytes(data, 0, 8),
                                    _ubyte = (byte)NumericHelper.InterpretBytes(data, 0, 8),
                                    _short = (short)NumericHelper.InterpretSignedBytes(data, 0, 16),
                                    _ushort = (ushort)NumericHelper.InterpretBytes(data, 0, 16),
                                    _int = (int)NumericHelper.InterpretSignedBytes(data, 0, 32),
                                    _uint = (uint)NumericHelper.InterpretBytes(data, 0, 32),
                                    _long = (long)NumericHelper.InterpretSignedBytes(data, 0, 64),
                                    _ulong = (ulong)NumericHelper.InterpretBytes(data, 0, 64),
                                    _float = (float)NumericHelper.InterpretFloatBytes(data, 0),
                                    _db = (double)NumericHelper.InterpretDoubleBytes(data, 0),
                                    _filetime = NumericHelper.TryCreateFileTimeUtc(NumericHelper.InterpretSignedBytes(data, 0, 64)),
                                    _lfiletime = NumericHelper.TryCreateFileTime(NumericHelper.InterpretSignedBytes(data, 0, 64))
                                }
                            },
                            Text = name,
                            SubItems =
                            {
                                new ListViewItem.ListViewSubItem
                                {
                                    Text = $"ID: {format}"
                                },
                                new ListViewItem.ListViewSubItem
                                {
                                    Text = type
                                }
                            },
                            //Selected = format == lastSelectedFormat
                        });
                    }
                }
                finally
                {
                    listView1.EndUpdate();
                    PInvoke.CloseClipboard();
                }
            }
        }

        private ClipboardByteProvider _byteProvider;

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_byteProvider.HasChanges() && listView1.SelectedItems.Count < 1)
            {
                var result = ShowSavePrompt("You have unwritten bytes in the current clipboard format.",
                    "Do you want to write changes?", "Unwritten Changes", "Write", "Don't write");

                if (result == SavePromptResult.Save)
                    _byteProvider.ApplyChanges();
                else if (result == SavePromptResult.None)
                    return;
            }

            if (listView1.SelectedItems.Count > 0)
            {
                _byteProvider.Entry = (listView1.SelectedItems[0].Tag) as ClipboardEntryInfo;
                propertyGrid1.SelectedObject = ((listView1.SelectedItems[0].Tag) as ClipboardEntryInfo).Info;
            }
            else
            {
                _byteProvider.Bytes.Clear();
                propertyGrid1.SelectedObject = null;
            }

            UpdateDocsView();
            hexBox1.Refresh();
        }

        private void menuItem20_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Help", "help.chm");

            Help.ShowHelp(this, path, HelpNavigator.TableOfContents);
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == toolBarButton9)
            {
                _byteProvider.ApplyChanges();

                toolBarButton9.Enabled = false;
                menuItem31.Enabled = false;
                menuItem40.Enabled = false;
            }
            else if (e.Button == toolBarButton4)
            {
                _paused = e.Button.Pushed;
                menuItem4.Text = _paused ? "Resume updates" : "Pause updates";
            }
            else if (e.Button == toolBarButton5)
            {
                var dlg = new FormatListForm();
                dlg.ShowDialog();
            }
            else if (e.Button == toolBarButton1)
                menuItem7_Click(sender, e);
            else if (e.Button == toolBarButton2)
                menuItem6_Click(sender, e);
            else if (e.Button == toolBarButton7)
                ShowSettings();
            else if (e.Button == toolBarButton6)
            {
                var dlg = new AboutDlg();
                dlg.ShowDialog();
            }
        }

        private void menuItem31_Click(object sender, EventArgs e)
        {
            _byteProvider.ApplyChanges();

            toolBarButton9.Enabled = false;
            menuItem31.Enabled = false;
            menuItem40.Enabled = false;
        }

        private void menuItem7_Select(object sender, EventArgs e)
        {
            statusBar1.Text = "Open a snapshot file to view the clipboard contents as they were at the time it was created.";
        }

        private void menuItem6_Select(object sender, EventArgs e)
        {
            statusBar1.Text = "Save a snapshot to save the current clipboard data to view later.";
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            _paused = !_paused;

            toolBarButton4.Pushed = _paused;
            menuItem4.Text = _paused ? "Resume updates" : "Pause updates";
        }

        private void menuItem41_Click(object sender, EventArgs e)
        {
            UpdateClipboard();
        }

        private void menuItem15_Click(object sender, EventArgs e)
        {
            var dlg = new FormatListForm();
            dlg.ShowDialog();
        }

        private void menuItem27_Click(object sender, EventArgs e)
        {
            var dlg = new FindDialog();
            dlg.Box = hexBox1;
            dlg.Show();
        }

        private void menuItem11_Popup(object sender, EventArgs e)
        {
            menuItem21.Enabled = hexBox1.CanCut();
            menuItem22.Enabled = hexBox1.CanCopy();
            menuItem28.Enabled = hexBox1.CanCopy();
            menuItem23.Enabled = hexBox1.CanPaste();
            menuItem24.Enabled = hexBox1.CanPasteHex();
            menuItem26.Enabled = hexBox1.CanSelectAll();
        }

        private void menuItem21_Click(object sender, EventArgs e) => hexBox1.Cut();

        private void menuItem22_Click(object sender, EventArgs e) => hexBox1.Copy();

        private void menuItem28_Click(object sender, EventArgs e) => hexBox1.CopyHex();

        private void menuItem23_Click(object sender, EventArgs e) => hexBox1.Paste();

        private void menuItem24_Click(object sender, EventArgs e) => hexBox1.PasteHex();

        private void menuItem26_Click(object sender, EventArgs e) => hexBox1.SelectAll();

        private void menuItem10_Click(object sender, EventArgs e) => ShowSettings();

        private async void menuItem39_Click(object sender, EventArgs e)
        {
            DoUpdateDocs();
        }

//        private HRESULT DocUpdateTaskDialogCallback(HWND hwnd, TASKDIALOG_NOTIFICATIONS uMsg, WPARAM wParam, LPARAM lParam,
//        nint lpRefData)
//        {
//            switch (uMsg)
//            {
//                case TASKDIALOG_NOTIFICATIONS.TDN_CREATED:
//                    PInvoke.SendMessage(hwnd, (uint)TASKDIALOG_MESSAGES.TDM_ENABLE_BUTTON, 1, 0);

//                    DocHelper.UpdateDocsAsync().ContinueWith(t =>
//                    {
//                        unsafe
//                        {
//                            fixed (char* pszTitle = "Update Documentation",
//                                pszInstruct = t.Result ? "An error has occured." : "Documentation updated",
//                                pszContent = t.Result ? "Sadly, ClipboardEdit couldn't update the documentation. Please check your internet"
//                                + " connection and try again later." : "The documentation has been successfully updated.")
//                            {
//                                TASKDIALOGCONFIG tdc = new TASKDIALOGCONFIG
//                                {
//                                    cbSize = (uint)Marshal.SizeOf<TASKDIALOGCONFIG>(),
//                                    pszWindowTitle = new PCWSTR(pszTitle),
//                                    pszMainInstruction = new PCWSTR(pszInstruct),
//                                    pszContent = new PCWSTR(pszContent),
//                                    Anonymous1 =
//                                    {
//                                        pszMainIcon = new PCWSTR((char*)65528)
//                                    },
//                                    dwFlags = TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW | TASKDIALOG_FLAGS.TDF_SIZE_TO_CONTENT,
//                                    hwndParent = new HWND(Handle)
//                                };

//#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
//                                PInvoke.SendMessage(hwnd, (uint)TASKDIALOG_MESSAGES.TDM_NAVIGATE_PAGE, 0, new LPARAM();
//#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
//                            }
//                        }
//                    });
//                    break;
//            }

//            return HRESULT.S_OK;
//        }

        private void DoUpdateDocs(bool center = false)
        {
            //// We have to manually P/Invoke TaskDialogIndirect here
            //// because WindowsFormsAero does not allow us to specify
            //// a marquee progress bar.
            //fixed (char* pszTitle = "Update Documentation", pszInstruct = "Updating documentation", pszContent =
            //    "Please wait while ClipboardEdit downloads updated documentation from the Internet...")
            //{
            //    TASKDIALOGCONFIG tdc = new TASKDIALOGCONFIG
            //    {
            //        cbSize = (uint)Marshal.SizeOf<TASKDIALOGCONFIG>(),
            //        pszWindowTitle = new PCWSTR(pszTitle),
            //        pszMainInstruction = new PCWSTR(pszInstruct),
            //        pszContent = new PCWSTR(pszContent),
            //        Anonymous1 =
            //        {
            //            pszMainIcon = new PCWSTR((char*)65531)
            //        },
            //        dwFlags = TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW | TASKDIALOG_FLAGS.TDF_SIZE_TO_CONTENT
            //        | TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR,
            //        hwndParent = new HWND(Handle),
            //        pfCallback = DocUpdateTaskDialogCallback
            //    };

            //    PInvoke.TaskDialogIndirect(tdc);
            //}

            var dlg = new DocUpdateProgressDialog();
            Task.Run(async () =>
            {
                HRESULT hr = PInvoke.CoInitializeEx(COINIT.COINIT_APARTMENTTHREADED);
                if (hr.Succeeded)
                {
                    unsafe
                    {
                        hr = PInvoke.CoCreateInstance(new Guid("{56FDF344-FD6D-11D0-958A-006097C9A090}"), null,
                            CLSCTX.CLSCTX_INPROC_SERVER, out ITaskbarList3 list);

                        if (hr.Succeeded)
                        {
                            list.HrInit();
                            list.SetProgressState(new HWND(Handle), TBPFLAG.TBPF_INDETERMINATE);

                            Marshal.FinalReleaseComObject(list);
                        }
                        else Debugger.Break();
                    }

                    PInvoke.CoUninitialize();
                }

                var result = await DocHelper.UpdateDocsAsync();

                dlg.Invoke(() =>
                {
                    dlg.headingTextControl1.Text = !result.Success ? "An error has occured." : "Documentation updated";
                    dlg.label1.Text = !result.Success ? "Sadly, ClipboardEdit couldn't update the documentation. Please check your internet"
                    + $" connection and try again later.\n\n{(int)result.Code}: {result.Reason ?? result.Code.ToString()}"
                    : "The documentation has been successfully updated.";

                    _docData = DocHelper.LoadDocs();

                    dlg.progressBar1.Visible = false;
                    dlg.button1.Enabled = true;
                    dlg.ControlBox = true;
                });

                hr = PInvoke.CoInitializeEx(COINIT.COINIT_APARTMENTTHREADED);
                if (hr.Succeeded)
                {
                    unsafe
                    {
                        hr = PInvoke.CoCreateInstance(new Guid("{56FDF344-FD6D-11D0-958A-006097C9A090}"), null,
                            CLSCTX.CLSCTX_INPROC_SERVER, out ITaskbarList3 list);

                        if (hr.Succeeded)
                        {
                            list.HrInit();
                            list.SetProgressState(new HWND(Handle), TBPFLAG.TBPF_NOPROGRESS);

                            Marshal.FinalReleaseComObject(list);
                        }
                        else Debugger.Break();
                    }

                    PInvoke.CoUninitialize();
                }
            });

            if (center) dlg.Center();
            dlg.ShowDialog();
        }

        private void menuItem32_Click(object sender, EventArgs e)
        {
            (sender as MenuItem)?.Checked = !((sender as MenuItem)?.Checked ?? false);

            splitContainer2.Panel1Collapsed = !menuItem32.Checked;
            splitContainer2.Panel2Collapsed = !menuItem33.Checked;
            splitContainer1.Panel2Collapsed = !menuItem34.Checked;
            toolBar1.Visible                = menuItem35.Checked;
            statusBar1.Visible              = menuItem36.Checked;
            
            splitContainer1.Panel1Collapsed = (!menuItem32.Checked) && (!menuItem33.Checked);
            //splitContainer2.Visible         = menuItem32.Checked || menuItem33.Checked || menuItem34.Checked;
        }

        private DocsViewerForm _docsView;
        private List<JsonDocInfo> _docData = [];

        private void menuItem37_Click(object sender, EventArgs e)
        {
            if (_docsView != null)
            {
                _docsView.Close();
                _docsView = null;
            }
            else
            {
                if (!_docData.Any())
                {
                    MessageBox.Show("The documentation cannot be shown as the documentation index does not exist. Update the index first" +
                        " by selecting Update Documentation on the Help menu and try again.", "Not Found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                _docsView = new DocsViewerForm();
                _docsView.Disposed += (s, e) => _docsView = null;
                _docsView.Show();

                UpdateDocsView();
            }
        }

        private MarkdownDocument PreprocessDocMarkdown(string md)
        {
            var doc = Markdown.Parse(md);

            var textInfo = CultureInfo.CurrentCulture.TextInfo;

            for (int i = 0; i < doc.Count; i++)
            {
                if (doc[i] is HeadingBlock heading)
                {
                    //if (i == 0)
                    //{
                    //    doc.RemoveAt(i);
                    //    continue;
                    //}

                    string text = ExtractInlineText(heading.Inline).Trim();

                    // normalize for matching
                    string normalized = text.Replace(" ", "")
                                            .Replace("-", "")
                                            .ToLowerInvariant();

                    // 1. Handle "- see also"
                    if (normalized == "seealso")
                    {
                        int startLevel = heading.Level;

                        // remove heading itself
                        doc.RemoveAt(i);

                        // remove everything until next heading of same or higher level
                        while (i < doc.Count)
                        {
                            if (doc[i] is HeadingBlock hb && hb.Level <= startLevel)
                                break;

                            doc.RemoveAt(i);
                        }

                        i--; // adjust index after removal
                        continue;
                    }

                    // 2. Fix "- something" headings
                    if (text.StartsWith("-"))
                    {
                        string cleaned = text.TrimStart('-').Trim();

                        cleaned = cleaned.Replace('-', ' ');
                        cleaned = textInfo.ToTitleCase(cleaned.ToLowerInvariant());

                        heading.Inline.Clear();
                        heading.Inline.AppendChild(new LiteralInline(cleaned));
                    }
                }
            }

            return doc;
        }

        private static string ExtractInlineText(ContainerInline container)
        {
            var sb = new StringBuilder();

            foreach (var inline in container)
            {
                switch (inline)
                {
                    case LiteralInline lit:
                        sb.Append(lit.Content.ToString());
                        break;

                    case ContainerInline child:
                        sb.Append(ExtractInlineText(child));
                        break;
                }
            }

            return sb.ToString();
        }

        private void UpdateDocsView()
        {
            if (_docsView == null) return;
            if (listView1.SelectedItems.Count == 0) return;

            JsonDocInfo doc = _docData.FirstOrDefault(x =>
            x.Name.Equals(listView1.SelectedItems[0].Text, StringComparison.OrdinalIgnoreCase));

            if (doc != null)
            {
                _docsView.label1.Text = doc.Description;

                if (string.IsNullOrEmpty(doc.AdditionalStructDocs))
                {
                    _docsView.AllowNavigate = true;

                    _docsView.webBrowser1.DocumentText = "<html></html>";
                    _docsView.ResizeBrowser();

                    _docsView.AllowNavigate = false;
                    return;
                }

                var    md   = PreprocessDocMarkdown(doc.AdditionalStructDocs);
                string html = Markdown.ToHtml(md,
                    new MarkdownPipelineBuilder()
                    .UsePipeTables()
                    .UseGridTables()
                    .Build());

                html =
                    $@"
                    <html>
                    <head>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                    <style>
                        body {{
                            font-family: Segoe UI, Arial, sans-serif;
                            font-size: 9pt;
                            margin: 8px;
                        }}

                        code {{
                            font-family: {Settings.Default.MonospaceFont.FontFamily.Name}, monospace;
                            font-size: {Settings.Default.MonospaceFont.SizeInPoints.ToString().Replace(',', '.')}pt;
                        }}

                        pre {{
                            font-family: {Settings.Default.MonospaceFont.FontFamily.Name}, monospace;
                            font-size: {Settings.Default.MonospaceFont.SizeInPoints.ToString().Replace(',', '.')}pt;
                        }}
                    </style>
                    </head>
                    <body>
                    {html}
                    </body>
                    </html>";

                _docsView.AllowNavigate = true;

                _docsView.webBrowser1.DocumentText = html;
                _docsView.ResizeBrowser();

                _docsView.AllowNavigate = false;
            }
        }

        private void menuItem18_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/FireBlade211/ClipboardEdit");
        }

        private const int SnapshotFormatMagic = 0x0F1B0A6D;

        private void WriteSnapshot(BinaryWriter writer)
        {
            writer.Write(SnapshotFormatMagic);  // magic
            writer.Write(0x10000000U);          // version
            
            if (PInvoke.OpenClipboard(new HWND(Handle)))
            {
                uint format = 0;
                List<(uint format, byte[] data)> formatData = new List<(uint format, byte[] data)>();

                while ((format = PInvoke.EnumClipboardFormats(format)) != 0)
                {
                    byte[] data = ClipboardHelper.GetData(format);

                    formatData.Add((format, data));
                }

                writer.Write(formatData.Count); // count
                
                foreach (var data in formatData)
                {
                    writer.Write(data.format);      // format
                    writer.Write(data.data.Length); // length of data
                    writer.Write(data.data);        // data
                }

                writer.Write(DateTime.Now.ToFileTimeUtc()); // date/time taken

                PInvoke.CloseClipboard();
            }
        }

        private struct SnapShotInfo
        {
            public uint Version { get; set; }
            public List<(uint format, byte[] data)> Formats { get; set; }
            public DateTime DateTaken { get; set; }
        }

        private SnapShotInfo? ReadSnapshot(BinaryReader reader)
        {
            try
            {
                SnapShotInfo info = new SnapShotInfo();
                List<(uint format, byte[] data)> formats = [];

                int magic = reader.ReadInt32();

                if (magic != SnapshotFormatMagic)
                    return null;

                uint version = reader.ReadUInt32();

                switch (version)
                {
                    // Version 1
                    case 0x10000000:
                        int count = reader.ReadInt32();
                        
                        for (int i = 0; i < count; i++)
                        {
                            uint format = reader.ReadUInt32();
                            int  length = reader.ReadInt32();
                            byte[] data = reader.ReadBytes(length);

                            formats.Add((format, data));
                        }

                        long fileTime = reader.ReadInt64();
                        info.DateTaken = DateTime.FromFileTimeUtc(fileTime);

                        break;
                }

                info.Formats = formats;
                info.Version = version;

                return info;
            }
            catch
            {
                return null;
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            var result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write))
                {
                    if (saveFileDialog1.FilterIndex == 2)
                    {
                        using (GZipStream gzip = new GZipStream(stream, Settings.Default.SnapshotCompressionLevel))
                        {
                            using (BinaryWriter writer = new BinaryWriter(gzip))
                            {
                                WriteSnapshot(writer);
                            }
                        }
                    }
                    else
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            WriteSnapshot(writer);
                        }
                    }
                }
            }
        }

        private bool _snapshotOpen = false;
        private SnapShotInfo? _snapshot = null;

        private void menuItem7_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
                OpenSnapshotFile(openFileDialog1.FileName);
        }

        public void OpenSnapshotFile(string path)
        {
            SnapShotInfo? info = null;

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(path).Equals(".CSNAPZ", StringComparison.OrdinalIgnoreCase))
                {
                    using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        using (BinaryReader reader = new BinaryReader(gzip))
                        {
                            info = ReadSnapshot(reader);
                        }
                    }
                }
                else
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        info = ReadSnapshot(reader);
                    }
                }
            }

            if (info != null)
            {
                _snapshotOpen = true;
                _snapshot = info;

                toolBarButton4.Enabled = false;
                menuItem4.Enabled = false;
                hexBox1.ReadOnly = true;
                menuItem41.Enabled = false;

                switch (info.Value.Version)
                {
                    case 0x10000000:
                        listView1.BeginUpdate();
                        listView1.Items.Clear();

                        foreach (var format in info.Value.Formats)
                        {
                            string name;

                            if (format.format >= 0xC000)
                            {
                                Span<char> nameBuf = stackalloc char[256];

                                int len = PInvoke.GetClipboardFormatName(format.format, nameBuf);

                                name = len > 0
                                    ? nameBuf.Slice(0, len).ToString()
                                    : $"CF_{format.format}";
                            }
                            else
                            {
                                name = ClipboardHelper.GetPredefinedFormatName(format.format);
                            }

                            string type = "Unknown";

                            if (format.format >= 0xC000 && format.format <= 0xFFFF)
                                type = "Registered";
                            else
                                type = "Standard";

                            listView1.Items.Add(new ListViewItem
                            {
                                Tag = new ClipboardEntryInfo
                                {
                                    Data = format.data,
                                    Format = format.format,
                                    Info = new()
                                    {
                                        _size = (IntPtr)format.data.LongLength,
                                        _fmt = name,
                                        _tfmt = type,
                                        _ufmt = format.format,
                                        _wide = Encoding.Unicode.GetStringOrDefault(format.data, "<invalid>"),
                                        _ansi = Encoding.Default.GetStringOrDefault(format.data, "<invalid>"),
                                        _ascii = Encoding.ASCII.GetStringOrDefault(format.data, "<invalid>"),
                                        _byte = (sbyte)NumericHelper.InterpretSignedBytes(format.data, 0, 8),
                                        _ubyte = (byte)NumericHelper.InterpretBytes(format.data, 0, 8),
                                        _short = (short)NumericHelper.InterpretSignedBytes(format.data, 0, 16),
                                        _ushort = (ushort)NumericHelper.InterpretBytes(format.data, 0, 16),
                                        _int = (int)NumericHelper.InterpretSignedBytes(format.data, 0, 32),
                                        _uint = (uint)NumericHelper.InterpretBytes(format.data, 0, 32),
                                        _long = (long)NumericHelper.InterpretSignedBytes(format.data, 0, 64),
                                        _ulong = (ulong)NumericHelper.InterpretBytes(format.data, 0, 64),
                                        _float = (float)NumericHelper.InterpretFloatBytes(format.data, 0),
                                        _db = (double)NumericHelper.InterpretDoubleBytes(format.data, 0),
                                        _filetime = NumericHelper
                                            .TryCreateFileTimeUtc(NumericHelper.InterpretSignedBytes(format.data, 0, 64)),
                                        _lfiletime = NumericHelper
                                            .TryCreateFileTime(NumericHelper.InterpretSignedBytes(format.data, 0, 64))
                                    }
                                },
                                Text = name,
                                SubItems =
                                    {
                                        new ListViewItem.ListViewSubItem
                                        {
                                            Text = $"ID: {format.format}"
                                        },
                                        new ListViewItem.ListViewSubItem
                                        {
                                            Text = type
                                        }
                                    }
                            });
                        }

                        listView1.EndUpdate();
                        break;
                    default:
                        MessageBox.Show("ClipboardEdit does not recognize this file version. This file may" +
                            " have been created by a later version of ClipboardEdit and cannot be read in" +
                            " this version.", "Invalid version", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        toolBarButton4.Enabled = true;
                        menuItem4.Enabled = true;
                        hexBox1.ReadOnly = false;
                        menuItem41.Enabled = true;

                        _snapshotOpen = false;
                        _snapshot = null;
                        break;
                }
            }
            else MessageBox.Show("The file you specified is invalid.", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void menuItem1_Popup(object sender, EventArgs e)
        {
            menuItem9.Enabled = _snapshotOpen;
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            _snapshotOpen = false;
            _snapshot = null;

            toolBarButton4.Enabled = true;
            menuItem4.Enabled = true;
            hexBox1.ReadOnly = false;
            menuItem41.Enabled = true;

            UpdateClipboard();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            var dlg = new AboutDlg();
            dlg.ShowDialog();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class ClipboardEntryInfo
    {
        public uint Format { get; set; }
        public byte[] Data { get; set; }
        
        public ClipboardFormatInfoItem Info { get; set; }
    }

    public enum SavePromptResult
    {
        /// <summary>
        /// The user cancelled the operation.
        /// </summary>
        None,
        /// <summary>
        /// The user wants to save the data.
        /// </summary>
        Save,
        /// <summary>
        /// The user does not want to save the data.
        /// </summary>
        Ignore
    }
}
