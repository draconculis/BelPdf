using System;
using System.Collections.Generic;
using System.Text;

namespace Dek.Cls
{
    public static class DekRangeExtension
    {
        public static List<DekRange> AddAndMerge(this List<DekRange> me, DekRange range)
        {
            return DekRange.AddAndMerge(me, range);
        }

        public static List<DekRange> SubtractRange(this List<DekRange> me, DekRange range)
        {
            List<DekRange> result = new List<DekRange>();
            foreach (DekRange r in me)
            {
                result.AddRange(r.Subtract(range));
            }

            return result;
        }

        /// <summary>
        /// Returns true if any range in list contains the provided range.
        /// </summary>
        public static bool ContainsRange(this List<DekRange> me, DekRange range)
        {
            foreach (DekRange r in me)
                if (r.Contains(range))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true if any range in list contians i.
        /// </summary>
        public static bool ContainsInteger(this List<DekRange> me, int i)
        {
            return me.ContainsRange(new DekRange(i, i));
        }

        //
        public static string ConvertToText(this List<DekRange> me)
        {
            if (me == null || me.Count == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach(var range in me)
            {
                sb.Append($"{range.Start},{range.Stop};");
            }
            return sb.ToString();
        }

        public static void LoadFromText(this List<DekRange> me, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            string[] asdjh = text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string s in asdjh)
            {
                string[] ns = s.Split(',');
                DekRange tr = new DekRange(int.Parse(ns[0]), int.Parse(ns[1]));
                me.Add(tr);
            }
        }

    }
}