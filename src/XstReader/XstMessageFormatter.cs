using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstReader
{
    internal class XstMessageFormatter
    {
        public XstMessage Message { get; set; }
        public XstFormatOptions FormatOptions { get; set; } = new XstFormatOptions();



        /// <summary>
        /// Ctor
        /// </summary>
        public XstMessageFormatter()
        {
        }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        public XstMessageFormatter(XstMessage message)
        {
            Message = message;
        }


    }
}
