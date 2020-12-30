using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace XstReader
{
    static public class Extensions
    { 
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> f)
            => e.SelectMany(c => f(c).Flatten(f)).Concat(e);
        
    }
}
