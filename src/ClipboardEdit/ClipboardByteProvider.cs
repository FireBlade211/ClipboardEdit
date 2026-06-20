using Be.Windows.Forms;
using ClipboardEdit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using System.Runtime.InteropServices;
using Windows.Win32.System.Memory;

namespace ClipboardEdit
{
    public class ClipboardByteProvider : DynamicByteProvider
    {
        private nint _hwnd = 0;
        private bool _lock = false;
        private ClipboardEntryInfo _entry;

        public ClipboardByteProvider(nint hwnd) : base(Array.Empty<byte>())
        {
            _hwnd = hwnd;
        }

        public ClipboardByteProvider(ClipboardEntryInfo entry, nint hwnd) : base(entry.Data)
        {
            _hwnd = hwnd;
            _entry = entry;
        }

        public bool Lock => _lock;
        public ClipboardEntryInfo Entry
        {
            get => _entry;
            set
            {
                _entry = value;
                base.ApplyChanges();

                Bytes.Replace(_entry.Data);
            }
        }

        #region R/W Utils
        private enum ClipboardFormatKind
        {
            HGlobal,
            Bitmap,
            EnhancedMetafile,
            MetafilePict,
            Ignored
        }

        private sealed class ClipboardFormatEntry
        {
            public uint Format;
            public string Name;

            public ClipboardFormatKind Kind;

            public byte[] Data;
        }

        private static List<ClipboardFormatEntry> ReadClipboard(HWND hwnd)
        {
            var result = new List<ClipboardFormatEntry>();

            PInvoke.OpenClipboard(hwnd);

            try
            {
                uint format = 0;

                while ((format = PInvoke.EnumClipboardFormats(format)) != 0)
                {
                    var entry = new ClipboardFormatEntry
                    {
                        Format = format,
                        Name = ClipboardHelper.GetPredefinedFormatName(format),
                        Kind = GetKind(format),
                        Data = ClipboardHelper.GetData(format)
                    };

                    result.Add(entry);
                }
            }
            finally
            {
                PInvoke.CloseClipboard();
            }

            return result;
        }

        private unsafe static void WriteClipboard(HWND hwnd, List<ClipboardFormatEntry> entries)
        {
            PInvoke.OpenClipboard(hwnd);

            try
            {
                PInvoke.EmptyClipboard();

                foreach (var e in entries)
                {
                    HANDLE handle = e.Kind switch
                    {
                        ClipboardFormatKind.HGlobal =>
                            WriteHGlobal(e.Data),

                        ClipboardFormatKind.Bitmap =>
                            WriteBitmap(e.Data),

                        ClipboardFormatKind.EnhancedMetafile =>
                            WriteEnhMetafile(e.Data),

                        ClipboardFormatKind.MetafilePict =>
                            WriteMetafilePict(e.Data),

                        _ => default
                    };

                    if ((IntPtr)handle.Value != IntPtr.Zero)
                    {
                        PInvoke.SetClipboardData(e.Format, handle);
                    }
                }
            }
            finally
            {
                PInvoke.CloseClipboard();
            }
        }

        private unsafe static HANDLE WriteHGlobal(byte[] data)
        {
            HGLOBAL mem = PInvoke.GlobalAlloc(
                GLOBAL_ALLOC_FLAGS.GMEM_MOVEABLE,
                (nuint)data.Length);

            IntPtr p = (IntPtr)PInvoke.GlobalLock(mem);

            try
            {
                Marshal.Copy(data, 0, p, data.Length);
            }
            finally
            {
                PInvoke.GlobalUnlock(mem);
            }

            return new HANDLE(mem.Value);
        }

        private unsafe static HANDLE WriteBitmap(byte[] dib)
        {
            fixed (byte* p = dib)
            {
                BITMAPINFO* bmi = (BITMAPINFO*)p;

                void* bits = (byte*)p + bmi->bmiHeader.biSize;

                HBITMAP hbm = PInvoke.CreateDIBitmap(
                    PInvoke.GetDC(HWND.Null),
                    &bmi->bmiHeader,
                    0,
                    bits,
                    bmi,
                    DIB_USAGE.DIB_RGB_COLORS);

                return new HANDLE(hbm.Value);
            }
        }

        private unsafe static HANDLE WriteEnhMetafile(byte[] emf)
        {
            fixed (byte* p = emf)
            {
                HENHMETAFILE h = PInvoke.SetEnhMetaFileBits((uint)emf.Length, p);
                return new HANDLE(h.Value);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct METAFILEPICT
        {
            public int mm;
            public int xExt;
            public int yExt;
            public nint hMF;
        }

        private unsafe static HANDLE WriteMetafilePict(byte[] data)
        {
            fixed (byte* p = data)
            {
                HGLOBAL mem = PInvoke.GlobalAlloc(
                    GLOBAL_ALLOC_FLAGS.GMEM_MOVEABLE,
                    (nuint)data.Length);

                IntPtr dst = (IntPtr)PInvoke.GlobalLock(mem);

                try
                {
                    Marshal.Copy(data, 0, dst, data.Length);

                    return new HANDLE(mem.Value);
                }
                finally
                {
                    PInvoke.GlobalUnlock(mem);
                }
            }
        }

        private static ClipboardFormatKind GetKind(uint format)
        {
            if (format is ClipboardHelper.CF_BITMAP)
                return ClipboardFormatKind.Bitmap;

            if (format is ClipboardHelper.CF_ENHMETAFILE)
                return ClipboardFormatKind.EnhancedMetafile;

            if (format is ClipboardHelper.CF_METAFILEPICT)
                return ClipboardFormatKind.MetafilePict;

            return ClipboardFormatKind.HGlobal;
        }
        #endregion

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            if (_entry == null) return;

            _lock = true;

            var entries = ReadClipboard(new HWND(_hwnd));

            if (entries.FirstOrDefault(x => x.Format == _entry.Format) is ClipboardFormatEntry entry)
                entry.Data = Bytes.ToArray(); // clone the collection

            WriteClipboard(new HWND(_hwnd), entries);

            _lock = false;
        }
    }
}
