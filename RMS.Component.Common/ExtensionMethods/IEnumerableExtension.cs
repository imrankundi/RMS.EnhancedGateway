using System.Collections.Generic;
using System.Linq;

namespace RMS.Component.Common.ExtensionMethods
{
    public static class IEnumerableExtension
    {
        public static string ToHexString(this IEnumerable<byte> bytes)
        {
            return string.Join("", bytes.Select(b => ("0" + b.ToString("X")).Right(2)));
        }
    }
}
