using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Core.Cls
{
    /// <summary>
    /// https://stackoverflow.com/questions/5248827/convert-datetime-to-julian-date-in-c-sharp-tooadate-safe
    /// </summary>
    public class DekJulianDate
    {
        /// <summary>
        /// Test if date is julian (true) or gregorian (false).
        /// </summary>
        /// <remarks>Throws ArgumentOutOfRangeException for invalid dates</remarks>
        public static bool IsJulianDate(int year, int month, int day)
        {
            // All dates prior to 1582 are in the Julian calendar
            if (year < 1582)
                return true;
            // All dates after 1582 are in the Gregorian calendar
            else if (year > 1582)
                return false;
            else
            {
                // If 1582, check before October 4 (Julian) or after October 15 (Gregorian)
                if (month < 10)
                    return true;
                else if (month > 10)
                    return false;
                else
                {
                    if (day < 5)
                        return true;
                    else if (day > 14)
                        return false;
                    else
                        // Any date in the range 1582-10-5 to 1582-10-14 is invalid 
                        throw new ArgumentOutOfRangeException(
                            $"{year}-{month}-{day} is not valid as it does not exist in either the Julian or the Gregorian calendars.");
                }
            }
        }

        private static double DateToJD(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
        {
            // Determine correct calendar based on date
            bool JulianCalendar = IsJulianDate(year, month, day);

            int M = month > 2 ? month : month + 12;
            if (year < 1)
                year++;
            int Y = month > 2 ? year : year - 1;
            double timeOfDay = hour / 24.0 + minute / 1440.0 + (second + millisecond / 1000.0) / 86400.0;
            double D = day + timeOfDay;
            int B = JulianCalendar ? 0 : 2 - Y / 100 + Y / 100 / 4;

            return (int)(365.25 * (Y + 4716)) + (int)(30.6001 * (M + 1)) + D + B - 1524.5;
        }

        public static double ToJulianDate(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
        {
            return DateToJD(year, month, day, hour, minute, second, millisecond);
        }

        public static double ToJulianDate(DateTime date)
        {
            return DateToJD(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        public static (int year, int month, int day, int hour, int minutes, int seconds, int ms) ToTuple(double julianDate)
        {
            double HMS = julianDate - (int)julianDate - 0.5;

            int hours = (int)(24 * HMS);
            HMS = HMS - (double)(hours / 24.0);
            int mins = (int)(24 * 60 * HMS);
            HMS = HMS - (double)(mins / (24.0 * 60));
            int secs = (int)(24 * 60 * 60 * HMS);
            HMS = HMS - (double)(secs / (24 * 60 * 60));
            int ms = (int)(24 * 60 * 60 * 1000 * HMS);

            double L = julianDate;
            long N = (long)((4 * L) / 146097);
            L = L - ((long)((146097 * N + 3) / 4));
            long I = (long)((4000 * (L + 1) / 1461001));
            L = L - (long)((1461 * I) / 4) + 31;
            long J = (long)((80 * L) / 2447);
            int day = (int)(L - (long)((2447 * J) / 80));
            L = (long)(J / 11);
            int month = (int)(J + 2 - 12 * L);
            int year = (int)(100 * (N - 49) + I + L);

            return (year, month, day, hours, mins, secs, ms);
        }

        /// <summary>
        /// To DateTime. Returns min if not gregorian.
        /// </summary>
        /// <param name="julianDate"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(double julianDate)
        {
            var dateTuple = ToTuple(julianDate);
            return IsJulianDate(dateTuple.year, dateTuple.month, dateTuple.day) 
                ? DateTime.MinValue 
                : DateTime.FromOADate(julianDate - 2415018.5);
        }


        public const string SaneFormat_yyyyMMdd_HHmmssfff = "yyyy-MM-dd HH:mm:ss.fff";
        public const string SaneFormat_yyyyMMdd_HHmmss = "yyyy-MM-dd HH:mm:ss";
        public const string SaneFormat_yyyyMMdd_HHmm = "yyyy-MM-dd HH:mm";
        public const string SaneFormat_yyyyMMdd = "yyyy-MM-dd";

        /// <summary>
        /// To string method (sane = ISO w/o 'T'). Standard c# format string can be provided. 
        /// </summary>
        /// <param name="julianDate"></param>
        /// <param name="format">Subset of c# standard datetime format. Only recognizes sane values, e.g. yyyy for four digit 
        /// year and mm for 2 digit minutes, etc. Also, era 'gg' can be provided.</param>
        public static string ToDateTimeString(double julianDate, string format = SaneFormat_yyyyMMdd_HHmmss)
        {
            var dateTuple = ToTuple(julianDate);
            string fmt2 = "00";
            string fmt3 = "000";
            string fmt4 = "0000";
            // Era - A.D. or B.C
            string era = dateTuple.year < 0 ? "B.C." : "A.D";

            if (string.IsNullOrWhiteSpace(format))
                format = SaneFormat_yyyyMMdd_HHmmss;

            string res = format
                .Replace("dd", dateTuple.day.ToString(fmt2))
                .Replace("fff", dateTuple.ms.ToString(fmt3))
                .Replace("gg", era)
                .Replace("HH", dateTuple.hour.ToString(fmt2))
                .Replace("mm", dateTuple.minutes.ToString(fmt2))
                .Replace("MM", dateTuple.month.ToString(fmt2))
                .Replace("yyyy", dateTuple.year.ToString(fmt4))
                ;

            return res;
        }

        /// <summary>
        /// Calculates time in hour, minutes, seconds from the Julian time part (the decimals).
        /// </summary>
        /// <param name="julianTime"></param>
        /// <returns></returns>
        public (int hour, int minutes, int seconds) JulianTimeToTuple(double julianTime)
        {
            /* Astronomical to civil */
            double julianTimeZeroDate = julianTime + .5;

            double ij = 0;

            ij = (julianTimeZeroDate - Math.Floor(julianTimeZeroDate)) * 86400.0;
            return ((int)Math.Floor(ij / 3600),
                    (int)Math.Floor((ij / 60) % 60),
                    (int)Math.Floor(ij % 60));
        }

        string[] Weekdays = new string[]{"Monday", "Tuesday", "Wednesday",
                          "Thursday", "Friday", "Saturday", "Sunday" };

        /// <summary>
        /// 0 is Monday, 6 is Sunday
        /// </summary>
        public int ToWeekDayNbr(double julianDate)
        {
            return (int)Math.Floor((julianDate + .5) % 7);
        }

        public string ToWeekDay(double julianDate)
        {
            return Weekdays[ToWeekDayNbr(julianDate)];
        }

        public static bool IsValidSaneDate(string me)
        {
            // fail fast
            string trim = me.Trim().Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
            if (trim.Length < 1)
                return false;

            bool bc = false;
            bool ad = false;
            if (trim.ToLower().Contains("bc")
                || trim.ToLower().Contains("b.c.")
                || trim.ToLower().Contains("bce")
                || trim.ToLower().Contains("b.c.e.")
                )
            {
                trim = trim
                    .Replace("b.c.e.", "")
                    .Replace("B.C.E.", "")
                    .Replace("bce", "")
                    .Replace("BCE", "")
                    .Replace("b.c.", "")
                    .Replace("B.C.", "")
                    .Replace("bc", "")
                    .Replace("BC", "")
                    .Trim();
                bc = true;
            }

            // Do NOT allow negative year, as this clashes with '-' separator
            if (trim.ToLower().StartsWith("-"))
                return false;

            if (trim.ToLower().Contains("ad")
                || trim.ToLower().Contains("a.d.")
                || trim.ToLower().Contains("ce")
                || trim.ToLower().Contains("c.e.")
                )
            {
                trim = trim
                    .Replace("a.d.", "")
                    .Replace("A.D.", "")
                    .Replace("ad", "")
                    .Replace("AD", "")
                    .Replace("ce", "")
                    .Replace("CE", "")
                    .Replace("c.e.", "")
                    .Replace("C.E.", "")
                    .Trim();
                ad = true;
            }

            if (ad && bc)
                return false;

            // Valid chars
            foreach (char c in trim.ToLower())
            {
                if (!" 1234567890-:.".Contains(c))
                    return false;
            }

            string[] dateParts = trim.Split(new[] {' ', '-', ':'});

            // First datepart always year
            int year;
            try
            {
                year = int.Parse(dateParts[0]);
                if (year == 0)
                    return false;
                if (dateParts.Length == 1)
                    return true;
            }
            catch
            {
                return false;
            }

            if (bc)
                year = -year;

            // Second should be month
            int month;
            try
            {
                // Enforce 2 digit month
                if (dateParts[1].Length != 2)
                    return false;

                month = int.Parse(dateParts[1]);
                if (month < 1 || month > 12)
                    return false;
                if (dateParts.Length == 2)
                    return true;
            }
            catch
            {
                return false;
            }

            // Third should be day
            int day;
            int[] daysOfMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            try
            {
                // Enforce 2 digit day
                if (dateParts[2].Length != 2)
                    return false;

                day = int.Parse(dateParts[2]);
                if (day < 1) // definitely wrong
                    return false;

                if (IsLeapYear(year) && month == 2 && day > 29)
                    return false;

                if (day > daysOfMonth[month])
                    return false;

                if (dateParts.Length == 3)
                    return true;
            }
            catch
            {
                return false;
            }

            // TODO: Add time parsing
            return true;
        }

        /*
         45 BC, 42 BC, 39 BC, 36 BC, 33 BC, 30 BC, 27 BC, 24 BC, 21 BC, 18 BC, 15 BC, 12 BC, 9 BC, 8 AD, 12 AD, 
          */
        static int[] leapYears = { -45, -42, -39, -36, -33, -30, -27, -24, -21, -18, -15, -12, -9, 8, 12 };
        public static bool IsLeapYear(int year)
        {
            if(year > 1582)
            {
                if (year % 4 != 0)
                    return false;
                else if (year % 100 != 0)
                    return true;
                else if (year % 400 != 0)
                    return false;
                else
                    return true;
            }
            else if(year > 7)
            {
                return year % 4 == 0;
            }
            else if (year > -47)
            {
                return leapYears.Contains(year);
            }

            return false;
        }

    }
}
