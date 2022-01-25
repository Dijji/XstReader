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
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    public class XstRecipient : XstElement
    {
        public XstMessage Message { get; internal set; }
        protected internal override XstFile XstFile => Message.XstFile;

        public RecipientType RecipientType 
            => (RecipientType)(Properties[PropertyCanonicalName.PidTagRecipientType]?.Value ?? 0);

        public string Address
            => Properties[PropertyCanonicalName.PidTagSmtpAddress]?.Value ??
               Properties[PropertyCanonicalName.PidTagEmailAddress]?.Value;

        public XstRecipient()
        {
        }

        public XstRecipient(XstMessage message, Func<IEnumerable<XstProperty>> propertiesGetter)
        {
            Message = message;
            Properties = new XstPropertySet(propertiesGetter);
        }

        private protected override IEnumerable<XstProperty> LoadProperties()
            => new XstProperty[0];//Ltp.ReadAllProperties(Nid);


        internal void Initialize(XstMessage message)
        {
            Message = message;
        }
    }
}
