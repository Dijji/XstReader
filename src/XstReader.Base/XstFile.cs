// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;



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

    public class XstFile: IDisposable
    {
        private NDB _Ndb;
        internal NDB Ndb => _Ndb ?? (_Ndb = new NDB(this));

        private LTP _Ltp;
        internal LTP Ltp => _Ltp ?? (_Ltp = new LTP(Ndb));

        private string _FileName = null;
        public string FileName { get => _FileName; set => SetFileName(value); }
        private void SetFileName(string fileName)
        {
            _FileName = fileName;
            ResetXstFile();
        }
        
        private FileStream _ReadStream = null;
        internal FileStream ReadStream
        {
            get => _ReadStream ?? (_ReadStream = new FileStream(FileName, FileMode.Open, FileAccess.Read));
        }
        private void ResetXstFile()
        {
            if (_ReadStream != null)
            {
                _ReadStream.Close();
                _ReadStream.Dispose();
                _ReadStream = null;
            }
            _Ndb = null;
            _Ltp = null;
        }

        #region PropertyGetters
        // We use sets of PropertyGetters to define the equivalent of queries when reading property sets and tables

        // The folder properties we read when exploring folder structure
        private static readonly PropertyGetters<Folder> pgFolder = new PropertyGetters<Folder>
        {
            {EpropertyTag.PidTagDisplayName, (f, val) => f.Name = val },
            {EpropertyTag.PidTagContentCount, (f, val) => f.ContentCount = val },
            // Don't bother reading HasSubFolders, because it is not always set
            // {EpropertyTag.PidTagSubfolders, (f, val) => f.HasSubFolders = val },
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

        // The properties we read when accessing the recipient table of a message
        private static readonly PropertyGetters<Recipient> pgMessageRecipient = new PropertyGetters<Recipient>
        {
            {EpropertyTag.PidTagRecipientType, (r, val) => r.RecipientType = (RecipientType)val },
            {EpropertyTag.PidTagDisplayName, (r, val) => r.DisplayName = val },
            {EpropertyTag.PidTagEmailAddress, (r, val) => r.EmailAddress = val },
        };

        //The properties we read when accessing a message attached to a message
        private static readonly PropertyGetters<Message> pgMessageAttachment = new PropertyGetters<Message>
        {
            {EpropertyTag.PidTagSubjectW, (m, val) => m.Subject = val },
            {EpropertyTag.PidTagDisplayCcW, (m, val) => m.Cc = val },
            {EpropertyTag.PidTagDisplayToW, (m, val) => m.To = val },
            {EpropertyTag.PidTagMessageFlags, (m, val) => m.Flags = (MessageFlags)val },
            {EpropertyTag.PidTagSentRepresentingNameW, (m, val) => m.From = val },
            {EpropertyTag.PidTagClientSubmitTime, (m, val) => m.Submitted = val },
            {EpropertyTag.PidTagMessageDeliveryTime, (m, val) => m.Received = val },
            {EpropertyTag.PidTagLastModificationTime, (m, val) => m.Modified = val },
            {EpropertyTag.PidTagNativeBody, (m, val) => m.NativeBody = (BodyType)val },
            {EpropertyTag.PidTagBody, (m, val) => m.Body = val },
            {EpropertyTag.PidTagHtml, (m, val) => { if (val is string)  m.BodyHtml = val; else m.Html = val; } },
            {EpropertyTag.PidTagRtfCompressed, (m, val) => m.RtfCompressed = val },
        };

        private static readonly HashSet<EpropertyTag> contentExclusions = new HashSet<EpropertyTag>
        {
            EpropertyTag.PidTagNativeBody,
            EpropertyTag.PidTagBody,
            EpropertyTag.PidTagHtml,
            EpropertyTag.PidTagRtfCompressed,
        };

        // The properties we read when getting a list of attachments
        private static readonly PropertyGetters<Attachment> pgAttachmentList = new PropertyGetters<Attachment>
        {
            {EpropertyTag.PidTagDisplayName, (a, val) => a.DisplayName = val },
            {EpropertyTag.PidTagAttachFilenameW, (a, val) => a.FileNameW = val },
            {EpropertyTag.PidTagAttachLongFilename, (a, val) => a.LongFileName = val },
            {EpropertyTag.PidTagAttachmentSize, (a, val) => a.Size = val },
            {EpropertyTag.PidTagAttachMethod, (a, val) => a.AttachMethod = (AttachMethods)val },
            //{EpropertyTag.PidTagAttachMimeTag, (a, val) => a.MimeTag = val },
            {EpropertyTag.PidTagAttachPayloadClass, (a, val) => a.FileNameW = val },
        };

        // The properties we read To enable handling of HTML images delivered as attachments
        private static readonly PropertyGetters<Attachment> pgAttachedHtmlImages = new PropertyGetters<Attachment>
        {
            {EpropertyTag.PidTagAttachFlags, (a, val) => a.Flags = (AttachFlags)val },
            {EpropertyTag.PidTagAttachMimeTag, (a, val) => a.MimeTag = val },
            {EpropertyTag.PidTagAttachContentId, (a, val) => a.ContentId = val },
            {EpropertyTag.PidTagAttachmentHidden, (a, val) => a.Hidden = val },
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
            {EpropertyTag.PidTagAttachDataBinary, (a, val) => a.Content = val },
        };

        private static readonly HashSet<EpropertyTag> attachmentContentExclusions = new HashSet<EpropertyTag>
        {
            EpropertyTag.PidTagAttachDataBinary,
        };
        #endregion PropertyGetters

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fileName"></param>
        public XstFile(string fileName)
        {
            FileName = fileName;
        }
        #endregion Ctor


        private Folder _RootFolder = null;
        public Folder RootFolder => _RootFolder ?? (_RootFolder = new Folder(this, new NID(EnidSpecial.NID_ROOT_FOLDER)));

        #region Public methods

        public void ReadMessageDetails(Message m)
        {
            // Read the contents properties
            var subNodeTree = Ltp.ReadProperties<Message>(m.Nid, pgMessageContent, m);

            // Read all other properties
            m.Properties.Clear();
            m.Properties.AddRange(Ltp.ReadAllProperties(m.Nid, contentExclusions).ToList());

            ReadMessageTables(subNodeTree, m);
        }

        public List<Property> ReadAttachmentProperties(Attachment a)
        {
            BTree<Node> subNodeTreeMessage = a.subNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                Ndb.LookupNodeAndReadItsSubNodeBtree(a.Message.Nid, out subNodeTreeMessage);

            // Read all non-content properties
            // Convert to list so that we can dispose the file access
            return new List<Property>(Ltp.ReadAllProperties(subNodeTreeMessage, a.Nid, attachmentContentExclusions, true));
        }

        public void SaveVisibleAttachmentsToAssociatedFolder(string fullFileName, Message m)
        {
            if (m.HasVisibleFileAttachment)
            {
                var targetFolder = Path.Combine(Path.GetDirectoryName(fullFileName),
                    Path.GetFileNameWithoutExtension(fullFileName) + " Attachments");
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                    if (m.Date != null)
                        Directory.SetCreationTime(targetFolder, (DateTime)m.Date);
                }
                SaveAttachmentsToFolder(targetFolder, m.Date, m.Attachments.Where(a => a.IsFile && !a.Hide));
            }
        }

        public void SaveAttachmentsToFolder(string fullFolderName, DateTime? creationTime, IEnumerable<Attachment> attachments)
        {
            foreach (var a in attachments)
            {
                SaveAttachmentToFolder(fullFolderName, creationTime, a);
            }
        }

        const int MaxPath = 260;
        public void SaveAttachmentToFolder(string folderpath, DateTime? creationTime, Attachment a)
        {
            var fullFileName = Path.Combine(folderpath, a.FileName);

            // If the result is too long, truncate the attachment name as required
            if (fullFileName.Length >= MaxPath)
            {
                var ext = Path.GetExtension(a.FileName);
                var att = Path.GetFileNameWithoutExtension(a.FileName)
                    .Truncate(MaxPath - folderpath.Length - ext.Length - 5) + ext;
                fullFileName = Path.Combine(folderpath, att);
            }
            SaveAttachment(fullFileName, creationTime, a);
        }

        public void SaveAttachment(string fullFileName, DateTime? creationTime, Attachment a)
        {
            using (var afs = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
            {
                SaveAttachment(afs, a);
            }
            if (creationTime != null)
                File.SetCreationTime(fullFileName, (DateTime)creationTime);
        }

        public void SaveAttachment(Stream s, Attachment a)
        {
            if (a.WasLoadedFromMime)
            {
                s.Write(a.Content, 0, a.Content.Length);
            }
            else
            {
                BTree<Node> subNodeTreeMessage = a.subNodeTreeProperties;

                if (subNodeTreeMessage == null)
                    // No subNodeTree given: assume we can look it up in the main tree
                    Ndb.LookupNodeAndReadItsSubNodeBtree(a.Message.Nid, out subNodeTreeMessage);

                var subNodeTreeAttachment = Ltp.ReadProperties<Attachment>(subNodeTreeMessage, a.Nid, pgAttachmentContent, a);

                if ((object)a.Content != null)
                {
                    // If the value is inline, we just write it out
                    if (a.Content.GetType() == typeof(byte[]))
                    {
                        s.Write(a.Content, 0, a.Content.Length);
                    }
                    // Otherwise we need to dereference the node pointing to the data,
                    // using the subnode tree belonging to the attachment
                    else if (a.Content.GetType() == typeof(NID))
                    {
                        var nb = NDB.LookupSubNode(subNodeTreeAttachment, (NID)a.Content);

                        // Copy the data to the output file stream without getting it all into memory at once,
                        // as there can be a lot of data
                        Ndb.CopyDataBlocks(s, nb.DataBid);
                    }
                }
            }
        }

        public Message OpenAttachedMessage(Attachment a)
        {
            BTree<Node> subNodeTreeMessage = a.subNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                Ndb.LookupNodeAndReadItsSubNodeBtree(a.Message.Nid, out subNodeTreeMessage);

            var subNodeTreeAttachment = Ltp.ReadProperties<Attachment>(subNodeTreeMessage, a.Nid, pgAttachmentContent, a);
            if (a.Content.GetType() == typeof(PtypObjectValue))
            {
                Message m = new Message { Nid = new NID(((PtypObjectValue)a.Content).Nid) };

                // Read the basic and contents properties
                var childSubNodeTree = Ltp.ReadProperties<Message>(subNodeTreeAttachment, m.Nid, pgMessageAttachment, m, true);

                // Read all other properties
                m.Properties.AddRange(Ltp.ReadAllProperties(subNodeTreeAttachment, m.Nid, contentExclusions, true).ToList());

                ReadMessageTables(childSubNodeTree, m, true);

                return m;
            }
            else
                throw new XstException("Unexpected data type for attached message");

        }


        private struct LineProp
        {
            public int line;
            public Property p;
        }

        public void ExportMessageProperties(IEnumerable<Message> messages, string fileName)
        {
            // We build a dictionary of queues of line,Property pairs where each queue represents
            // a column in the CSV file, and the line is the line number in the file.
            // The key to the dictionary is the property ID.

            var dict = new Dictionary<string, Queue<LineProp>>();
            int lines = 1;

            foreach (var m in messages)
            {
                //// Do not reread properties for current message as it will fail updating the display
                //if (m != view.CurrentMessage)
                {
                    try
                    {
                        ReadMessageDetails(m);
                    }
                    catch (XstException)
                    {
                        // Ignore file exceptions to get as much as we can
                    }
                }


                foreach (var p in m.Properties)
                {
                    Queue<LineProp> queue;
                    if (!dict.TryGetValue(p.CsvId, out queue))
                    {
                        queue = new Queue<LineProp>();
                        dict[p.CsvId] = queue;
                    }
                    queue.Enqueue(new LineProp { line = lines, p = p });
                }
                lines++;
            }

            // Now we sort the columns by ID
            var columns = dict.Keys.OrderBy(x => x).ToArray();

            // And finally output the CSV file line by line
            using (var sw = new System.IO.StreamWriter(fileName, false, Encoding.UTF8))

            {
                StringBuilder sb = new StringBuilder();
                bool hasValue = false;

                for (int line = 0; line < lines; line++)
                {
                    foreach (var col in columns)
                    {
                        var q = dict[col];

                        // First line is always the column headers
                        if (line == 0)
                            AddCsvValue(sb, q.Peek().p.CsvDescription, ref hasValue);

                        // After that, output the column value if it has one
                        else if (q.Count > 0 && q.Peek().line == line)
                            AddCsvValue(sb, q.Dequeue().p.DisplayValue, ref hasValue);

                        // Or leave it blank
                        else
                            AddCsvValue(sb, "", ref hasValue);
                    }

                    // Write the completed line out
                    sw.WriteLine(sb.ToString());
                    sb.Clear();
                    hasValue = false;
                }
            }
        }
        #endregion

        #region Private methods




        private void ReadMessageTables(BTree<Node> subNodeTree, Message m, bool isAttached = false)
        {
            // Read the recipient table for the message
            var recipientsNid = new NID(EnidSpecial.NID_RECIPIENT_TABLE);
            if (Ltp.IsTablePresent(subNodeTree, recipientsNid))
            {
                var rs = Ltp.ReadTable<Recipient>(subNodeTree, recipientsNid, pgMessageRecipient, null, (r, p) => r.Properties.Add(p));
                m.Recipients.Clear();
                foreach (var r in rs)
                {
                    // Sort the properties
                    List<Property> lp = new List<Property>(r.Properties);
                    lp.Sort((a, b) => a.Tag.CompareTo(b.Tag));
                    r.Properties.Clear();
                    foreach (var p in lp)
                        r.Properties.Add(p);

                    m.Recipients.Add(r);
                }
            }

            // Read any attachments
            var attachmentsNid = new NID(EnidSpecial.NID_ATTACHMENT_TABLE);
            if (m.HasAttachment)
            {
                if (!Ltp.IsTablePresent(subNodeTree, attachmentsNid))
                    throw new XstException("Could not find expected Attachment table");

                // Read the attachment table, which is held in the subnode of the message
                var atts = Ltp.ReadTable<Attachment>(subNodeTree, attachmentsNid, pgAttachmentList, (a, id) => a.Nid = new NID(id)).ToList();
                foreach (var a in atts)
                {
                    a.Message = m; // For lazy reading of the complete properties: a.Message.Folder.XstFile

                    // If the long name wasn't in the attachment table, go look for it in the attachment properties
                    if (a.LongFileName == null)
                        Ltp.ReadProperties<Attachment>(subNodeTree, a.Nid, pgAttachmentName, a);

                    // Read properties relating to HTML images presented as attachments
                    Ltp.ReadProperties<Attachment>(subNodeTree, a.Nid, pgAttachedHtmlImages, a);

                    // If this is an embedded email, tell the attachment where to look for its properties
                    // This is needed because the email node is not in the main node tree
                    if (isAttached)
                        a.subNodeTreeProperties = subNodeTree;
                }

                m.SaveAttachments(atts);
            }
        }

        private void AddCsvValue(StringBuilder sb, string value, ref bool hasValue)
        {
            if (hasValue)
                sb.Append(CultureInfo.CurrentCulture.TextInfo.ListSeparator); // aka comma

            if (value != null)
            {
                // Multilingual characters should be quoted, so we will just quote all values,
                // which means we need to double quotes in the value
                // Excel cannot cope with Unicode files with values containing
                // new line characters, so remove those as well
                var val = value.Replace("\"", "\"\"").Replace("\r\n", "; ").Replace("\r", " ").Replace("\n", " ");
                sb.Append("\"");
                sb.Append(EnforceCsvValueLengthLimit(val));
                sb.Append("\"");
            }

            hasValue = true;
        }

        private static int valueLengthLimit = (int)Math.Pow(2, 15) - 12;
        private string EnforceCsvValueLengthLimit(string value)
        {
            if (value.Length < valueLengthLimit)
                return value;
            else
                return value.Substring(0, valueLengthLimit) + "…";
        }

        public void Dispose()
        {
            ResetXstFile();
        }

        #endregion
    }
}
