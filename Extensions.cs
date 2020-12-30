using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace XstReader
{
    static class Extensions
    { 
        public static void PopulateWith<T>(this ObservableCollection<T> collection, List<T> list)
        {
            collection.Clear();
            foreach (T value in list)
                collection.Add(value);
        }
    }
}
