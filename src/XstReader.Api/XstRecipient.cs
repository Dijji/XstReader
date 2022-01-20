// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

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
        public RecipientType RecipientType => _RecipientType ?? (RecipientType)(XstPropertySet[PropertyCanonicalName.PidTagRecipientType]?.Value ?? 0);
        private string _DisplayName = null;
        public string DisplayName => _DisplayName ?? XstPropertySet[PropertyCanonicalName.PidTagDisplayName]?.Value;

        private string _EmailAddress = null;
        public string EmailAddress => _EmailAddress ?? XstPropertySet[PropertyCanonicalName.PidTagSmtpAddress]?.Value ??
                                                       XstPropertySet[PropertyCanonicalName.PidTagEmailAddress]?.Value;


        public XstRecipient()
        {

        }

        public XstRecipient(string displayName, string emailAddress, RecipientType recipientTypes)
        {
            _DisplayName = displayName;
            _EmailAddress = emailAddress;
            _RecipientType = recipientTypes;
        }

        internal void Initialize(XstMessage message)
        {
            Message = message;
        }

        private protected override IEnumerable<XstProperty> LoadProperties()
            => new XstProperty[0];//Ltp.ReadAllProperties(Nid);


        internal void Initialize(NID nid, XstMessage message)
        {
            Nid = nid;
            Message = message;
        }
    }
}
