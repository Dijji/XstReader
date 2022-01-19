﻿using System;
using System.Collections.Generic;

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


        private XstPropertySet _XstPropertySet = null;
        internal protected XstPropertySet XstPropertySet => _XstPropertySet ?? (_XstPropertySet = new XstPropertySet(LoadProperties));
        public IEnumerable<XstProperty> Properties => GetProperties();

        #region Properties
        private protected abstract IEnumerable<XstProperty> LoadProperties();

        public IEnumerable<XstProperty> GetProperties()
            => XstPropertySet.Values;

        private protected void ClearProperties()
        {
            XstPropertySet.ClearContents();
            _XstPropertySet = null;
        }

        #endregion Properties

        /// <summary>
        /// Clear all Contents
        /// </summary>
        public virtual void ClearContents()
        {
            ClearProperties();
        }
    }
}