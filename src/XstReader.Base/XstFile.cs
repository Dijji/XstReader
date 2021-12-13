// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using XstReader.Common.BTrees;
using XstReader.Properties;

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

    public class XstFile : IDisposable
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


        private XstFolder _RootFolder = null;
        public XstFolder RootFolder => _RootFolder ?? (_RootFolder = new XstFolder(this, new NID(EnidSpecial.NID_ROOT_FOLDER)));

        #region Public methods

        public List<XstProperty> ReadAttachmentProperties(XstAttachment a)
        {
            BTree<Node> subNodeTreeMessage = a.SubNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                Ndb.LookupNodeAndReadItsSubNodeBtree(a.Message.Nid, out subNodeTreeMessage);

            // Read all non-content properties
            // Convert to list so that we can dispose the file access
            return new List<XstProperty>(Ltp.ReadAllProperties(subNodeTreeMessage, a.Nid, XstAttachment.attachmentContentExclusions, true));
        }

        public void SaveVisibleAttachmentsToAssociatedFolder(string fullFileName, XstMessage m)
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

        public void SaveAttachmentsToFolder(string fullFolderName, DateTime? creationTime, IEnumerable<XstAttachment> attachments)
        {
            foreach (var a in attachments)
            {
                SaveAttachmentToFolder(fullFolderName, creationTime, a);
            }
        }

        const int MaxPath = 260;
        public void SaveAttachmentToFolder(string folderpath, DateTime? creationTime, XstAttachment a)
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

        public void SaveAttachment(string fullFileName, DateTime? creationTime, XstAttachment a)
        {
            using (var afs = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
            {
                SaveAttachment(afs, a);
            }
            if (creationTime != null)
                File.SetCreationTime(fullFileName, (DateTime)creationTime);
        }

        public void SaveAttachment(Stream s, XstAttachment a)
        {
            if (a.WasLoadedFromMime)
            {
                s.Write(a.Content, 0, a.Content.Length);
            }
            else
            {
                BTree<Node> subNodeTreeMessage = a.SubNodeTreeProperties;

                if (subNodeTreeMessage == null)
                    // No subNodeTree given: assume we can look it up in the main tree
                    Ndb.LookupNodeAndReadItsSubNodeBtree(a.Message.Nid, out subNodeTreeMessage);

                var subNodeTreeAttachment = Ltp.ReadProperties<XstAttachment>(subNodeTreeMessage, a.Nid, PropertiesGetter.pgAttachmentContent, a);

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


        private struct LineProp
        {
            public int line;
            public XstProperty p;
        }

        public void ExportMessageProperties(IEnumerable<XstMessage> messages, string fileName)
        {
            // We build a dictionary of queues of line,Property pairs where each queue represents
            // a column in the CSV file, and the line is the line number in the file.
            // The key to the dictionary is the property ID.

            var dict = new Dictionary<string, Queue<LineProp>>();
            int lines = 1;

            foreach (var m in messages)
            {
                ////// Do not reread properties for current message as it will fail updating the display
                ////if (m != view.CurrentMessage)
                //{
                //    try
                //    {
                //        m.ReadMessageDetails();
                //    }
                //    catch (XstException)
                //    {
                //        // Ignore file exceptions to get as much as we can
                //    }
                //}


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
