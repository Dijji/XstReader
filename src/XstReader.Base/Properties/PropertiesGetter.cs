using System;
using System.Collections.Generic;
using System.Text;

namespace XstReader.Properties
{
    internal static class PropertiesGetter
    {
        // We use sets of PropertyGetters to define the equivalent of queries when reading property sets and tables

        // The folder properties we read when exploring folder structure
        public static readonly PropertyGetters<XstFolder> pgFolder = new PropertyGetters<XstFolder>
        {
            {EpropertyTag.PidTagDisplayName, (f, val) => f.Name = val },
            {EpropertyTag.PidTagContentCount, (f, val) => f.ContentCount = val },
            // Don't bother reading HasSubFolders, because it is not always set
            // {EpropertyTag.PidTagSubfolders, (f, val) => f.HasSubFolders = val },
        };

        // When reading folder contents, the message properties we ask for
        // In Unicode4K, PidTagSentRepresentingNameW doesn't yield a useful value
        public static readonly PropertyGetters<XstMessage> pgMessageList4K = new PropertyGetters<XstMessage>
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
        public static readonly PropertyGetters<XstMessage> pgMessageList = new PropertyGetters<XstMessage>
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

        public static readonly PropertyGetters<XstMessage> pgMessageDetail4K = new PropertyGetters<XstMessage>
        {
            {EpropertyTag.PidTagSentRepresentingNameW, (m, val) => m.From = val },
            {EpropertyTag.PidTagSentRepresentingEmailAddress, (m, val) => { if(m.From == null) m.From = val; } },
            {EpropertyTag.PidTagSenderName, (m, val) => { if(m.From == null) m.From = val; } },
        };

        // The properties we read when accessing the contents of a message
        public static readonly PropertyGetters<XstMessage> pgMessageContent = new PropertyGetters<XstMessage>
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
        public static readonly PropertyGetters<XstRecipient> pgMessageRecipient = new PropertyGetters<XstRecipient>
        {
            {EpropertyTag.PidTagRecipientType, (r, val) => r.RecipientType = (RecipientTypes)val },
            {EpropertyTag.PidTagDisplayName, (r, val) => r.DisplayName = val },
            {EpropertyTag.PidTagEmailAddress, (r, val) => r.EmailAddress = val },
        };

        //The properties we read when accessing a message attached to a message
        public static readonly PropertyGetters<XstMessage> pgMessageAttachment = new PropertyGetters<XstMessage>
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


        // The properties we read when getting a list of attachments
        public static readonly PropertyGetters<XstAttachment> pgAttachmentList = new PropertyGetters<XstAttachment>
        {
            {EpropertyTag.PidTagDisplayName, (a, val) => a.DisplayName = val },
            {EpropertyTag.PidTagAttachFilenameW, (a, val) => a.FileNameW = val },
            {EpropertyTag.PidTagAttachLongFilename, (a, val) => a.LongFileName = val },
            {EpropertyTag.PidTagAttachmentSize, (a, val) => a.Size = val },
            {EpropertyTag.PidTagAttachMethod, (a, val) => a.AttachMethod = (AttachMethod)val },
            //{EpropertyTag.PidTagAttachMimeTag, (a, val) => a.MimeTag = val },
            {EpropertyTag.PidTagAttachPayloadClass, (a, val) => a.FileNameW = val },
        };

        // The properties we read To enable handling of HTML images delivered as attachments
        public static readonly PropertyGetters<XstAttachment> pgAttachedHtmlImages = new PropertyGetters<XstAttachment>
        {
            {EpropertyTag.PidTagAttachFlags, (a, val) => a.Flags = (AttachFlags)val },
            {EpropertyTag.PidTagAttachMimeTag, (a, val) => a.MimeTag = val },
            {EpropertyTag.PidTagAttachContentId, (a, val) => a.ContentId = val },
            {EpropertyTag.PidTagAttachmentHidden, (a, val) => a.IsHidden = val },
        };

        // The properties we read when accessing the name of an attachment
        public static readonly PropertyGetters<XstAttachment> pgAttachmentName = new PropertyGetters<XstAttachment>
        {
            {EpropertyTag.PidTagAttachFilenameW, (a, val) => a.FileNameW = val },
            {EpropertyTag.PidTagAttachLongFilename, (a, val) => a.LongFileName = val },
        };

        // The properties we read when accessing the contents of an attachment
        public static readonly PropertyGetters<XstAttachment> pgAttachmentContent = new PropertyGetters<XstAttachment>
        {
            {EpropertyTag.PidTagAttachDataBinary, (a, val) => a.Content = val },
        };
    }
}
