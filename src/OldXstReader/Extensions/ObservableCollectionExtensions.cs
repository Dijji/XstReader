// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

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
