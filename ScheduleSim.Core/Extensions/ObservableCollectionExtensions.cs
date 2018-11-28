using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> collection)
        {
            if (ValidateAddCollectionCount(source, collection))
            {
                return;
            }

            var itProperty = typeof(ObservableCollection<T>).GetProperty("Items", BindingFlags.NonPublic | BindingFlags.Instance);
            var colResetMethod = typeof(ObservableCollection<T>).GetMethod("OnCollectionReset", BindingFlags.NonPublic | BindingFlags.Instance);

            var list = itProperty.GetValue(source) as List<T>;
            if (list != null)
            {
                list.AddRange(collection);
                colResetMethod.Invoke(source, null);
            }
        }

        private const int switchForeachThresold = 0;
        private static bool ValidateAddCollectionCount<T>(ObservableCollection<T> source, IEnumerable<T> collection)
        {
            var count = collection.Count();
            if (count <= switchForeachThresold)
            {
                foreach (var item in collection)
                {
                    source.Add(item);
                }
                return true;
            }

            return false;
        }

        public static void RemoveRange<T>(this ObservableCollection<T> source, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                source.Remove(item);
            }
        }
    }
}
