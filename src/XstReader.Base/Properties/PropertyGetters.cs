// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;

namespace XstReader.Properties
{
    // Property getters are used to specify which properties should be retrieved from a property context
    // or table context, and where they should be stored.
    // T is the target object, Action arguments are target object, column value 
    internal class PropertyGetters<T> : Dictionary<EpropertyTag, Action<T, dynamic>>
    {
    }
}
