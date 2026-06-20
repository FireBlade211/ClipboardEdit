using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardEdit.Helpers
{
    public static class EncodingExtensions
    {
        public static string GetStringOrDefault(this Encoding enc, byte[] data, string fallback = null)
        {
            try
            {
                var clone = (Encoding)enc.Clone();
                clone.DecoderFallback = DecoderFallback.ExceptionFallback;

                var str = clone.GetString(data);
                return str;
            }
            catch { }

            return fallback;
        }
    }
}
