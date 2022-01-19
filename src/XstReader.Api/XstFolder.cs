// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System.Collections.Generic;
using System.Linq;
using XstReader.Common.BTrees;
using XstReader.ElementProperties;

namespace XstReader
{
    public class XstFolder : XstElement
    {
        internal protected override XstFile XstFile { get; }

        public string DisplayName => XstPropertySet[PropertyCanonicalName.PidTagDisplayName]?.Value;
        public uint ContentCount => XstPropertySet[PropertyCanonicalName.PidTagContentCount]?.Value ?? (uint)0;
        public uint ContentUnreadCount => XstPropertySet[PropertyCanonicalName.PidTagContentUnreadCount]?.Value ?? (uint)0;

        public XstFolder ParentFolder { get; set; }
        private IEnumerable<XstFolder> _Folders = null;
        public IEnumerable<XstFolder> Folders => GetFolders();
        public bool HasSubFolders => Folders.Any();

        private string _Path = null;
        public string Path => _Path ?? (_Path = string.IsNullOrEmpty(ParentFolder?.DisplayName) ? DisplayName : $"{ParentFolder.Path}\\{DisplayName}");

        private IEnumerable<XstMessage> _Messages = null;
        public IEnumerable<XstMessage> Messages => GetMessages();

        private BTree<Node> _SubnodeTreeProperties = null;


        #region Ctor
        internal XstFolder(XstFile xstFile, NID nid, XstFolder parentFolder = null)
        {
            XstFile = xstFile;
            Nid = nid;
            ParentFolder = parentFolder;
            _SubnodeTreeProperties = Ltp.ReadProperties(nid, XstPropertySet);
            //_SubnodeTreeProperties = Ltp.ReadProperties<XstFolder>(nid, PropertyGetters.FolderProperties, this);
        }
        #endregion Ctor

        #region Properties
        private protected override IEnumerable<XstProperty> LoadProperties()
            => Ltp.ReadAllProperties(Nid);

        #endregion Properties

        #region Folders
        public IEnumerable<XstFolder> GetFolders()
        {
            if (_Folders == null)
                _Folders = Ltp.ReadTableRowIds(NID.TypedNID(EnidType.HIERARCHY_TABLE, Nid))
                              .Where(id => id.nidType == EnidType.NORMAL_FOLDER)
                              .Select(id => new XstFolder(XstFile, id, this))
                              .OrderBy(sf => sf.DisplayName);

            return _Folders;
        }
        private void ClearForlders()
        {
            if (_Folders != null)
            {
                foreach (var folder in _Folders)
                    folder.ClearContents();
                _Folders = null;
            }
        }
        #endregion Folders

        #region Messages
        public IEnumerable<XstMessage> GetMessages()
        {
            if (_Messages == null)
            {
                if (ContentCount > 0)
                    // Get the Contents table for the folder
                    // For 4K, not all the properties we want are available in the Contents table, so supplement them from the Message itself
                    _Messages = Ltp.ReadTable<XstMessage>(NID.TypedNID(EnidType.CONTENTS_TABLE, Nid),
                                                          (m, id) => m.Initialize(new NID(id), this),
                                                          (m, p) => m.AddProperty(p),
                                                          m => m.ProcessSignedOrEncrypted());
                else
                    _Messages = new XstMessage[0];
            }
            return _Messages;
        }

        private void ClearMessages()
        {
            if (_Messages != null)
            {
                foreach (var message in _Messages)
                    message.ClearContents();
                _Messages = null;
            }
        }

        #endregion Messages

        public override void ClearContents()
        {
            base.ClearContents();

            ClearForlders();
            ClearMessages();
        }
    }
}
