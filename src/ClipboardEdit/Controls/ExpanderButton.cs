using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.UI.Controls;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ClipboardEdit.Controls
{
    public class ExpanderButton : Control
    {
        public ExpanderButton()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.Selectable
                | ControlStyles.UseTextForAccessibility
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint, true);
        }

        private const int TDLG_EXPANDOBUTTON = 13;
        private const int TDLGEBS_EXPANDEDNORMAL = 4;
        private const int TDLGEBS_EXPANDEDPRESSED = 6;
        private const int TDLGEBS_EXPANDEDHOVER = 5;
        private const int TDLGEBS_NORMAL = 1;
        private const int TDLGEBS_PRESSED = 3;
        private const int TDLGEBS_HOVER = 2;

        private bool _expanded;

        public bool Expanded
        {
            get => _expanded;
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    Refresh();

                    ExpandedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private bool _hover;
        private bool _pressed;

        public event EventHandler ExpandedChanged;

        protected unsafe override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            SIZE size = new();

            // We don't have access to WinInteropUtils here so we have to call the APIs manually,
            // because WinInteropUtils requires .NET 8+ (unless I make a Compatibility version in the future)
            fixed (char* szClassList = "TASKDIALOG")
            {
                HTHEME hTheme = PInvoke.OpenThemeData(new HWND(Handle), new PCWSTR(szClassList));

                if (!hTheme.IsNull)
                {
                    IntPtr hdc = e.Graphics.GetHdc();

                    int state = _expanded
                        ? TDLGEBS_EXPANDEDNORMAL
                        : TDLGEBS_NORMAL;

                    if (_pressed)
                        state = _expanded
                        ? TDLGEBS_EXPANDEDPRESSED
                        : TDLGEBS_PRESSED;

                    else if (_hover)
                        state = _expanded
                        ? TDLGEBS_EXPANDEDHOVER
                        : TDLGEBS_HOVER;

                    PInvoke.GetThemePartSize(hTheme, new HDC(hdc), TDLG_EXPANDOBUTTON, state,
                        null, THEMESIZE.TS_TRUE, &size);

                    RECT rc = new RECT(0, 0, size.Width, size.Height);

                    PInvoke.DrawThemeBackground(hTheme, new HDC(hdc), TDLG_EXPANDOBUTTON, state,
                       &rc, null);

                    PInvoke.CloseThemeData(hTheme);

                    e.Graphics.ReleaseHdc(hdc);
                }
            }

            using (Brush textBrush = new SolidBrush(ForeColor))
            {
                SizeF textSize = e.Graphics.MeasureString(Text, Font);
                e.Graphics.DrawString(Text, Font, textBrush, new PointF(size.Width + 4, (Height / 2) - (textSize.Height / 2)));
            }

            // Draw a focus rectangle if the expander is focused.
            // We subtract 1 from the width and height to prevent the edge from getting clipped.
            if (Focused)
                using (Pen pen = new Pen(Color.Black, 1) { DashStyle = DashStyle.Dot })
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            _pressed = true;
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _pressed = false;
            Expanded = !Expanded;

            Refresh();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            _hover = true;
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _hover = false;
            Refresh();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Space)
            {
                _pressed = true;
                Refresh();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.Space)
            {
                _pressed = false;
                Expanded = !Expanded;

                Refresh();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Refresh();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            // Ensure all states are reset correctly, else they can get stuck if the user holds down the Space key
            // and tabs over to another control at the same time.
            _pressed = false;
            _hover = false;

            Refresh();
        }
    }
}
