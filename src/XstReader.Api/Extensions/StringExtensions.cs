// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.
// Copyright (c) 2020, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XstReader
{
    /// <summary>
    /// Extionsions for String
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Appends new line to string, returning the new string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string AppendNewLine(this string text)
            => text + Environment.NewLine;

        /// <summary>
        /// Convert the string as a valid html text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Truncates the string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        static Regex removeInvalidChars = null;
        /// <summary>
        /// Returns a string with invalid chars for File name replaced with specified string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="with"></param>
        /// <returns></returns>
        public static string ReplaceInvalidFileNameChars(this string value, string with = "")
        {
            if (removeInvalidChars == null)
                removeInvalidChars = new Regex(String.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))),
                        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return removeInvalidChars.Replace(value, with);
        }

    }
}
