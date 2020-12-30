using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XstReader
{
    static public class Extensions
    {
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
