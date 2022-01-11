// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;

namespace XstReader.ElementProperties
{
    // Property getters are used to specify which properties should be retrieved from a property context
    // or table context, and where they should be stored.
    // T is the target object, Action arguments are target object, column value 
    internal class PropertyGetters<T> : Dictionary<PropertyCanonicalName, Action<T, dynamic>>
    {
    }

    internal static class PropertyGetters
    {
        // We use sets of PropertyGetters to define the equivalent of queries when reading property sets and tables

        //// The folder properties we read when exploring folder structure
        //public static readonly PropertyGetters<XstFolder> FolderProperties = new PropertyGetters<XstFolder>
        //{
        //    {PropertyCanonicalName.PidTagDisplayName, (f, val) => f.Name = val },
        //    {PropertyCanonicalName.PidTagContentCount, (f, val) => f.ContentCount = val },
        //    // Don't bother reading HasSubFolders, because it is not always set
        //    // {PropertyCanonicalName.PidTagSubfolders, (f, val) => f.HasSubFolders = val },
        //};

        //// When reading folder contents, the message properties we ask for
        //// In Unicode4K, PidTagSentRepresentingNameW doesn't yield a useful value
        //public static readonly PropertyGetters<XstMessage> MessageList4KProperties = new PropertyGetters<XstMessage>
        //{
        //    {PropertyCanonicalName.PidTagSubjectW, (m, val) => m.Subject = val },
        //    {PropertyCanonicalName.PidTagDisplayCcW, (m, val) => m.Cc = val },
        //    {PropertyCanonicalName.PidTagDisplayToW, (m, val) => m.To = val },
        //    {PropertyCanonicalName.PidTagMessageFlags, (m, val) => m.Flags = (MessageFlags)val },
        //    {PropertyCanonicalName.PidTagClientSubmitTime, (m, val) => m.Submitted = val },
        //    {PropertyCanonicalName.PidTagMessageDeliveryTime, (m, val) => m.Received = val },
        //    {PropertyCanonicalName.PidTagLastModificationTime, (m, val) => m.Modified = val },
        //};

        //// When reading folder contents, the message properties we ask for
        //public static readonly PropertyGetters<XstMessage> MessageListProperties = new PropertyGetters<XstMessage>
        //{
        //    {PropertyCanonicalName.PidTagSubjectW, (m, val) => m.Subject = val },
        //    {PropertyCanonicalName.PidTagDisplayCcW, (m, val) => m.Cc = val },
        //    {PropertyCanonicalName.PidTagDisplayToW, (m, val) => m.To = val },
        //    {PropertyCanonicalName.PidTagMessageFlags, (m, val) => m.Flags = (MessageFlags)val },
        //    {PropertyCanonicalName.PidTagSentRepresentingNameW, (m, val) => m.From = val },
        //    {PropertyCanonicalName.PidTagClientSubmitTime, (m, val) => m.Submitted = val },
        //    {PropertyCanonicalName.PidTagMessageDeliveryTime, (m, val) => m.Received = val },
        //    {PropertyCanonicalName.PidTagLastModificationTime, (m, val) => m.Modified = val },
        //};

        //public static readonly PropertyGetters<XstMessage> MessageDetail4KProperties = new PropertyGetters<XstMessage>
        //{
        //    {PropertyCanonicalName.PidTagSentRepresentingNameW, (m, val) => m.From = val },
        //    {PropertyCanonicalName.PidTagSentRepresentingEmailAddress, (m, val) => { if(m.From == null) m.From = val; } },
        //    {PropertyCanonicalName.PidTagSenderName, (m, val) => { if(m.From == null) m.From = val; } },
        //};

        //// The properties we read when accessing the contents of a message
        //public static readonly PropertyGetters<XstMessage> MessageContentProperties = new PropertyGetters<XstMessage>
        //{
        //    {PropertyCanonicalName.PidTagNativeBody, (m, val) => m.NativeBody = (BodyType)val },
        //    {PropertyCanonicalName.PidTagBody, (m, val) => m.BodyPlainText = val },
        //    //{PropertyCanonicalName.PidTagInternetCodepage, (m, val) => m.InternetCodePage = (int)val },
        //    // In ANSI format, PidTagHtml is called PidTagBodyHtml (though the tag code is the same), because it is a string rather than a binary value
        //    // Here, we test the type to determine where to put the value 
        //    {PropertyCanonicalName.PidTagHtml, (m, val) => { if (val is string)  m.BodyHtml = val; else m.Html = val; } },
        //    {PropertyCanonicalName.PidTagRtfCompressed, (m, val) => m.BodyRtfCompressed = val },
        //};

        // The properties we read when accessing the recipient table of a message
        public static readonly PropertyGetters<XstRecipient> MessageRecipientProperties = new PropertyGetters<XstRecipient>
        {
            {PropertyCanonicalName.PidTagRecipientType, (r, val) => r.RecipientType = (RecipientTypes)val },
            {PropertyCanonicalName.PidTagDisplayName, (r, val) => r.DisplayName = val },
            {PropertyCanonicalName.PidTagSmtpAddress, (r, val) => r.EmailAddress = val },
            {PropertyCanonicalName.PidTagEmailAddress, (r, val) => r.EmailAddress = r.EmailAddress ?? val},
        };

        ////The properties we read when accessing a message attached to a message
        //public static readonly PropertyGetters<XstMessage> MessageAttachmentProperties = new PropertyGetters<XstMessage>
        //{
        //    {PropertyCanonicalName.PidTagSubjectW, (m, val) => m.Subject = val },
        //    {PropertyCanonicalName.PidTagDisplayCcW, (m, val) => m.Cc = val },
        //    {PropertyCanonicalName.PidTagDisplayToW, (m, val) => m.To = val },
        //    {PropertyCanonicalName.PidTagMessageFlags, (m, val) => m.Flags = (MessageFlags)val },
        //    {PropertyCanonicalName.PidTagSentRepresentingNameW, (m, val) => m.From = val },
        //    {PropertyCanonicalName.PidTagClientSubmitTime, (m, val) => m.Submitted = val },
        //    {PropertyCanonicalName.PidTagMessageDeliveryTime, (m, val) => m.Received = val },
        //    {PropertyCanonicalName.PidTagLastModificationTime, (m, val) => m.Modified = val },
        //    {PropertyCanonicalName.PidTagNativeBody, (m, val) => m.NativeBody = (BodyType)val },
        //    {PropertyCanonicalName.PidTagBody, (m, val) => m.BodyPlainText = val },
        //    {PropertyCanonicalName.PidTagHtml, (m, val) => { if (val is string)  m.BodyHtml = val; else m.Html = val; } },
        //    {PropertyCanonicalName.PidTagRtfCompressed, (m, val) => m.BodyRtfCompressed = val },
        //};


        // The properties we read when getting a list of attachments
        public static readonly PropertyGetters<XstAttachment> AttachmentListProperties = new PropertyGetters<XstAttachment>
        {
            {PropertyCanonicalName.PidTagDisplayName, (a, val) => a.DisplayName = val },
            {PropertyCanonicalName.PidTagAttachFilename, (a, val) => a.FileNameW = val },
            {PropertyCanonicalName.PidTagAttachLongFilename, (a, val) => a.LongFileName = val },
            {PropertyCanonicalName.PidTagAttachSize, (a, val) => a.Size = val },
            {PropertyCanonicalName.PidTagAttachMethod, (a, val) => a.AttachMethod = (AttachMethod)val },
            //{PropertyCanonicalName.PidTagAttachMimeTag, (a, val) => a.MimeTag = val },
            {PropertyCanonicalName.PidTagAttachPayloadClass, (a, val) => a.FileNameW = val },
        };

        // The properties we read To enable handling of HTML images delivered as attachments
        public static readonly PropertyGetters<XstAttachment> AttachedHtmlImagesProperties = new PropertyGetters<XstAttachment>
        {
            {PropertyCanonicalName.PidTagAttachFlags, (a, val) => a.Flags = (AttachFlags)val },
            {PropertyCanonicalName.PidTagAttachMimeTag, (a, val) => a.MimeTag = val },
            {PropertyCanonicalName.PidTagAttachContentId, (a, val) => a.ContentId = val },
            {PropertyCanonicalName.PidTagAttachmentHidden, (a, val) => a.IsHidden = val },
        };

        // The properties we read when accessing the name of an attachment
        public static readonly PropertyGetters<XstAttachment> AttachmentNameProperties = new PropertyGetters<XstAttachment>
        {
            {PropertyCanonicalName.PidTagAttachFilename, (a, val) => a.FileNameW = val },
            {PropertyCanonicalName.PidTagAttachLongFilename, (a, val) => a.LongFileName = val },
        };

        // The properties we read when accessing the contents of an attachment
        public static readonly PropertyGetters<XstAttachment> AttachmentContentProperties = new PropertyGetters<XstAttachment>
        {
            {PropertyCanonicalName.PidTagAttachDataBinary, (a, val) => a.Content = val },
        };
    }
}
