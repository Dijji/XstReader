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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// A Recipient of a Message in a pst/ost File
    /// </summary>
    public class XstRecipient : XstElement
    {
        /// <summary>
        /// Says if this Recipient is Obtained from Message Properties
        /// </summary>
        [DisplayName("Is Generated From Message Properties")]
        [Category("General")]
        [Description("Says if this Recipient is Obtained from Message Properties")]
        public bool IsGeneratedFromMessageProperties { get;}
        /// <summary>
        /// The Message of the Recipient
        /// </summary>
        [DisplayName("Message")]
        [Category("General")]
        [Description("The Message of the Recipient")]
        public XstMessage Message { get; internal set; }
        /// <summary>
        /// The File
        /// </summary>
        [DisplayName("File")]
        [Category("General")]
        [Description("The Container File")]
        public override XstFile XstFile => Message.XstFile;

        /// <summary>
        /// The Type of the Recipient in the Message
        /// </summary>
        [DisplayName("Recipient Type")]
        [Category(@"Mapi Recipient")]
        [Description(@"Represents the recipient type of a recipient on the message.")]
        public RecipientType RecipientType
            => (RecipientType)(Properties[PropertyCanonicalName.PidTagRecipientType]?.Value ?? 0);

        /// <summary>
        /// The Address of the Recipient (SmtpAddress or EmailAddress)
        /// </summary>
        [DisplayName("Address")]
        [Category(@"General")]
        [Description(@"Contains the SMTP address or the email address of the Message object.")]
        public string Address
            => Properties[PropertyCanonicalName.PidTagSmtpAddress]?.ValueAsStringSanitized ??
               Properties[PropertyCanonicalName.PidTagEmailAddress]?.ValueAsStringSanitized;

        /// <summary>
        /// Ctor
        /// </summary>
        public XstRecipient() : base(XstElementType.Recipient)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="propertiesGetter"></param>
        /// param name="isGeneratedFromMessageProperties">
        public XstRecipient(XstMessage message, Func<IEnumerable<XstProperty>> propertiesGetter, bool isGeneratedFromMessageProperties) : this()
        {
            Message = message;
            Properties = new XstPropertySet(propertiesGetter,
                                            (t) => propertiesGetter?.Invoke().FirstOrDefault(p => p.Tag == t),
                                            (t) => propertiesGetter?.Invoke().Any(p => p.Tag == t) ?? false);

            IsGeneratedFromMessageProperties = isGeneratedFromMessageProperties;
        }
        private protected override XstProperty LoadProperty(PropertyCanonicalName tag)
            => null;
        private protected override bool CheckProperty(PropertyCanonicalName tag)
            => false;
        private protected override IEnumerable<XstProperty> LoadProperties()
            => new XstProperty[0];//Ltp.ReadAllProperties(Nid);


        internal void Initialize(XstMessage message)
        {
            Message = message;
        }
    }
}
