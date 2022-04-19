// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace XstReader.ElementProperties
{
    /// <summary>
    /// Class with information of all documented (known) CanonicalName Properties, from the documentation at 
    /// <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/../ms-oxcfold/c0f31b95-c07f-486c-98d9-535ed9705fbf">
    /// [MS-OXCFOLD]</see>
    /// </summary>
    public static partial class KnownCanonicalNames
    {
        private static Dictionary<string, KnownCanonicalNameProperty> _DicKnownProperties = null;
        private static Dictionary<string, KnownCanonicalNameProperty> DicKnownProperties
            => _DicKnownProperties ?? (_DicKnownProperties = LoadKnownProperties());

        /// <summary>
        /// All the Known Canonical Name Properties, defined at
        /// <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/../ms-oxcfold/c0f31b95-c07f-486c-98d9-535ed9705fbf">
        /// [MS-OXCFOLD]</see>
        /// </summary>
        public static IEnumerable<KnownCanonicalNameProperty> KnonwnProperties => DicKnownProperties.Values;

        /// <summary>
        /// Gets the Known Property with Name
        /// </summary>
        /// <param name="canonicalName"></param>
        /// <returns></returns>
        public static KnownCanonicalNameProperty GetByCanonicalName(string canonicalName)
            => DicKnownProperties.ContainsKey(canonicalName) ? DicKnownProperties[canonicalName] : null;


        /// <summary>
        /// Gets the Known Property with Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<KnownCanonicalNameProperty> GetById(UInt16 id)
            => DicKnownProperties.Values.Where(p => p.Id == id);

        /// <summary>
        /// Gets the Known Property with its Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static IEnumerable<KnownCanonicalNameProperty> GetByEnum(PropertyCanonicalName tag)
            => GetById((UInt16)tag);
    }
}
