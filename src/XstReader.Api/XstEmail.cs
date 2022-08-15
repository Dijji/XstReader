// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

using System;
using System.Collections.Generic;
using System.Text;

namespace XstReader
{
    /// <summary>
    /// Class for an eMail stored inside an ost/pst file
    /// It derives from <see cref="XstMessage"/>
    /// </summary>
    public class XstEmail: XstMessageDecoratorBase
    {

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        internal XstEmail(XstMessage message): base(message)
        {
        }

    }
}
