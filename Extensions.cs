// Copyright (c) 2020, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
