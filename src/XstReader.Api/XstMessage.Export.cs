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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XstReader.ElementProperties;

namespace XstReader
{
    public partial class XstMessage
    {
        private string EmbedAttachments(string body)
        {
            if (body == null)
                return null;

            if (!Attachments.Inlines().Any())
                return body;

            var dict = Attachments.Inlines()
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
            var p = Properties.Items.NonBinary().FirstOrDefault(x => x.PropertySet?.Guid()?.ToString() == "00020386-0000-0000-c000-000000000046" && x.Name == "content-type");
            if (p != null)
            {

                Match m = Regex.Match((string)p.Value, @".*charset=""(.*?)""");
                if (m.Success)
                    return Encoding.GetEncoding(m.Groups[1].Value);
            }

            p = Properties[PropertyCanonicalName.PidTagInternetCodepage];
            if (p != null)
            {
                try { return Encoding.GetEncoding((int)p.Value); }
                catch { return null; }
            }

            return null;
        }
    }
}
