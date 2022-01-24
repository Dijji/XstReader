// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    public class XstRecipient : XstElement
    {
        public XstMessage Message { get; internal set; }
        protected internal override XstFile XstFile => Message.XstFile;

        private RecipientType? _RecipientType = null;
        public RecipientType RecipientType
        {
            get => _RecipientType ?? (RecipientType)(Properties[PropertyCanonicalName.PidTagRecipientType]?.Value ?? 0);
            set => _RecipientType = value;
        }

        private string _EmailAddress = null;
        public string EmailAddress
        {
            get => _EmailAddress ?? Properties[PropertyCanonicalName.PidTagSmtpAddress]?.Value ??
                                    Properties[PropertyCanonicalName.PidTagEmailAddress]?.Value;
            private set => _EmailAddress = value;
        }

        public XstRecipient()
        {

        }
        public XstRecipient(XstMessage message, Func<IEnumerable<XstProperty>> propertiesGetter)
        {
            Message = message;
            Properties = new XstPropertySet(propertiesGetter);
        }

        public XstRecipient(string displayName, string emailAddress, RecipientType recipientTypes)
        {
            DisplayName = displayName;
            EmailAddress = emailAddress;
            RecipientType = recipientTypes;
        }

        private protected override IEnumerable<XstProperty> LoadProperties()
            => new XstProperty[0];//Ltp.ReadAllProperties(Nid);


        internal void Initialize(XstMessage message)
        {
            Message = message;
        }
    }
}
