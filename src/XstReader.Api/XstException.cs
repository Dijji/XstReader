using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XstReader
{
    public class XstException : Exception
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        public XstException(string message) : base(message)
        {
        }
    }
}
