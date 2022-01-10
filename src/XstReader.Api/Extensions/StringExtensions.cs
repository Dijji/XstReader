// Copyright (c) 2020, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XstReader
{
    internal static class StringExtensions
    {
        public static string AppendNewLine(this string text)
            => text + Environment.NewLine;

        public static string TextToHtml(this string text)
        {
            var replacements = new Dictionary<string, string>
            {
                {"<", "&lt;"},
                {">", "&gt;" },
                {"/", "&sol;" },
                {"\\", "&bsol;" },
                {"\r\n", "\r" },
                {"\n", "\r" },
                {"\r", "<br>\r\n" },
                {"  ", " &nbsp;" }
            };
            foreach (var rep in replacements)
                text = text.Replace(rep.Key, rep.Value);
            return text;
        }
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        static Regex removeInvalidChars = null;
        public static string ReplaceInvalidFileNameChars(this string value, string with = "")
        {
            if (removeInvalidChars == null)
                removeInvalidChars = new Regex(String.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))),
                        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return removeInvalidChars.Replace(value, with);
        }

    }
}
