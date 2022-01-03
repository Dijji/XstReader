using System.Collections.Generic;
using System.Linq;
using XstReader.ItemProperties;

namespace XstReader
{
    public class XstAttachmentSet
    {
        public XstMessage Message { get; private set; }
        private XstFile XstFile => Message.ParentFolder.XstFile;
        private LTP Ltp => XstFile.Ltp;
        private NDB Ndb => XstFile.Ndb;

        private IEnumerable<XstAttachment> _All = null;
        public IEnumerable<XstAttachment> All => GetAttachments();
        public IEnumerable<XstAttachment> Files => All.Where(a => a.IsFile);
        public IEnumerable<XstAttachment> VisibleFiles => Files.Where(a => !a.Hide);
        public IEnumerable<XstAttachment> MaybeInline => All.Where(a => a.HasContentId);
        public bool Any => (Message.Flags & MessageFlags.mfHasAttach) == MessageFlags.mfHasAttach;
        public bool MayHaveAnyInline => Any && MaybeInline.Any();
        public bool HasFiles => Any && Files.Any();
        public bool HasVisibleFiles => Any && VisibleFiles.Any();

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        internal XstAttachmentSet(XstMessage message)
        {
            Message = message;
        }

        public IEnumerable<XstAttachment> GetAttachments()
            => _All ?? (_All = GetAttachmentsInternal());

        private IEnumerable<XstAttachment> GetAttachmentsInternal()
        {
            if (Any)
            {
                // Read any attachments
                var attachmentsNid = new NID(EnidSpecial.NID_ATTACHMENT_TABLE);

                if (!Ltp.IsTablePresent(Message.SubNodeTreeProperties, attachmentsNid))
                    throw new XstException("Could not find expected Attachment table");

                // Read the attachment table, which is held in the subnode of the message
                var atts = Ltp.ReadTable<XstAttachment>(Message.SubNodeTreeProperties, attachmentsNid, PropertyGetters.AttachmentListProperties, (a, id) => a.Nid = new NID(id)).ToList();
                foreach (var a in atts)
                {
                    a.Message = Message; // For lazy reading of the complete properties: a.Message.Folder.XstFile

                    // If the long name wasn't in the attachment table, go look for it in the attachment properties
                    if (a.LongFileName == null)
                        Ltp.ReadProperties<XstAttachment>(Message.SubNodeTreeProperties, a.Nid, PropertyGetters.AttachmentNameProperties, a);

                    // Read properties relating to HTML images presented as attachments
                    Ltp.ReadProperties<XstAttachment>(Message.SubNodeTreeProperties, a.Nid, PropertyGetters.AttachedHtmlImagesProperties, a);

                    // If this is an embedded email, tell the attachment where to look for its properties
                    // This is needed because the email node is not in the main node tree
                    if (Message.IsAttached)
                        a.SubNodeTreeProperties = Message.SubNodeTreeProperties;

                    yield return a;
                }
            }
            yield break;
        }

        public void ClearContents()
        {
            _All = null;
        }

        internal void RemoveFirst()
            => _All = All.Skip(1);

        internal void AddAttachments(params XstAttachment[] attachments)
            => _All = All.Union(attachments);
    }
}
