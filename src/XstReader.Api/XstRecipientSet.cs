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
    /// <summary>
    /// A set of Recipients of a Message in a pst/ost File
    /// </summary>
    public class XstRecipientSet
    {
        /// <summary>
        /// The Message of the Recipients
        /// </summary>
        public XstMessage Message { get; private set; }
        private LTP Ltp => Message.XstFile.Ltp;

        private List<XstRecipient> _Items = null;

        /// <summary>
        /// The Recipients of the Message
        /// </summary>
        public IEnumerable<XstRecipient> Items => GetRecipients();

        /// <summary>
        /// Gets the Recipients of specified type in the Message
        /// </summary>
        /// <param name="recipientType"></param>
        /// <returns></returns>
        public IEnumerable<XstRecipient> this[RecipientType recipientType]
            => Items.Where(r => r.RecipientType == recipientType);

        /// <summary>
        /// The Recipient of type "Originator" of the Message
        /// </summary>
        public XstRecipient Originator => this[RecipientType.Originator].FirstOrDefault();
        /// <summary>
        /// The Recipients of type "To" of the Message
        /// </summary>
        public IEnumerable<XstRecipient> To => this[RecipientType.To];
        /// <summary>
        /// The Recipients of type "Cc" of the Message
        /// </summary>
        public IEnumerable<XstRecipient> Cc => this[RecipientType.Cc];
        /// <summary>
        /// The Recipients of type "Bcc" of the Message
        /// </summary>
        public IEnumerable<XstRecipient> Bcc => this[RecipientType.Bcc];
        /// <summary>
        /// The Recipient of type "OriginalSentRepresenting" of the Message
        /// </summary>
        public XstRecipient OriginalSentRepresenting => this[RecipientType.OriginalSentRepresenting].FirstOrDefault();
        /// <summary>
        /// The Recipient of type "SentRepresenting" of the Message
        /// </summary>
        public XstRecipient SentRepresenting => this[RecipientType.SentRepresenting].FirstOrDefault();
        /// <summary>
        /// The Recipient of type "ReceivedRepresenting" of the Message
        /// </summary>
        public XstRecipient ReceivedRepresenting => this[RecipientType.ReceivedRepresenting].FirstOrDefault();
        /// <summary>
        /// The Recipient of type "Sender" of the Message
        /// </summary>
        public XstRecipient Sender => this[RecipientType.Sender].FirstOrDefault();
        /// <summary>
        /// The Recipient of type "ReceivedBy" of the Message
        /// </summary>
        public XstRecipient ReceivedBy => this[RecipientType.ReceivedBy].FirstOrDefault();

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        public XstRecipientSet(XstMessage message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the Recipients of the Message
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XstRecipient> GetRecipients()
        {
            if (_Items == null)
            {
                _Items = GetRecipientsInternal().ToList();
                _Items.Add(new XstRecipient(Message, GetPropertiesOriginalSentRepresenting, true));
                _Items.Add(new XstRecipient(Message, GetPropertiesSentRepresenting, true));
                _Items.Add(new XstRecipient(Message, GetPropertiesReceivedRepresenting, true));
                _Items.Add(new XstRecipient(Message, GetPropertiesSender, true));
                _Items.Add(new XstRecipient(Message, GetPropertiesReceivedBy, true));
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
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, PropertyType.PT_LONG, RecipientType.OriginalSentRepresenting));

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
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, PropertyType.PT_LONG, RecipientType.SentRepresenting));

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
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, PropertyType.PT_LONG, RecipientType.ReceivedRepresenting));

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
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, PropertyType.PT_LONG, RecipientType.Sender));

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
                propList.Add(new XstProperty(PropertyCanonicalName.PidTagRecipientType, PropertyType.PT_LONG, RecipientType.ReceivedBy));

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

        /// <summary>
        /// Clear contents and memory
        /// </summary>
        public void ClearContents()
        {
            //if (_Items != null)
            //    _Items.ForEach(r => r.ClearContentsInternal());

            _Items = null;
        }
    }
}
