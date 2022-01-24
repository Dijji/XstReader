// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021,2022 iluvadev, and released under Ms-PL License.

using System.Collections.Generic;
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    public class XstRecipientSet
    {
        public XstMessage Message { get; private set; }
        private LTP Ltp => Message.XstFile.Ltp;

        private List<XstRecipient> _Items = null;
        public IEnumerable<XstRecipient> Items => GetRecipients();

        public IEnumerable<XstRecipient> this[RecipientType recipientType]
            => Items.Where(r => r.RecipientType == recipientType);

        public XstRecipient Originator => this[RecipientType.Originator].FirstOrDefault();
        public IEnumerable<XstRecipient> To => this[RecipientType.To];
        public IEnumerable<XstRecipient> Cc => this[RecipientType.Cc];
        public IEnumerable<XstRecipient> Bcc => this[RecipientType.Bcc];
        public XstRecipient OriginalSentRepresenting => this[RecipientType.OriginalSentRepresenting].FirstOrDefault();
        public XstRecipient SentRepresenting => this[RecipientType.SentRepresenting].FirstOrDefault();
        public XstRecipient ReceivedRepresenting => this[RecipientType.ReceivedRepresenting].FirstOrDefault();
        public XstRecipient Sender => this[RecipientType.Sender].FirstOrDefault();
        public XstRecipient ReceivedBy => this[RecipientType.ReceivedBy].FirstOrDefault();

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        public XstRecipientSet(XstMessage message)
        {
            Message = message;
        }


        public IEnumerable<XstRecipient> GetRecipients()
        {
            if (_Items == null)
            {
                _Items = GetRecipientsInternal().ToList();
                _Items.Add(new XstRecipient(Message, GetPropertiesOriginalSentRepresenting));
                _Items.Add(new XstRecipient(Message, GetPropertiesSentRepresenting));
                _Items.Add(new XstRecipient(Message, GetPropertiesReceivedRepresenting));
                _Items.Add(new XstRecipient(Message, GetPropertiesSender));
                _Items.Add(new XstRecipient(Message, GetPropertiesReceivedBy));
            }
            return _Items.Where(r => r.Properties.Items.Any(p => p.Tag != PropertyCanonicalName.PidTagRecipientType));
        }

        private List<XstProperty> GetPropertiesOriginalSentRepresenting()
        {
            var propList = new List<XstProperty>
            {
                Message.Properties[PropertyCanonicalName.PidTagOriginalSentRepresentingAddressType]?.CopyToNew(PropertyCanonicalName.PidTagAddressType),
                Message.Properties[PropertyCanonicalName.PidTagOriginalSentRepresentingEmailAddress]?.CopyToNew(PropertyCanonicalName.PidTagEmailAddress),
                Message.Properties[PropertyCanonicalName.PidTagOriginalSentRepresentingEntryId]?.CopyToNew(PropertyCanonicalName.PidTagEntryId),
                Message.Properties[PropertyCanonicalName.PidTagOriginalSentRepresentingName]?.CopyToNew(PropertyCanonicalName.PidTagDisplayName),
                Message.Properties[PropertyCanonicalName.PidTagOriginalSentRepresentingSearchKey]?.CopyToNew(PropertyCanonicalName.PidTagSearchKey),
            }.Where(p => p != null).ToList();
            if (propList.Any())
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, EpropertyType.PtypInteger32, () => RecipientType.OriginalSentRepresenting));

            return propList;
        }
        private List<XstProperty> GetPropertiesSentRepresenting()
        {
            var propList = new List<XstProperty>
            {
                Message.Properties[PropertyCanonicalName.PidTagSentRepresentingAddressType]?.CopyToNew(PropertyCanonicalName.PidTagAddressType),
                Message.Properties[PropertyCanonicalName.PidTagSentRepresentingEmailAddress]?.CopyToNew(PropertyCanonicalName.PidTagEmailAddress),
                Message.Properties[PropertyCanonicalName.PidTagSentRepresentingEntryId]?.CopyToNew(PropertyCanonicalName.PidTagEntryId),
                Message.Properties[PropertyCanonicalName.PidTagSentRepresentingFlags]?.CopyToNew(PropertyCanonicalName.PidTagRecipientFlags),
                Message.Properties[PropertyCanonicalName.PidTagSentRepresentingName]?.CopyToNew(PropertyCanonicalName.PidTagDisplayName),
                Message.Properties[PropertyCanonicalName.PidTagSentRepresentingSearchKey]?.CopyToNew(PropertyCanonicalName.PidTagSearchKey),
                Message.Properties[PropertyCanonicalName.PidTagSentRepresentingSmtpAddress]?.CopyToNew(PropertyCanonicalName.PidTagSmtpAddress),
            }.Where(p => p != null).ToList();
            if (propList.Any())
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, EpropertyType.PtypInteger32, () => RecipientType.SentRepresenting));

            return propList;
        }
        private List<XstProperty> GetPropertiesReceivedRepresenting()
        {
            var propList = new List<XstProperty>
            {
                Message.Properties[PropertyCanonicalName.PidTagReceivedRepresentingAddressType]?.CopyToNew(PropertyCanonicalName.PidTagAddressType),
                Message.Properties[PropertyCanonicalName.PidTagReceivedRepresentingEmailAddress]?.CopyToNew(PropertyCanonicalName.PidTagEmailAddress),
                Message.Properties[PropertyCanonicalName.PidTagReceivedRepresentingEntryId]?.CopyToNew(PropertyCanonicalName.PidTagEntryId),
                Message.Properties[PropertyCanonicalName.PidTagReceivedRepresentingName]?.CopyToNew(PropertyCanonicalName.PidTagDisplayName),
                Message.Properties[PropertyCanonicalName.PidTagReceivedRepresentingSearchKey]?.CopyToNew(PropertyCanonicalName.PidTagSearchKey),
                Message.Properties[PropertyCanonicalName.PidTagReceivedRepresentingSmtpAddress]?.CopyToNew(PropertyCanonicalName.PidTagSmtpAddress),
            }.Where(p => p != null).ToList();
            if (propList.Any())
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, EpropertyType.PtypInteger32, () => RecipientType.ReceivedRepresenting));

            return propList;
        }
        private List<XstProperty> GetPropertiesSender()
        {
            var propList = new List<XstProperty>
            {
                Message.Properties[PropertyCanonicalName.PidTagSenderAddressType]?.CopyToNew(PropertyCanonicalName.PidTagAddressType),
                Message.Properties[PropertyCanonicalName.PidTagSenderEmailAddress]?.CopyToNew(PropertyCanonicalName.PidTagEmailAddress),
                Message.Properties[PropertyCanonicalName.PidTagSenderEntryId]?.CopyToNew(PropertyCanonicalName.PidTagEntryId),
                Message.Properties[PropertyCanonicalName.PidTagSenderIdStatus]?.CopyToNew(PropertyCanonicalName.PidTagSenderIdStatus),
                Message.Properties[PropertyCanonicalName.PidTagSenderName]?.CopyToNew(PropertyCanonicalName.PidTagDisplayName),
                Message.Properties[PropertyCanonicalName.PidTagSenderSearchKey]?.CopyToNew(PropertyCanonicalName.PidTagSearchKey),
                Message.Properties[PropertyCanonicalName.PidTagSenderSmtpAddress]?.CopyToNew(PropertyCanonicalName.PidTagSmtpAddress),
                Message.Properties[PropertyCanonicalName.PidTagSenderTelephoneNumber]?.CopyToNew(PropertyCanonicalName.PidTagSenderTelephoneNumber),
            }.Where(p => p != null).ToList();
            if (propList.Any())
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, EpropertyType.PtypInteger32, () => RecipientType.Sender));

            return propList;
        }
        private List<XstProperty> GetPropertiesReceivedBy()
        {
            var propList = new List<XstProperty>
            {
                Message.Properties[PropertyCanonicalName.PidTagReceivedByAddressType]?.CopyToNew(PropertyCanonicalName.PidTagAddressType),
                Message.Properties[PropertyCanonicalName.PidTagReceivedByEmailAddress]?.CopyToNew(PropertyCanonicalName.PidTagEmailAddress),
                Message.Properties[PropertyCanonicalName.PidTagReceivedByEntryId]?.CopyToNew(PropertyCanonicalName.PidTagEntryId),
                Message.Properties[PropertyCanonicalName.PidTagReceivedByName]?.CopyToNew(PropertyCanonicalName.PidTagDisplayName),
                Message.Properties[PropertyCanonicalName.PidTagReceivedBySearchKey]?.CopyToNew(PropertyCanonicalName.PidTagSearchKey),
                Message.Properties[PropertyCanonicalName.PidTagReceivedBySmtpAddress]?.CopyToNew(PropertyCanonicalName.PidTagSmtpAddress),
            }.Where(p => p != null).ToList();
            if (propList.Any())
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, EpropertyType.PtypInteger32, () => RecipientType.ReceivedBy));

            return propList;
        }
        private IEnumerable<XstRecipient> GetRecipientsInternal()
        {
            // Read the recipient table for the message
            var recipientsNid = new NID(EnidSpecial.NID_RECIPIENT_TABLE);
            if (!Ltp.IsTablePresent(Message.SubNodeTreeProperties, recipientsNid))
                return new XstRecipient[0];
            //    throw new XstException("Could not find expected Recipient table");

            return Ltp.ReadTable<XstRecipient>(Message.SubNodeTreeProperties, recipientsNid,
                                               (r, id) => r.Nid = new NID(id),
                                               r => r.Initialize(Message));
            // Sort the properties
            //.Select(r => { r.Properties.OrderBy(p => p.Tag).ToList(); return r; });
        }

        public void ClearContents()
        {
            if (_Items != null)
                _Items.ForEach(r => r.ClearContents());

            _Items = null;
        }
    }
}
