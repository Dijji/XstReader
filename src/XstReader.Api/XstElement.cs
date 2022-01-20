using System;
using System.Collections.Generic;
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


        private XstPropertySet _XstPropertySet = null;
        internal protected XstPropertySet XstPropertySet => _XstPropertySet ?? (_XstPropertySet = new XstPropertySet(LoadProperties));
        public IEnumerable<XstProperty> Properties => GetProperties();

        private string _DisplayName = null;
        public string DisplayName
        {
            get => _DisplayName??XstPropertySet[PropertyCanonicalName.PidTagDisplayName]?.Value;
            protected set => _DisplayName = value;
        }
        public DateTime? LastModificationTime => XstPropertySet[PropertyCanonicalName.PidTagLastModificationTime]?.Value;


        #region Properties
        private protected abstract IEnumerable<XstProperty> LoadProperties();

        internal void AddProperty(XstProperty property)
            => XstPropertySet.Add(property);

        public IEnumerable<XstProperty> GetProperties()
            => XstPropertySet.PropertiesNonBinary;

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
