using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardEdit
{
    public partial class DocUpdateProgressDialog : Form
    {
        public DocUpdateProgressDialog()
        {
            InitializeComponent();
        }

        private bool _center = false;

        public void Center()
        {
            if (IsHandleCreated) CenterToScreen();
            else _center = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_center) CenterToScreen();
        }
    }
}
