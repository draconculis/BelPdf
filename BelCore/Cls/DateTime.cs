using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Cls
{
    public static class DateTimeExtensions
    {
        private static readonly string sanePattern = "yyyy-MM-dd HH:mm:ss.fff";
        private static readonly string sanePatternShort = "yyyy-MM-dd HH:mm:ss";
        private static readonly string saneIsoPattern = "yyyy-MM-dd\\THH:mm:ss.fff";
        private static readonly string saneIsoPatternShort = "yyyy-MM-dd\\THH:mm:ss";

        public static string ToSaneString(this DateTime me)
        {
            return me.ToString(sanePattern);
        }

        public static DateTime ToSaneDateTime(this string me)
        {
            try
            {
                return DateTime.ParseExact(me, sanePattern, CultureInfo.InvariantCulture);
            }
            catch { }

            try
            {
                return DateTime.ParseExact(me, sanePatternShort, CultureInfo.InvariantCulture);
            }
            catch { }

            try
            {
                return DateTime.ParseExact(me, saneIsoPattern, CultureInfo.InvariantCulture);
            }
            catch { }

            try
            {
                return DateTime.ParseExact(me, saneIsoPatternShort, CultureInfo.InvariantCulture);
            }
            catch { }

            return DateTime.MinValue;
        }
    }
}
