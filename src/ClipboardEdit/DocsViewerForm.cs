using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAero.TaskDialog;

namespace ClipboardEdit
{
    public partial class DocsViewerForm : Form
    {
        public DocsViewerForm()
        {
            InitializeComponent();
        }

        public void ResizeBrowser()
        {
            var doc = webBrowser1.Document;

            if (doc?.Body == null)
                return;

            int newHeight = doc.Body.ScrollRectangle.Height;
            int delta = newHeight - webBrowser1.Height;

            webBrowser1.Top -= delta;
            webBrowser1.Height = newHeight;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ResizeBrowser();

            //Debugger.Break();
        }

        public bool AllowNavigate { get; set; }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (AllowNavigate)
                return;

            if (e.Url != null && e.Url.Scheme == "about")
                e.Cancel = true;
        }

        private void DocsViewerForm_Resize(object sender, EventArgs e)
        {
            ResizeBrowser();
        }
    }
}
