using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Win32;

namespace ClipboardEdit
{
    public partial class FormatFillerForm : Form
    {
        public FormatFillerForm()
        {
            InitializeComponent();
        }

        private float _expandProgress = 0f;

        private void expanderButton1_ExpandedChanged(object sender, System.EventArgs e)
        {
            expanderButton1.Text = expanderButton1.Expanded ? "Hide details" : "Show details";

            //_expandProgress = expanderButton1.Expanded ? 0.0f : 2.0f;

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float target = expanderButton1.Expanded ? 2.0f : 0.0f;
            float easing = 0.3f;

            _expandProgress += (target - _expandProgress) * easing;

            this.Height = 169 + (int)(_expandProgress * 100);

            if (Math.Abs(_expandProgress - target) < 0.001f)
            {
                _expandProgress = target;
                timer1.Stop();
            }
        }

        private void WriteLog(string entry) => Invoke(() => textBox1.AppendText($"{entry}\r\n"));
        private static int _last = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            int count = (int)numericUpDown1.Value;

            textBox1.Text += $"\r\n----------------------\r\nFill started: Formats: {count}, started at: {DateTime.Now:t}\r\n" +
                $"----------------------\r\n";

            progressBar1.Maximum = count;
            progressBar1.Minimum = _last;

            int registered = 0;

            Task.Run(() =>
            {
                for (int i = _last; i < count; i++)
                {
                    string name = $"ClipboardEdit Clipboard Format {i}";
                    uint f = PInvoke.RegisterClipboardFormat(name);

                    if (f == 0)
                    {
                        WriteLog($"Failed to register format: {i}");
                        break;
                    }

                    Invoke(() => progressBar1.Value = i);
                    registered++;

                    WriteLog($"Registered format: 0x{f:X}");
                    _last++;
                }
            })
            .ContinueWith(t => Invoke(() =>
                {
                    if (t.Exception != null)
                    {
                        MessageBox.Show($"An error occured.\n\n0x{t.Exception.HResult:X}: {t.Exception.Message}", null,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        textBox1.AppendText($"An error occured: {t.Exception.Message} (0x{t.Exception.HResult:X})\r\n");
                    }
                    else if (registered == 0)
                    {
                        MessageBox.Show($"An error occured.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        textBox1.AppendText("An error occured.");
                    }
                    else
                        MessageBox.Show("The formats have been filled successfully.", "Fill successful",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    button1.Enabled = true;

                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = 100;
                    progressBar1.Value   = 0;
                }));
        }
    }
}
