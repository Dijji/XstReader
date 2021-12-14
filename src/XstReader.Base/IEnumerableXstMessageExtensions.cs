using System.Collections.Generic;
using System.Linq;

namespace XstReader
{
    public static class IEnumerableXstMessageExtensions
    {
        public static void SavePropertiesToFile(this IEnumerable<XstMessage> messages, string fileName)
            => messages.Select(m => m.Properties).SaveToFile(fileName);
    }
}
