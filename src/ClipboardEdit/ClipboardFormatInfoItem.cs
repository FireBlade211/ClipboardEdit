using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardEdit
{
    /// <summary>
    /// Represents information about a clipboard format item for viewing in a <see cref="PropertyGrid"/>.
    /// </summary>
    public class ClipboardFormatInfoItem
    {
        internal IntPtr _size;
        internal string _fmt;
        internal string _tfmt;
        internal uint _ufmt;
        internal string _wide;
        internal string _ansi;
        internal string _ascii;
        internal sbyte _byte;
        internal byte _ubyte;
        internal short _short;
        internal ushort _ushort;
        internal int _int;
        internal uint _uint;
        internal long _long;
        internal ulong _ulong;
        internal float _float;
        internal double _db;
        internal DateTime? _filetime;
        internal DateTime? _lfiletime;

        [Category("Data")]
        [Description("The size of the data in the clipboard, in bytes.")]
        public IntPtr Size => _size;

        [Category("Data")]
        [Description("The size of the data in the clipboard, in bits.")]
        [DisplayName("Size in Bits")]
        public IntPtr BitSize => new IntPtr(_size.ToInt64() * 8);

        [Category("Data Formats")]
        [Description("The clipboard data represented as a wide (UTF-16) string.")]
        [DisplayName("As Wide String")]
        public string WideString => _wide;

        [Category("Data Formats")]
        [Description("The clipboard data represented as an ANSI string.")]
        [DisplayName("As ANSI String")]
        public string AnsiString => _ansi;

        [Category("Data Formats")]
        [Description("The clipboard data represented as an ASCII string.")]
        [DisplayName("As ASCII String")]
        public string AsciiString => _ascii;

        [Category("Data Formats")]
        [Description("The clipboard data represented as a signed 8-bit integer (byte).")]
        [DisplayName("As Byte (8 bits)")]
        public sbyte Byte => _byte;

        [Category("Data Formats")]
        [Description("The clipboard data represented as an unsigned 8-bit integer (byte).")]
        [DisplayName("As Unsigned Byte (8 bits)")]
        public byte UByte => _ubyte;

        [Category("Data Formats")]
        [Description("The clipboard data represented as a signed 16-bit integer.")]
        [DisplayName("As 16-Bit Integer")]
        public short Short => _short;

        [Category("Data Formats")]
        [Description("The clipboard data represented as an unsigned 16-bit integer.")]
        [DisplayName("As Unsigned 16-Bit Integer")]
        public ushort UShort => _ushort;

        [Category("Data Formats")]
        [Description("The clipboard data represented as a signed 32-bit integer.")]
        [DisplayName("As 32-Bit Integer")]
        public int Int => _int;

        [Category("Data Formats")]
        [Description("The clipboard data represented as an unsigned 32-bit integer.")]
        [DisplayName("As Unsigned 32-Bit Integer")]
        public uint UInt => _uint;

        [Category("Data Formats")]
        [Description("The clipboard data represented as a signed 64-bit integer.")]
        [DisplayName("As 64-Bit Integer")]
        public long Long => _long;

        [Category("Data Formats")]
        [Description("The clipboard data represented as an unsigned 64-bit integer.")]
        [DisplayName("As Unsigned 64-Bit Integer")]
        public ulong ULong => _ulong;

        [Category("Data Formats")]
        [Description("The clipboard data represented as a single precision floating-point number.")]
        [DisplayName("As Single-Precision Floating Point")]
        public float Float => _float;

        [Category("Data Formats")]
        [Description("The clipboard data represented as a double precision floating-point number.")]
        [DisplayName("As Double-Precision Floating Point")]
        public double Double => _db;

        [Category("Data Formats")]
        [Description("The clipboard data represented as a Windows FILETIME (UTC time).")]
        [DisplayName("As File Time")]
        public DateTime? FileTime => _filetime;

        [Category("Data Formats")]
        [Description("The clipboard data represented as a Windows FILETIME (local time).")]
        [DisplayName("As Local File Time")]
        public DateTime? LocalFileTime => _lfiletime;

        [Category("Format")]
        [Description("The name of the clipboard format.")]
        [DisplayName("Format Name")]
        public string FormatName => _fmt;

        [Category("Format")]
        [Description("The ID of the format.")]
        [DisplayName("Format ID")]
        public uint FormatId => _ufmt;

        [Category("Format")]
        [Description("The type of the format.")]
        [DisplayName("Format Type")]
        public string FormatType => _tfmt;
    }
}
