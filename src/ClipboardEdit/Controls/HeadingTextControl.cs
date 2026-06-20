using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.UI.Controls;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using System;

namespace ClipboardEdit.Controls
{
    public class HeadingTextControl : Control
    {
        protected unsafe override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            fixed (char* pszClassList = "TEXTSTYLE")
            {
                nint hdc = e.Graphics.GetHdc();

                HTHEME hTheme = PInvoke.OpenThemeData(new HWND(Handle), new PCWSTR(pszClassList));
                if (!hTheme.IsNull)
                {
                    fixed (char* pszText = Text)
                    {
                        RECT rc = new RECT(0, 0, Width, Height);

                        PInvoke.DrawThemeText(hTheme, new HDC(hdc), 1, 0, pszText, -1,
                            DRAW_TEXT_FORMAT.DT_SINGLELINE | DRAW_TEXT_FORMAT.DT_LEFT, 0, &rc);
                    }

                    PInvoke.CloseThemeData(hTheme);
                }

                e.Graphics.ReleaseHdc(hdc);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Refresh();
        }
    }
}
