// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System.Collections.Generic;
using System.Linq;
using XstReader.Common.BTrees;
using XstReader.Properties;

namespace XstReader
{
    public class XstFolder
    {
        public XstFile XstFile { get; set; }
        internal LTP Ltp => XstFile.Ltp;
        internal NDB Ndb => XstFile.Ndb;

        public string Name { get; set; }

        internal NID Nid { get; set; }  // Where folder data is held

        public XstFolder ParentFolder { get; set; }
        private IEnumerable<XstFolder> _Folders = null;
        public IEnumerable<XstFolder> Folders => GetFolders();
        public bool HasSubFolders => Folders.Any();

        private string _Path = null;
        public string Path => _Path ?? (_Path = string.IsNullOrEmpty(ParentFolder?.Name) ? Name : $"{ParentFolder.Path}\\{Name}");

        public uint ContentCount { get; set; } = 0;
        private IEnumerable<XstMessage> _Messages = null;
        public IEnumerable<XstMessage> Messages => GetMessages();

        internal BTree<Node> SubnodeTreeProperties = null;

        
        #region Ctor
        internal XstFolder(XstFile xstFile, NID nid, XstFolder parentFolder = null)
        {
            XstFile = xstFile;
            Nid = nid;
            ParentFolder = parentFolder;
            SubnodeTreeProperties = Ltp.ReadProperties<XstFolder>(nid, PropertyGetters.FolderProperties, this);
        }
        #endregion Ctor

        #region Folders
        public IEnumerable<XstFolder> GetFolders()
        {
            if (_Folders == null)
                _Folders = Ltp.ReadTableRowIds(NID.TypedNID(EnidType.HIERARCHY_TABLE, Nid))
                              .Where(id => id.nidType == EnidType.NORMAL_FOLDER)
                              .Select(id => new XstFolder(XstFile, id, this))
                              .OrderBy(sf => sf.Name);

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
                                                          Ndb.IsUnicode4K ? PropertyGetters.MessageList4KProperties : PropertyGetters.MessageListProperties, (m, id) => m.Nid = new NID(id))
                                   .Select(m => Ndb.IsUnicode4K ? Add4KMessageProperties(m) : m)
                                   .Select(m => m.Initialize(this));
                else
                    _Messages = new XstMessage[0];
            }
            return _Messages;
        }

        private void ClearMessages()
        {
            if(_Messages!=null)
            {
                foreach (var message in _Messages)
                    message.ClearContents();
                _Messages = null;
            }
        }

        private XstMessage Add4KMessageProperties(XstMessage m)
        {
            Ltp.ReadProperties<XstMessage>(m.Nid, PropertyGetters.MessageDetail4KProperties, m);
            return m;
        }

        #endregion Messages

        public void ClearContents()
        {
            ClearForlders();
            ClearMessages();
        }
    }
}
