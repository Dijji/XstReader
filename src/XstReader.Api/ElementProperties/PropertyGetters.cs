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

        // The properties we read when accessing the recipient table of a message
        public static readonly PropertyGetters<XstRecipient> MessageRecipientProperties = new PropertyGetters<XstRecipient>
        {
            {PropertyCanonicalName.PidTagRecipientType, (r, val) => r.RecipientType = (RecipientTypes)val },
            {PropertyCanonicalName.PidTagDisplayName, (r, val) => r.DisplayName = val },
            {PropertyCanonicalName.PidTagSmtpAddress, (r, val) => r.EmailAddress = val },
            {PropertyCanonicalName.PidTagEmailAddress, (r, val) => r.EmailAddress = r.EmailAddress ?? val},
        };


        // The properties we read when accessing the contents of an attachment
        public static readonly PropertyGetters<XstAttachment> AttachmentContentProperties = new PropertyGetters<XstAttachment>
        {
            {PropertyCanonicalName.PidTagAttachDataBinary, (a, val) => a.Content = val },
        };
    }
}
