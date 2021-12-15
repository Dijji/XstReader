﻿// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System.Collections.Generic;
using XstReader.Properties;

namespace XstReader
{
    public class XstRecipient
    {
        public RecipientTypes RecipientType { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public List<XstProperty> Properties { get; private set; } = new List<XstProperty>();
    }
}