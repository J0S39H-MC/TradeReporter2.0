using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTradeReporter.Core.Extensions
{
    public static class UtilityExtensions
    {
        public static byte[] ToBytes(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static string ToBytes(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
