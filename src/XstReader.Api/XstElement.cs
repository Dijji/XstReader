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
using System.ComponentModel;
using System.Threading;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// Base class for an element inside a pst or ost file
    /// </summary>
    public abstract class XstElement
    {
        /// <summary>
        /// The Type of the element
        /// </summary>
        [DisplayName("Type")]
        [Category(@"General")]
        [Description(@"The internal type of the XstElement")]
        public XstElementType ElementType { get; private set; }


        /// <summary>
        /// The Container File
        /// </summary>
        [DisplayName("File")]
        [Category("General")]
        [Description("The Container File")]
        public abstract XstFile XstFile { get; }
        private protected LTP Ltp => XstFile.Ltp;
        private protected NDB Ndb => XstFile.Ndb;
        internal NID Nid { get; set; } //Where element data is held


        private XstPropertySet _Properties = null;
        /// <summary>
        /// The Properties of the Element
        /// </summary>
        [Browsable(false)]
        public XstPropertySet Properties
        {
            get => _Properties ?? (_Properties = new XstPropertySet(LoadProperties, LoadProperty, CheckProperty));
            protected set => _Properties = value;
        }

        private string _DisplayName = null;
        /// <summary>
        /// The Name of the Element
        /// </summary>
        [DisplayName("Display Name")]
        [Category(@"Mapi Common")]
        [Description(@"Contains the display name of the folder.")]
        public string DisplayName
        {
            get => _DisplayName ?? (_DisplayName = Properties[PropertyCanonicalName.PidTagDisplayName]?.ValueAsStringSanitized);
            protected set => _DisplayName = value.SanitizeControlChars();
        }
        /// <summary>
        /// The last modification time of the Element
        /// </summary>
        [DisplayName("Last Modification Time")]
        [Category(@"Message Time Properties")]
        [Description(@"Contains the time, in UTC, of the last modification to the object.")]
        public DateTime? LastModificationTime => Properties[PropertyCanonicalName.PidTagLastModificationTime]?.Value;

        #region Properties
        private protected abstract IEnumerable<XstProperty> LoadProperties();

        private protected abstract XstProperty LoadProperty(PropertyCanonicalName tag);

        private protected abstract bool CheckProperty(PropertyCanonicalName tag);

        internal void AddProperty(XstProperty property)
            => Properties.Add(property);

        /// <summary>
        /// Returns all the Properties of the Element
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XstProperty> GetProperties()
            => Properties.Items.NonBinary();

        #endregion Properties

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="elementType"></param>
        protected XstElement(XstElementType elementType)
        {
            ElementType = elementType;
        }
        #endregion Ctor

        private protected void ClearProperties()
        {
            Properties.ClearContents();
            //_Properties = null;
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

        /// <summary>
        /// Gets the String representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => DisplayName;
    }
}
