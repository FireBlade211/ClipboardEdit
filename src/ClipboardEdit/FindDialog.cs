using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ClipboardEdit
{
    public partial class FindDialog : Form
    {
        public HexBox Box { get; set; }

        private DynamicByteProvider _provider;

        public FindDialog()
        {
            InitializeComponent();

            _provider = new DynamicByteProvider(new List<byte>());
            hexBox1.ByteProvider = _provider;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Box.Find(new FindOptions
            {
                Type        = tabControl1.SelectedIndex == 0 ? FindType.Text : FindType.Hex,
                Text        = textBox1.Text,
                Hex         = _provider.Bytes.ToArray(),
                MatchCase   = checkBox1.Checked
            }) == -1)
                MessageBox.Show("ClipboardEdit has reached the end of the data.", "Reached End of Data", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Help", "help.chm");

            Help.ShowHelp(this, path, HelpNavigator.Topic, "datafind.htm");
        }
    }
}
