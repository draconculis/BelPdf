using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Cls
{
    public static class DateTimeExtensions
    {
        private static readonly string compactPattern = "yyyyMMddHHmmssfff";
        private static readonly string compactPatternShort = "yyyyMMddHHmmss";

        private static readonly string sanePattern = "yyyy-MM-dd HH:mm:ss.fff";
        private static readonly string sanePatternShort = "yyyy-MM-dd HH:mm:ss";
        private static readonly string sanePatternShorter = "yyyy-MM-dd HH:mm";
        private static readonly string saneIsoPattern = "yyyy-MM-dd\\THH:mm:ss.fff";
        private static readonly string saneIsoPatternShort = "yyyy-MM-dd\\THH:mm:ss";

        public static string ToCompactString(this DateTime me, string nullString = "")
        {
            if (me.Year == DateTime.MinValue.Year)
                return nullString;

            return me.ToString(compactPattern);
        }

        public static string ToCompactStringShort(this DateTime me, string nullString = "")
        {
            if (me.Year == DateTime.MinValue.Year)
                return nullString;

            return me.ToString(compactPatternShort);
        }

        public static string ToSaneString(this DateTime me, string nullString = "")
        {
            if (me.Year == DateTime.MinValue.Year)
                return nullString;

            return me.ToString(sanePattern);
        }

        public static string ToSaneStringShort(this DateTime me, string nullString = "")
        {
            if (me.Year == DateTime.MinValue.Year)
                return nullString;

            return me.ToString(sanePatternShort);
        }

        public static string ToSaneStringShorter(this DateTime me, string nullString = "")
        {
            if (me.Year == DateTime.MinValue.Year)
                return nullString;

            return me.ToString(sanePatternShorter);
        }

        public static string ToSaneStringDateOnly(this DateTime me, string nullString = "")
        {
            if (me.Year == DateTime.MinValue.Year)
                return nullString;

            return me.ToString("yyyy-MM-dd");
        }

        public static DateTime ToSaneDateTime(this string me)
        {
            // fail fast
            string trim = me.Trim();
            if (trim.Length < 4
                || trim.Length == 5 // 1999-
                || trim.Length == 6 // 1999-1
                || trim.Length == 8 // 1999-10-
                || trim.Length == 9 // 1999-10-1
                )
                return DateTime.MinValue;

            if (me.EndsWith(":"))
                return DateTime.MinValue;

            foreach (char c in me.ToLower())
            {
                if (!" 1234567890-:.".Contains(c))
                    return DateTime.MinValue;
            }

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
                return DateTime.ParseExact(me, sanePatternShorter, CultureInfo.InvariantCulture);
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

            try
            {
                return DateTime.ParseExact(me, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch { }

            try
            {
                return DateTime.ParseExact(me, "yyyy-MM", CultureInfo.InvariantCulture);
            }
            catch { }

            try
            {
                return DateTime.ParseExact(me, "yyyy", CultureInfo.InvariantCulture);
            }
            catch { }

            return DateTime.MinValue;
        }

        public static bool IsValidSaneDateTime(this string me)
        {
            return me.ToSaneDateTime() > DateTime.MinValue;
        }
    }
}
