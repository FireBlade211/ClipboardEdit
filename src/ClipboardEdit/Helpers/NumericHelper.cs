using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardEdit.Helpers
{
    public static class NumericHelper
    {
        public static ulong InterpretBytes(byte[] bytes, int index, int bitWidth)
        {
            int length = bitWidth / 8;

            if (index < 0 || index + length > bytes.Length)
                return 0u;

            ulong value = 0;

            for (int i = 0; i < length; i++)
            {
                value |= (ulong)bytes[index + i] << (8 * i);
            }

            return value;
        }

        public static long InterpretSignedBytes(byte[] bytes, int index, int bitWidth)
        {
            int length = bitWidth / 8;
            long value = 0;

            if (index < 0 || index + length > bytes.Length)
                return 0;

            for (int i = 0; i < length; i++)
            {
                value |= (long)bytes[index + i] << (8 * i);
            }

            return value;
        }

        public static float InterpretFloatBytes(byte[] bytes, int index, bool littleEndian = true)
        {
            if (index + 4 > bytes.Length)
                return float.NaN;

            byte[] temp = new byte[4];
            Buffer.BlockCopy(bytes, index, temp, 0, 4);

            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(temp);

            return BitConverter.ToSingle(temp, 0);
        }

        public static double InterpretDoubleBytes(byte[] bytes, int index, bool littleEndian = true)
        {
            if (index + 8 > bytes.Length)
                return double.NaN;

            ulong i = BitConverter.ToUInt64(bytes, index);

            if (!BitConverter.IsLittleEndian && littleEndian)
                i = Reverse(i);

            return BitConverter.Int64BitsToDouble((long)i);
        }

        private static uint Reverse(uint x)
        {
            return
                (x & 0x000000FFU) << 24 |
                (x & 0x0000FF00U) << 8 |
                (x & 0x00FF0000U) >> 8 |
                (x & 0xFF000000U) >> 24;
        }

        private static ulong Reverse(ulong x)
        {
            return
                ((ulong)Reverse((uint)x) << 32) |
                Reverse((uint)(x >> 32));
        }

        public static DateTime? TryCreateFileTime(long value)
        {
            try
            {
                return DateTime.FromFileTime(value);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public static DateTime? TryCreateFileTimeUtc(long value)
        {
            try
            {
                return DateTime.FromFileTimeUtc(value);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
