using System;
using System.Collections.Generic;
using System.Text;

namespace XstReader
{
    public class XstMessageBody
    {
        private XstMessage Message { get; set; }

        public XstMessageBodyFormat Format { get; private set; }
        public string Text { get; private set; }

        private byte[] _Bytes = null;
        public byte[] Bytes=>_Bytes ?? (_Bytes = Message?.Encoding?.GetBytes(Text));

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="text"></param>
        /// <param name="format"></param>
        public XstMessageBody(XstMessage message, string text, XstMessageBodyFormat format)
        {
            Message = message;
            Text = text;
            Format = format;
        }
    }
}
