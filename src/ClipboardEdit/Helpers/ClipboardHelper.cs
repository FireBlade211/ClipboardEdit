using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace ClipboardEdit.Helpers
{
    /// <summary>
    /// Provides helper methods for the clipboard.
    /// </summary>
    public static class ClipboardHelper
    {
        public static string GetPredefinedFormatName(uint format) => format switch
        {
            CF_DSPENHMETAFILE => nameof(CF_DSPENHMETAFILE),
            CF_DSPMETAFILEPICT => nameof(CF_DSPMETAFILEPICT),
            CF_ENHMETAFILE => nameof(CF_ENHMETAFILE),
            CF_METAFILEPICT => nameof(CF_METAFILEPICT),
            CF_BITMAP => nameof(CF_BITMAP),
            CF_DSPBITMAP => nameof(CF_DSPBITMAP),
            CF_PALETTE => nameof(CF_PALETTE),
            CF_DIB => nameof(CF_DIB),
            CF_DIBV5 => nameof(CF_DIBV5),
            CF_DSPTEXT => nameof(CF_DSPTEXT),
            CF_OEMTEXT => nameof(CF_OEMTEXT),
            CF_TEXT => nameof(CF_TEXT),
            CF_UNICODETEXT => nameof(CF_UNICODETEXT),
            CF_HDROP => nameof(CF_HDROP),
            CF_DIF => nameof(CF_DIF),
            CF_LOCALE => nameof(CF_LOCALE),
            CF_OWNERDISPLAY => nameof(CF_OWNERDISPLAY),
            CF_PENDATA => nameof(CF_PENDATA),
            CF_RIFF => nameof(CF_RIFF),
            CF_SYLK => nameof(CF_SYLK),
            CF_TIFF => nameof(CF_TIFF),
            CF_WAVE => nameof(CF_WAVE),
            _ => $"CF_{format}"
        };

        public static byte[] GetData(uint format)
        {
            HANDLE h = PInvoke.GetClipboardData(format);

            if (h == IntPtr.Zero)
                return Array.Empty<byte>();

            if (IsBitmapFormat(format))
                return ReadBitmapAsDIB(h);

            if (IsMetafileFormat(format))
                return ReadMetafile(h, format);

            return ReadHGlobal(h);
        }

        public static bool IsBitmapFormat(uint f) => f is CF_BITMAP or CF_DSPBITMAP;

        public static bool IsMetafileFormat(uint f) => f is CF_ENHMETAFILE or CF_METAFILEPICT
                   or CF_DSPENHMETAFILE or CF_DSPMETAFILEPICT;

        private unsafe static byte[] ReadHGlobal(HANDLE h)
        {
            nuint size = PInvoke.GlobalSize(new HGLOBAL(h.Value));
            nint p = (nint)PInvoke.GlobalLock(new HGLOBAL(h.Value));

            try
            {
                byte[] data = new byte[(int)size];
                Marshal.Copy(p, data, 0, data.Length);
                return data;
            }
            finally
            {
                PInvoke.GlobalUnlock(new HGLOBAL(h.Value));
            }
        }

        private static byte[] ReadBitmapAsDIB(HANDLE hBitmap)
        {
            IntPtr hdc = PInvoke.GetDC(HWND.Null);

            try
            {
                BITMAP bmp;
                
                unsafe
                {
                    PInvoke.GetObject(new HGDIOBJ(hBitmap.Value), Marshal.SizeOf<BITMAP>(), &bmp);

                    int width = bmp.bmWidth;
                    int height = bmp.bmHeight;

                    BITMAPINFO bi = new BITMAPINFO
                    {
                        bmiHeader =
                        {
                            biSize = (uint)Marshal.SizeOf<BITMAPINFOHEADER>(),
                            biWidth = width,
                            biHeight = height,
                            biPlanes = 1,
                            biBitCount = 32,
                            biCompression = BI_RGB
                        }
                    };

                    int imageSize = width * height * 4;
                    fixed (byte* pixels = new byte[imageSize])
                    {
                        PInvoke.GetDIBits(
                            new HDC(hdc),
                            new HBITMAP(hBitmap.Value),
                            0,
                            (uint)height,
                            pixels,
                            &bi,
                            DIB_USAGE.DIB_RGB_COLORS
                        );

                        return new Span<byte>(pixels, imageSize).ToArray();
                    }
                }
            }
            finally
            {
                PInvoke.ReleaseDC(HWND.Null, new HDC(hdc));
            }
        }

        private unsafe static byte[] ReadMetafile(HANDLE hEmf, uint format)
        {
            if (format == CF_METAFILEPICT)
            {
                IntPtr p = (IntPtr)PInvoke.GlobalLock(new HGLOBAL(hEmf.Value));

                if (p == IntPtr.Zero)
                    return Array.Empty<byte>();

                try
                {
                    METAFILEPICT mfp = Marshal.PtrToStructure<METAFILEPICT>(p);

                    IntPtr hMF = mfp.hMF;

                    uint size = PInvoke.GetMetaFileBitsEx(new HMETAFILE(hMF), 0, null);

                    if (size == 0)
                        return Array.Empty<byte>();

                    unsafe
                    {
                        fixed (byte* buffer = new byte[size])
                        {
                            PInvoke.GetMetaFileBitsEx(new HMETAFILE(hMF), size, buffer);

                            return new Span<byte>(buffer, (int)size).ToArray();
                        }
                    }
                }
                finally
                {
                    PInvoke.GlobalUnlock(new HGLOBAL(hEmf.Value));
                }
            }
            else
            {
                uint size = PInvoke.GetEnhMetaFileBits(new HENHMETAFILE(hEmf.Value), 0, null);

                if (size == 0)
                    return Array.Empty<byte>();

                unsafe
                {
                    fixed (byte* buffer = new byte[size])
                    {
                        PInvoke.GetEnhMetaFileBits(
                            new HENHMETAFILE(hEmf.Value),
                            size,
                            buffer
                        );

                        return new Span<byte>(buffer, (int)size).ToArray();
                    }
                }
            }
        }

        public const uint CF_DSPENHMETAFILE = 0x008E;
        public const uint CF_DSPMETAFILEPICT = 0x0083;
        public const uint CF_ENHMETAFILE = 14;
        public const uint CF_METAFILEPICT = 3;

        public const uint CF_DIF = 5;

        public const uint CF_BITMAP = 2;
        public const uint CF_DSPBITMAP = 0x0082;
        public const uint CF_PALETTE = 9;

        public const uint CF_LOCALE = 16;

        public const uint CF_DIB = 8;
        public const uint CF_DIBV5 = 17;
        
        public const uint CF_DSPTEXT = 0x0081;
        public const uint CF_OEMTEXT = 7;
        public const uint CF_TEXT = 1;
        public const uint CF_UNICODETEXT = 13;
        
        public const uint CF_HDROP = 15;

        public const uint CF_OWNERDISPLAY = 0x0080;
        public const uint CF_PENDATA = 10;
        public const uint CF_RIFF = 11;
        public const uint CF_SYLK = 4;
        public const uint CF_TIFF = 6;
        public const uint CF_WAVE = 12;
        
        public const uint BI_RGB = 0;
        public const uint DIB_RGB_COLORS = 0;

        [StructLayout(LayoutKind.Sequential)]
        private struct METAFILEPICT
        {
            public int mm;      // mapping mode
            public int xExt;    // width
            public int yExt;    // height
            public nint hMF;    // HMETAFILE
        }
    }
}
