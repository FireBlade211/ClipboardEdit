using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardEdit.Helpers
{
    public static class CollectionExtensions
    {
        public static void Replace<T>(this IList<T> col, IEnumerable<T> items)
        {
            col.Clear();
            foreach (var item in items) col.Add(item); // NOTE: AddRange only exists on List<T>, not IList<T>
        }
    }
}
