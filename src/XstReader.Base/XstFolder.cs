// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System.Collections.Generic;
using System.Linq;
using XstReader.Properties;

namespace XstReader
{
    public class XstFolder
    {
        public XstFile XstFile { get; set; }
        private LTP Ltp => XstFile.Ltp;
        private NDB Ndb => XstFile.Ndb;

        public string Name { get; set; }

        internal NID Nid { get; set; }  // Where folder data is held

        public XstFolder ParentFolder { get; set; }
        private List<XstFolder> _Folders = null;
        public List<XstFolder> Folders => GetFolders();
        public bool HasSubFolders => Folders.Count > 0;

        private string _Path = null;
        public string Path => _Path ?? (_Path = string.IsNullOrEmpty(ParentFolder?.Name) ? Name : $"{ParentFolder.Path}\\{Name}");

        public uint ContentCount { get; set; } = 0;
        private List<XstMessage> _Messages = null;
        public List<XstMessage> Messages => GetMessages();

        #region PropertyGetters
        // We use sets of PropertyGetters to define the equivalent of queries when reading property sets and tables

        // The folder properties we read when exploring folder structure
        private static readonly PropertyGetters<XstFolder> pgFolder = new PropertyGetters<XstFolder>
        {
            {EpropertyTag.PidTagDisplayName, (f, val) => f.Name = val },
            {EpropertyTag.PidTagContentCount, (f, val) => f.ContentCount = val },
            // Don't bother reading HasSubFolders, because it is not always set
            // {EpropertyTag.PidTagSubfolders, (f, val) => f.HasSubFolders = val },
        };

        // When reading folder contents, the message properties we ask for
        // In Unicode4K, PidTagSentRepresentingNameW doesn't yield a useful value
        private static readonly PropertyGetters<XstMessage> pgMessageList4K = new PropertyGetters<XstMessage>
        {
            {EpropertyTag.PidTagSubjectW, (m, val) => m.Subject = val },
            {EpropertyTag.PidTagDisplayCcW, (m, val) => m.Cc = val },
            {EpropertyTag.PidTagDisplayToW, (m, val) => m.To = val },
            {EpropertyTag.PidTagMessageFlags, (m, val) => m.Flags = (MessageFlags)val },
            {EpropertyTag.PidTagClientSubmitTime, (m, val) => m.Submitted = val },
            {EpropertyTag.PidTagMessageDeliveryTime, (m, val) => m.Received = val },
            {EpropertyTag.PidTagLastModificationTime, (m, val) => m.Modified = val },
        };

        // When reading folder contents, the message properties we ask for
        private static readonly PropertyGetters<XstMessage> pgMessageList = new PropertyGetters<XstMessage>
        {
            {EpropertyTag.PidTagSubjectW, (m, val) => m.Subject = val },
            {EpropertyTag.PidTagDisplayCcW, (m, val) => m.Cc = val },
            {EpropertyTag.PidTagDisplayToW, (m, val) => m.To = val },
            {EpropertyTag.PidTagMessageFlags, (m, val) => m.Flags = (MessageFlags)val },
            {EpropertyTag.PidTagSentRepresentingNameW, (m, val) => m.From = val },
            {EpropertyTag.PidTagClientSubmitTime, (m, val) => m.Submitted = val },
            {EpropertyTag.PidTagMessageDeliveryTime, (m, val) => m.Received = val },
            {EpropertyTag.PidTagLastModificationTime, (m, val) => m.Modified = val },
        };

        private static readonly PropertyGetters<XstMessage> pgMessageDetail4K = new PropertyGetters<XstMessage>
        {
            {EpropertyTag.PidTagSentRepresentingNameW, (m, val) => m.From = val },
            {EpropertyTag.PidTagSentRepresentingEmailAddress, (m, val) => { if(m.From == null) m.From = val; } },
            {EpropertyTag.PidTagSenderName, (m, val) => { if(m.From == null) m.From = val; } },
        };

        #endregion PropertyGetters

        #region Ctor
        internal XstFolder(XstFile xstFile, NID nid, XstFolder parentFolder = null)
        {
            XstFile = xstFile;
            Nid = nid;
            ParentFolder = parentFolder;
            Ltp.ReadProperties<XstFolder>(nid, pgFolder, this);
        }
        #endregion Ctor

        #region Folders
        public List<XstFolder> GetFolders()
        {
            if (_Folders == null)
                _Folders = Ltp.ReadTableRowIds(NID.TypedNID(EnidType.HIERARCHY_TABLE, Nid))
                              .Where(id => id.nidType == EnidType.NORMAL_FOLDER)
                              .Select(id => new XstFolder(XstFile, id, this))
                              .OrderBy(sf => sf.Name)
                              .ToList();

            return _Folders;
        }
        public void ClearFolders()
            => _Folders = null;
        #endregion Folders

        #region Messages
        public List<XstMessage> GetMessages()
        {
            if (_Messages == null)
            {
                if (ContentCount > 0)
                    // Get the Contents table for the folder
                    // For 4K, not all the properties we want are available in the Contents table, so supplement them from the Message itself
                    _Messages = Ltp.ReadTable<XstMessage>(NID.TypedNID(EnidType.CONTENTS_TABLE, Nid),
                                                       Ndb.IsUnicode4K ? pgMessageList4K : pgMessageList, (m, id) => m.Nid = new NID(id))
                                   .Select(m => Ndb.IsUnicode4K ? Add4KMessageProperties(m) : m)
                                   .Select(m => { m.Folder = this; return m; })
                                   .ToList(); // to force complete execution on the current thread

                else
                    _Messages = new List<XstMessage>();
            }
            return _Messages;
        }

        public void ClearMessages()
            => _Messages = null;

        private XstMessage Add4KMessageProperties(XstMessage m)
        {
            Ltp.ReadProperties<XstMessage>(m.Nid, pgMessageDetail4K, m);
            return m;
        }

        #endregion Messages
    }
}
