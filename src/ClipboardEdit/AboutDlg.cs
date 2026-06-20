using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell;

namespace ClipboardEdit
{
    partial class AboutDlg : Form
    {
        public AboutDlg()
        {
            InitializeComponent();
            
            labelVersion.Text = string.Format("Version {0}", AssemblyVersion);
        }

        #region Assembly Attribute Accessors

        public string AssemblyVersion
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;

                return $"{ver.Major}.{ver.Minor}";
            }
        }
        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LICENSE");

            unsafe
            {
                fixed (char* szPath = path, cl = "txtfilelegacy")
                {
                    OPENASINFO info = new OPENASINFO();
                    info.pcszFile = new PCWSTR(szPath);
                    info.pcszClass = new PCWSTR(cl);
                    info.oaifInFlags = OPEN_AS_INFO_FLAGS.OAIF_HIDE_REGISTRATION | OPEN_AS_INFO_FLAGS.OAIF_EXEC;

                    PInvoke.SHOpenWithDialog(new HWND(Handle), info);
                }
            }
        }
    }
}
