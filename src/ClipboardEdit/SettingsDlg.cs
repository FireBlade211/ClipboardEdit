using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Globalization;
using ClipboardEdit.Properties;
using System;
using System.Linq;
using System.IO.Compression;

namespace ClipboardEdit
{
    public partial class SettingsDlg : Form
    {
        private bool _fontChangeLock = false;

        public SettingsDlg()
        {
            InitializeComponent();

            _fontChangeLock = true;
            var collection = new InstalledFontCollection();
            
            comboBox1.DataSource = collection.Families;
            comboBox1.DisplayMember = "Name";

            comboBox1.SelectedIndex =
                comboBox1.Items.Cast<FontFamily>()
                    .ToList()
                    .FindIndex(f => f.Equals(Settings.Default.MonospaceFont.FontFamily));

            numericUpDown1.Value = (decimal)Settings.Default.MonospaceFont.SizeInPoints;

            _fontChangeLock = false;
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1 || comboBox1.Items.Count <= e.Index) return;

            var family = (FontFamily)comboBox1.Items[e.Index];

            e.DrawBackground();
            e.DrawFocusRectangle();
            
            e.Graphics.DrawString(family.GetName(CultureInfo.CurrentCulture.LCID) ?? family.Name, new Font(family, 9),
                e.State.HasFlag(DrawItemState.Selected) ? Brushes.White : Brushes.Black, new PointF(e.Bounds.Left, e.Bounds.Top));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            Close(); // we cant set DialogResult because it closes too early
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings.Default.Reload();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_fontChangeLock) return;

            Settings.Default.MonospaceFont = new Font((FontFamily)comboBox1.SelectedItem, Settings.Default.MonospaceFont.SizeInPoints,
                Settings.Default.MonospaceFont.Style, GraphicsUnit.Point, Settings.Default.MonospaceFont.GdiCharSet,
                Settings.Default.MonospaceFont.GdiVerticalFont);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.MonospaceFont = new Font(Settings.Default.MonospaceFont.FontFamily, (int)numericUpDown1.Value,
                Settings.Default.MonospaceFont.Style, GraphicsUnit.Point, Settings.Default.MonospaceFont.GdiCharSet,
                Settings.Default.MonospaceFont.GdiVerticalFont);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = fontDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    comboBox1.SelectedItem = fontDialog1.Font.FontFamily;
                    numericUpDown1.Value = (decimal)fontDialog1.Font.SizeInPoints;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("For some reason, you aren't allowed to pick this font because .NET Framework sucks. Try setting" +
                    " the font through the dropdown instead.\n\n" + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) Settings.Default.SnapshotCompressionLevel = CompressionLevel.Optimal;
            else if (radioButton2.Checked) Settings.Default.SnapshotCompressionLevel = CompressionLevel.Fastest;
        }
    }
}
