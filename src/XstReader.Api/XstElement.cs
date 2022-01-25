// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System;
using System.Collections.Generic;
using System.Threading;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// A pst/ost element 
    /// </summary>
    public abstract class XstElement
    {
        internal protected abstract XstFile XstFile { get; }
        private protected LTP Ltp => XstFile.Ltp;
        private protected NDB Ndb => XstFile.Ndb;
        internal NID Nid { get; set; } //Where element data is held


        private XstPropertySet _Properties = null;
        public XstPropertySet Properties
        {
            get => _Properties ?? (_Properties = new XstPropertySet(LoadProperties));
            protected set => _Properties = value;
        }

        private string _DisplayName = null;
        public string DisplayName
        {
            get => _DisplayName ?? Properties[PropertyCanonicalName.PidTagDisplayName]?.Value;
            protected set => _DisplayName = value;
        }
        public DateTime? LastModificationTime => Properties[PropertyCanonicalName.PidTagLastModificationTime]?.Value;


        #region Properties
        private protected abstract IEnumerable<XstProperty> LoadProperties();

        internal void AddProperty(XstProperty property)
            => Properties.Add(property);

        public IEnumerable<XstProperty> GetProperties()
            => Properties.ItemsNonBinary;

        #endregion Properties

        private protected void ClearProperties()
        {
            Properties.ClearContents();
            _Properties = null;
        }

        /// <summary>
        /// Clear all Contents
        /// </summary>
        public void ClearContents()
        {
            new Thread(()=> { ClearContentsInternal(); GC.Collect(); }).Start();
        }

        internal virtual void ClearContentsInternal()
        {
            ClearProperties();
        }
    }
}
