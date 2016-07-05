// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.IO;
using System.Windows;

namespace XstReader
{
    // Main handling for xst (.ost and .pst) files
    //
    // The code here implements the messaging layer, which depends on and invokes the NDP and LTP layers
    //
    // The constructor provides the path to the file, and then there are just a few public methods:
    // - Read the structure of the folders
    // - Read the list of messages contained in a folder
    // - Read the contents of a message
    // - Save an attachment to a message
   
    class XstFile
    {
        private NDB ndb;
        private LTP ltp;
        private View view;


        // We use sets of PropertyGetters to define the equivalent of queries when reading property sets and tables

        // The folder properties we read when exploring folder structure
        private static readonly PropertyGetters<Folder> pgFolder = new PropertyGetters<Folder>
        {
            {EpropertyTag.PidTagDisplayName, (f, val) => f.Name = val },
            {EpropertyTag.PidTagContentCount, (f, val) => f.ContentCount = val },
            {EpropertyTag.PidTagSubfolders, (f, val) => f.HasSubFolders = val },
        };

        // When reading folder contents, the message properties we ask for
        private static readonly PropertyGetters<Message> pgMessageList = new PropertyGetters<Message>
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
        
        // The properties we read when accessing the contents of a message
        private static readonly PropertyGetters<Message> pgMessageContent = new PropertyGetters<Message>
        {
            {EpropertyTag.PidTagNativeBody, (m, val) => m.NativeBody = (BodyType)val },
            {EpropertyTag.PidTagBody, (m, val) => m.Body = val },
            //{EpropertyTag.PidTagInternetCodepage, (m, val) => m.InternetCodePage = (int)val },
            // In ANSI format, PidTagHtml is called PidTagBodyHtml (though the tag code is the same), because it is a string rather than a binary value
            // Here, we test the type to determine where to put the value 
            {EpropertyTag.PidTagHtml, (m, val) => { if (val is string)  m.BodyHtml = val; else m.Html = val; } },
            {EpropertyTag.PidTagRtfCompressed, (m, val) => m.RtfCompressed = val },
        };

        // The properties we read when getting a list of attachments
        private static readonly PropertyGetters<Attachment> pgAttachmentList = new PropertyGetters<Attachment>
        {
            {EpropertyTag.PidTagAttachFilenameW, (a, val) => a.FileNameW = val },
            {EpropertyTag.PidTagAttachLongFilename, (a, val) => a.LongFileName = val },
            {EpropertyTag.PidTagAttachmentSize, (a, val) => a.Size = val },
        };

        // The properties we read when accessing the name of an attachment
        private static readonly PropertyGetters<Attachment> pgAttachmentName = new PropertyGetters<Attachment>
        {
            {EpropertyTag.PidTagAttachFilenameW, (a, val) => a.FileNameW = val },
            {EpropertyTag.PidTagAttachLongFilename, (a, val) => a.LongFileName = val },
        };

        // The properties we read when accessing the contents of an attachment
        private static readonly PropertyGetters<Attachment> pgAttachmentContent = new PropertyGetters<Attachment>
        {
            {EpropertyTag.PidTagAttachMethod, (a, val) => a.AttachMethod = (AttachMethods)val },
            {EpropertyTag.PidTagAttachDataBinary, (a, val) => a.Content = val },
        };

        #region Public methods

        public XstFile(View view, string fullName)
        {
            this.ndb = new NDB(fullName);
            this.ltp = new LTP(ndb);
            this.view = view;
        }

        public void ReadFolderTree()
        {
            ndb.Initialise();

            using (var fs = ndb.GetReadStream())
            {
                var root = ReadFolderStructure(fs, new NID(EnidSpecial.NID_ROOT_FOLDER));

                foreach (var f in root.Folders)
                {
                    // We may be called on a background thread, so we need to dispatch this to the UI thread
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        view.RootFolders.Add(f);
                    }));
                }
            }
        }

        public void ReadMessages(Folder f)
        {
            if (f.ContentCount > 0)
            {
                using (var fs = ndb.GetReadStream())
                {
                    // Get the Contents table for the folder
                    var ms = ltp.ReadTable<Message>(fs, NID.TypedNID(EnidType.CONTENTS_TABLE, f.Nid), pgMessageList, (m, id) => m.Nid = new NID(id));
                    f.Messages.Clear();
                    foreach (var m in ms)
                        f.Messages.Add(m);
                }
            }
        }

        public void ReadMessageContent(Message m)
        {
            using (var fs = ndb.GetReadStream())
            {
                var subNodeTree = ltp.ReadProperties<Message>(fs, m.Nid, pgMessageContent, m);

                if (m.HasAttachment)
                {
                    // Read the attachment table, which is held in the subnode of the message
                    var atts = ltp.ReadTable<Attachment>(fs, subNodeTree, new NID(EnidSpecial.NID_ATTACHMENT_TABLE), pgAttachmentList, (a, id) => a.Nid = new NID(id));
                    m.Attachments.Clear();
                    foreach (var a in atts)
                    {
                        a.Parent = m;

                        // If the long name wasn't in the attachment table, go look for it in the attachment properties
                        if (a.LongFileName == null)
                        {
                            BTree<Node> subNodeTreeMessage;
                            ndb.LookupNodeAndReadItsSubNodeBtree(fs, a.Parent.Nid, out subNodeTreeMessage);
                            ltp.ReadProperties<Attachment>(fs, subNodeTreeMessage, a.Nid, pgAttachmentName, a);
                        }
                        m.Attachments.Add(a);
                    }
                }
            }
        }

        public void SaveAttachment(string path, Attachment a)
        {
            using (FileStream fs = ndb.GetReadStream(),
                             afs = new FileStream(Path.Combine(path, a.FileName), FileMode.Create, FileAccess.Write))
            {
                BTree<Node> subNodeTreeMessage;
                ndb.LookupNodeAndReadItsSubNodeBtree(fs, a.Parent.Nid, out subNodeTreeMessage);

                var subNodeTreeAttachment = ltp.ReadProperties<Attachment>(fs, subNodeTreeMessage, a.Nid, pgAttachmentContent, a);
                if (a.AttachMethod == AttachMethods.afByValue)
                {
                    // If the value is inline, we just write it out
                    if (a.Content.GetType() == typeof(byte[]))
                    {
                        afs.Write(a.Content, 0, a.Content.Length);
                    }
                    // Otherwise we need to dereference the node pointing to the data,
                    // using the subnode tree belonging to the attachment
                    else if (a.Content.GetType() == typeof(NID))
                    {
                        var nb = NDB.LookupSubNode(subNodeTreeAttachment, (NID)a.Content);

                        // Copy the data to the output file stream without getting it all into memory at once,
                        // as there can be a lot of data
                        ndb.CopyDataBlocks(fs, afs, nb.DataBid);
                    }
                }
            }
        }
        #endregion

        #region Private methods

        // Recurse down the folder tree, building a structure of Folder classes
        private Folder ReadFolderStructure(FileStream fs, NID nid)
        {
            Folder f = new Folder { Nid = nid };

            ltp.ReadProperties<Folder>(fs, nid, pgFolder, f);

            if (f.HasSubFolders)
            {
                foreach (var id in ltp.ReadTableRowIds(fs, NID.TypedNID(EnidType.HIERARCHY_TABLE, nid)))
                {
                    if (id.nidType == EnidType.NORMAL_FOLDER)
                        f.Folders.Add(ReadFolderStructure(fs, id));
                }
            }

            return f;
        }

        #endregion
    }
}
