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


    }
}
