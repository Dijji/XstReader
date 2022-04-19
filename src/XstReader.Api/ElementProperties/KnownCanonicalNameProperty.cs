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
using System.Text;
using System.Threading.Tasks;

namespace XstReader.ElementProperties
{
    /// <summary>
    /// Class with information of a Known CanonicalName Property, from the documentation at 
    /// <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/../ms-oxcfold/c0f31b95-c07f-486c-98d9-535ed9705fbf">
    /// [MS-OXCFOLD]</see>
    /// </summary>
    public class KnownCanonicalNameProperty
    {
        /// <summary>
        /// Canonical Name of the Property
        /// </summary>
        public string CanonicalName { get; internal set; }

        /// <summary>
        /// The Friendly Name of the property
        /// </summary>
        public string FriendlyName { get; internal set; }

        /// <summary>
        /// The Category of the Property
        /// </summary>
        public string Category { get; internal set; }

        /// <summary>
        /// Area of the Property
        /// </summary>
        public PropertyArea Area { get; internal set; }

        /// <summary>
        /// Set of the Property
        /// </summary>
        public PropertySet Set { get; internal set; }

        /// <summary>
        /// The Alternate Names of the Property
        /// </summary>
        public IEnumerable<string> AlternateNames { get; internal set; }

        /// <summary>
        /// The Description of the Property
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// The Description in Html format of the Property
        /// </summary>
        public string HtmlDescription { get; internal set; }

        /// <summary>
        /// The identifier of the Property
        /// This id is not unique in the Documentation!
        /// </summary>
        public UInt16 Id { get; internal set; }
    }
}
