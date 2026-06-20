using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Windows.Win32;

namespace ClipboardEdit
{
    public partial class FormatListForm : Form
    {
        public FormatListForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke(() => listView1.BeginUpdate());

            // Note: The maximum amount of registered clipboard formats on Windows is 65535
            for (uint format = 0xC000; format < 65535; format++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                unsafe
                {
                    fixed (char* szName = new char[256])
                    {
                        int len = PInvoke.GetClipboardFormatName(format, szName, 256);

                        // If the length is 0, this format is not registered or an error occured.
                        if (len != 0)
                        {
                            string name = new string(szName, 0, len);

                            Invoke(() => listView1.Items.Add(new ListViewItem
                            {
                                Text = name,
                                SubItems =
                                {
                                    new ListViewItem.ListViewSubItem
                                    {
                                        Text = $"0x{format:X} ({format})"
                                    }
                                }
                            }));
                        }

                        backgroundWorker1.ReportProgress(
                            (int)((double)format / 65535.0 * 100.0)
                        );
                    }
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listView1.EndUpdate();
            label2.Text = $"{listView1.Items.Count} items";

            progressBar1.Visible = false;
            button2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                List<(string name, uint id)> formats = [];

                for (uint format = 0xC000; format < 65535; format++)
                {
                    unsafe
                    {
                        fixed (char* szName = new char[256])
                        {
                            int len = PInvoke.GetClipboardFormatName(format, szName, 256);

                            // If the length is 0, this format is not registered or an error occured.
                            if (len != 0)
                            {
                                string name = new string(szName, 0, len);

                                formats.Add((name, format));
                            }
                        }
                    }
                }

                try
                {
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            // CSV
                            File.WriteAllText(
                                saveFileDialog1.FileName,
                                "Format ID,Name" + Environment.NewLine + string.Join(Environment.NewLine,
                                    formats.Select(x => $"{x.id},{EscapeCsv(x.name)}"))
                            );
                            break;
                        case 2:
                            // Plaintext
                            File.WriteAllText(saveFileDialog1.FileName, "Format ID        Name" + Environment.NewLine +
                                string.Join(Environment.NewLine, formats.Select(x => $"{x.id}            {x.name}")));
                            break;
                    }

                    MessageBox.Show("The formats were exported successfully.", "Export successful", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured.\n\n" + ex.Message, "Export failed", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        private static string EscapeCsv(string s)
        {
            if (s.Contains('"') || s.Contains(',') || s.Contains('\n') || s.Contains('\r'))
            {
                s = s.Replace("\"", "\"\"");
                return $"\"{s}\"";
            }

            return s;
        }
    }
}
