using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Extensions
{
    public static class IListExtensions
    {
        public static void RemoveRange<T>(this IList<T> source, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                source.Remove(item);
            }
        }
    }
}
