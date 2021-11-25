using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostTradeMonitor.Core.Extensions
{
    public static class UtilityExtensions
    {
        public static byte[] ToBytes(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static string AsString(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
