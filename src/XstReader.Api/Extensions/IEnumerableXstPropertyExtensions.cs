// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// Extensions for IEnumerable<XstProperty>
    /// </summary>
    public static class IEnumerableXstPropertyExtensions
    {
        private struct LineProp
        {
            public int line;
            public XstProperty p;
        }

        /// <summary>
        /// Saves the collection of Properties to specified file
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="fileName"></param>
        public static void SaveToFile(this IEnumerable<XstProperty> properties, string fileName)
            => (new IEnumerable<XstProperty>[1] { properties }).SaveToFile(fileName);

        /// <summary>
        /// Saves to file the collection of collections of Properties to specified file
        /// </summary>
        /// <param name="lineProperties"></param>
        /// <param name="fileName"></param>
        public static void SaveToFile(this IEnumerable<IEnumerable<XstProperty>> lineProperties, string fileName)
        {
            // We build a dictionary of queues of line,Property pairs where each queue represents
            // a column in the CSV file, and the line is the line number in the file.
            // The key to the dictionary is the property ID.

            var dict = new Dictionary<string, Queue<LineProp>>();
            int lines = 1;

            foreach(var line in lineProperties)
            {
                foreach(var p in line)
                {
                    if (!dict.TryGetValue(p.Tag.Id0x(), out Queue<LineProp> queue))
                    {
                        queue = new Queue<LineProp>();
                        dict[p.Tag.Id0x()] = queue;
                    }
                    queue.Enqueue(new LineProp { line = lines, p = p });
                }
                lines++;
            }    

            // Now we sort the columns by ID
            var columns = dict.Keys.OrderBy(x => x).ToArray();

            // And finally output the CSV file line by line
            using (var sw = new System.IO.StreamWriter(fileName, false, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                bool hasValue = false;

                for (int line = 0; line < lines; line++)
                {
                    foreach (var col in columns)
                    {
                        var q = dict[col];

                        // First line is always the column headers
                        if (line == 0)
                            AddCsvValue(sb, q.Peek().p.Tag.FriendlyName(), ref hasValue);

                        // After that, output the column value if it has one
                        else if (q.Count > 0 && q.Peek().line == line)
                            AddCsvValue(sb, q.Dequeue().p.DisplayValue, ref hasValue);

                        // Or leave it blank
                        else
                            AddCsvValue(sb, "", ref hasValue);
                    }

                    // Write the completed line out
                    sw.WriteLine(sb.ToString());
                    sb.Clear();
                    hasValue = false;
                }
            }
        }

        private static void AddCsvValue(StringBuilder sb, string value, ref bool hasValue)
        {
            if (hasValue)
                sb.Append(CultureInfo.CurrentCulture.TextInfo.ListSeparator); // aka comma

            if (value != null)
            {
                // Multilingual characters should be quoted, so we will just quote all values,
                // which means we need to double quotes in the value
                // Excel cannot cope with Unicode files with values containing
                // new line characters, so remove those as well
                var val = value.Replace("\"", "\"\"").Replace("\r\n", "; ").Replace("\r", " ").Replace("\n", " ");
                sb.Append("\"");
                sb.Append(EnforceCsvValueLengthLimit(val));
                sb.Append("\"");
            }
            hasValue = true;
        }

        private static int valueLengthLimit = (int)Math.Pow(2, 15) - 12;
        private static string EnforceCsvValueLengthLimit(string value)
        {
            if (value.Length < valueLengthLimit)
                return value;
            else
                return value.Substring(0, valueLengthLimit) + "…";
        }
    }
}
