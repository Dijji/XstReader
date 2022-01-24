using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstReader
{
    internal static class ObservableCollectionExtensions
    {
        public static void PopulateWith<T>(this ObservableCollection<T> collection, IEnumerable<T> list)
        {
            collection.Clear();
            foreach (T value in list)
                collection.Add(value);
        }

    }
}
