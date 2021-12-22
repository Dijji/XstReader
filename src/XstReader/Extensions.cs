// Copyright (c) 2020, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XstReader
{
    internal static class Extensions
    {
        
        public static void PopulateWith<T>(this ObservableCollection<T> collection, IEnumerable<T> list)
        {
            collection.Clear();
            foreach (T value in list)
                collection.Add(value);
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> f)
            => e.SelectMany(c => f(c).Flatten(f)).Concat(e);

    }
}
