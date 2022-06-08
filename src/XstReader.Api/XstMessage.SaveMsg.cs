// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021,2022 iluvadev, and released under Ms-PL License.
// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 
// 
//
// Export to Msg files adapted from MsgKit (https://github.com/Sicos1977/MsgKit), under MIT license
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2015-2021 Magic-Sessions. (www.magic-sessions.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using OpenMcdf;
using System.Collections.Generic;
using System.Linq;
using XstReader.ElementProperties;
using XstReader.MsgKit;
using XstReader.MsgKit.Enums;
using XstReader.MsgKit.Helpers;
using XstReader.MsgKit.Streams;

namespace XstReader
{
    public partial class XstMessage
    {
        /// <summary>
        ///     The <see cref="CompoundFile" />
        /// </summary>
        internal CompoundFile CompoundFile { get; private set; }
        /// <summary>
        ///     The <see cref="TopLevelProperties"/>
        /// </summary>
        internal TopLevelProperties TopLevelProperties { get; private set; }
        /// <summary>
        ///     The <see cref="NamedProperties"/>
        /// </summary>
        internal NamedProperties NamedProperties { get; private set; }

        private long WriteRecipientToStorage(XstRecipient recipient, CFStorage storage, long index)
        {
            var propertiesStream = new RecipientProperties();

            propertiesStream.AddProperty(new XstProperty(PropertyCanonicalName.PidTagRowid, PropertyType.PT_LONG, index));
            propertiesStream.AddProperty(new XstProperty(PropertyCanonicalName.PidTagEntryId, PropertyType.PT_BINARY, Mapi.GenerateEntryId));
            propertiesStream.AddProperty(new XstProperty(PropertyCanonicalName.PidTagInstanceKey, PropertyType.PT_BINARY, Mapi.GenerateInstanceKey));

            foreach (var xstProperty in recipient.Properties.Items.Where(p => !_NonExportableTagsList.Contains(p.Tag) &&
                                                                               KnownCanonicalNames.KnonwnProperties.Any(k => (PropertyCanonicalName)k.Id == p.Tag)))
                propertiesStream.AddProperty(xstProperty);

            return propertiesStream.WriteProperties(storage);
        }

        private long WriteRecipientsToStorage(CFStorage rootStorage)
        {
            long size = 0;
            int index = 0;
            foreach (var recipient in Recipients.Items.Where(r => !r.IsGeneratedFromMessageProperties))
            {
                var storage = rootStorage.AddStorage(PropertyTags.RecipientStoragePrefix + index.ToString("X8").ToUpper());
                size += WriteRecipientToStorage(recipient, storage, index);
                index++;
            }
            return size;
        }
        private long WriteAttachmentToStorage(XstAttachment attachment, CFStorage storage, long index)
        {
            var propertiesStream = new AttachmentProperties();


            propertiesStream.AddProperty(new XstProperty(PropertyCanonicalName.PidTagAttachNumber, PropertyType.PT_LONG, index), PropertyFlags.PROPATTR_READABLE);
            propertiesStream.AddProperty(new XstProperty(PropertyCanonicalName.PidTagInstanceKey, PropertyType.PT_BINARY, Mapi.GenerateInstanceKey), PropertyFlags.PROPATTR_READABLE);
            propertiesStream.AddProperty(new XstProperty(PropertyCanonicalName.PidTagRecordKey, PropertyType.PT_BINARY, Mapi.GenerateRecordKey), PropertyFlags.PROPATTR_READABLE);

            foreach (var xstProperty in attachment.Properties.Items.Where(p => !_NonExportableTagsList.Contains(p.Tag) &&
                                                                               KnownCanonicalNames.KnonwnProperties.Any(k => (PropertyCanonicalName)k.Id == p.Tag)))
                propertiesStream.AddProperty(xstProperty);

            return propertiesStream.WriteProperties(storage);
        }

        private long WriteAttachmentsToStorage(CFStorage rootStorage)
        {
            long size = 0;
            int index = 0;
            foreach (var attachment in Attachments)
            {
                var storage = rootStorage.AddStorage(PropertyTags.AttachmentStoragePrefix + index.ToString("X8").ToUpper());
                size += WriteAttachmentToStorage(attachment, storage, index);
                index++;
            }

            return size;
        }

        private List<PropertyCanonicalName> _NonExportableTagsList = new List<PropertyCanonicalName>
        {
            PropertyCanonicalName.PidTagEntryId,
            PropertyCanonicalName.PidTagInstanceKey,
            PropertyCanonicalName.PidTagStoreSupportMask,
            (PropertyCanonicalName)0x340F,
            PropertyCanonicalName.PidTagRowid,
            PropertyCanonicalName.PidTagAttachNumber,
            PropertyCanonicalName.PidTagRecordKey,
            PropertyCanonicalName.PidTagMessageSize,
        };
        private long WriteToStorage()
        {
            var rootStorage = CompoundFile.RootStorage;
            long messageSize = 0;

            messageSize += WriteRecipientsToStorage(rootStorage);
            messageSize += WriteAttachmentsToStorage(rootStorage);

            var recipientCount = Recipients.Items.Count();
            var attachmentCount = Attachments.Count();

            TopLevelProperties.RecipientCount = recipientCount;
            TopLevelProperties.AttachmentCount = attachmentCount;
            TopLevelProperties.NextRecipientId = recipientCount;
            TopLevelProperties.NextAttachmentId = attachmentCount;


            TopLevelProperties.AddProperty(new XstProperty(PropertyCanonicalName.PidTagEntryId, PropertyType.PT_BINARY, Mapi.GenerateEntryId));
            TopLevelProperties.AddProperty(new XstProperty(PropertyCanonicalName.PidTagInstanceKey, PropertyType.PT_BINARY, Mapi.GenerateInstanceKey));
            TopLevelProperties.AddProperty(new XstProperty(PropertyCanonicalName.PidTagStoreSupportMask, PropertyType.PT_LONG, StoreSupportMaskConst.StoreSupportMask), PropertyFlags.PROPATTR_READABLE);
            TopLevelProperties.AddProperty(new XstProperty((PropertyCanonicalName)0x340F, PropertyType.PT_LONG, StoreSupportMaskConst.StoreSupportMask), PropertyFlags.PROPATTR_READABLE);


            //TopLevelProperties.AddProperty(new XstProperty(PropertyCanonicalName.PidTagAlternateRecipientAllowed, PropertyType.PT_BOOLEAN, true), PropertyFlags.PROPATTR_READABLE);
            //TopLevelProperties.AddProperty(new XstProperty(PropertyCanonicalName.PidTagHasAttachments, PropertyType.PT_BOOLEAN, HasAttachments));


            // http://www.meridiandiscovery.com/how-to/e-mail-conversation-index-metadata-computer-forensics/
            // http://stackoverflow.com/questions/11860540/does-outlook-embed-a-messageid-or-equivalent-in-its-email-elements
            //propertiesStream.AddProperty(PropertyTags.PR_CONVERSATION_INDEX, Subject);

            return messageSize;
        }

        public void SaveToMsg(string fileName)
        {
            CompoundFile = new CompoundFile();
            TopLevelProperties = new TopLevelProperties();
            NamedProperties = new NamedProperties(TopLevelProperties);

            long messageSize = WriteToStorage();

            // In the preceding figure, the "__nameid_version1.0" named property mapping storage contains the 
            // three streams  used to provide a mapping from property ID to property name 
            // ("__substg1.0_00020102", "__substg1.0_00030102", and "__substg1.0_00040102") and various other 
            // streams that provide a mapping from property names to property IDs.
            if (!CompoundFile.RootStorage.TryGetStorage(PropertyTags.NameIdStorage, out var nameIdStorage))
                nameIdStorage = CompoundFile.RootStorage.AddStorage(PropertyTags.NameIdStorage);

            nameIdStorage.AddStream(PropertyTags.EntryStream).SetData(new byte[0]);
            nameIdStorage.AddStream(PropertyTags.StringStream).SetData(new byte[0]);
            nameIdStorage.AddStream(PropertyTags.GuidStream).SetData(new byte[0]);

            foreach (var xstProperty in Properties.Items.Where(p => !_NonExportableTagsList.Contains(p.Tag) &&
                                                                    KnownCanonicalNames.KnonwnProperties.Any(k => (PropertyCanonicalName)k.Id == p.Tag)))
            {
                if (xstProperty.PropertySet == null)
                    TopLevelProperties.AddProperty(xstProperty);
                else
                    NamedProperties.AddProperty(xstProperty);
            }

            TopLevelProperties.WriteProperties(CompoundFile.RootStorage, messageSize);
            NamedProperties.WriteProperties(CompoundFile.RootStorage);

            CompoundFile.Save(fileName);
        }
    }
}
