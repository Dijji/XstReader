using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XstReader.ElementProperties;

namespace XstReader
{
    public partial class XstMessage
    {
         private string _ExportFileName = null;
        public string ExportFileName => _ExportFileName ?? (_ExportFileName = String.Format("{0:yyyy-MM-dd HHmm} {1}", Date, Subject).Truncate(150).ReplaceInvalidFileNameChars(" "));

        public string ExportFileExtension => IsBodyHtml ? "html" : IsBodyRtf ? "rtf" : "txt";

        public string GetBodyAsHtmlString(bool embedInlineAttachments = true)
        {
            string body = GetBodyAsHtmlStringBase();

            if (embedInlineAttachments && MayHaveAttachmentsInline)
                body = EmbedAttachments(body);  // Returns null if this is not appropriate

            return body;
        }
        private string GetBodyAsHtmlStringBase()
        {
            if (BodyHtml != null)
                return BodyHtml; // This will be plain ASCII
            else if (Html != null)
            {
                if (Encoding != null)
                {
                    return EscapeUnicodeCharacters(new String(Encoding.GetChars(Html)));
                }
            }
            else if (BodyPlainText != null) // Not really expecting this as a source of HTML
                return EscapeUnicodeCharacters(BodyPlainText);

            return null;
        }

        private string EmbedAttachments(string body)
        {
            if (body == null)
                return null;

            if (!MayHaveAttachmentsInline)
                return body;

            var dict = Attachments.Where(a => a.HasContentId)
                                  .GroupBy(a => a.ContentId)
                                  .Select(g => g.First())
                                  .ToDictionary(a => a.ContentId);

            return Regex.Replace(body, @"(="")cid:(.*?)("")", match =>
            {
                if (dict.TryGetValue(match.Groups[2].Value, out XstAttachment a))
                {
                    // There are limits to what we can push into an inline data image, 
                    // but we don't know exactly what
                    // Todo handle limit when known
                    a.WasRenderedInline = true;
                    string cooked = null;
                    using (var s = new MemoryStream())
                    {
                        a.SaveToStream(s);
                        s.Seek(0, SeekOrigin.Begin);
                        cooked = match.Groups[1] + @"data:image/jpg;base64," + EscapeString(Convert.ToBase64String(s.ToArray())) + match.Groups[3];
                    }
                    return cooked;
                }

                return match.Value;
            }, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        private static string EscapeUnicodeCharacters(string source)
        {
            int length = source.Length;
            var escaped = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                char ch = source[i];

                if (ch >= '\x00a0')
                {
                    escaped.AppendFormat("&#x{0};", ((int)ch).ToString("X4"));
                }
                else
                {
                    escaped.Append(ch);
                }
            }

            return escaped.ToString();
        }

        private string EscapeString(string s)
        {
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length;)
            {
                int len = Math.Min(s.Length - i, 32766);
                sb.Append(Uri.EscapeDataString(s.Substring(i, len)));
                i += len;
            }
            return sb.ToString();
        }

        private Encoding GetEncoding()
        {
            var p = Properties.ItemsNonBinary.FirstOrDefault(x => x.PropertySetGuid == "00020386-0000-0000-c000-000000000046" && x.Name == "content-type");
            if (p != null)
            {

                Match m = Regex.Match((string)p.Value, @".*charset=""(.*?)""");
                if (m.Success)
                    return Encoding.GetEncoding(m.Groups[1].Value);
            }

            p = Properties[PropertyCanonicalName.PidTagInternetCodepage];
            if (p != null)
            {
                return Encoding.GetEncoding((int)p.Value);
            }

            return null;
        }
    }
}
