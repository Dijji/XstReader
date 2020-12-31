// Copyright (c) 2020, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace XstReader
{
    static public class Extensions
    { 
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> f)
            => e.SelectMany(c => f(c).Flatten(f)).Concat(e);
        
    }
}
