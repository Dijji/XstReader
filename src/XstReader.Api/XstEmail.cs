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
