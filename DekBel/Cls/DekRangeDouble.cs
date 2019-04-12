using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dek.Cls
{
    public class TextRange : IComparable, IEquatable<TextRange>
    {
        public int Start { get; }
        public int Stop { get; }

        public TextRange(int start, int stop, bool stopAsLength = false)
        {
            if (start < 0 || stop < 0)
                throw new ArgumentException($"Index out of range. Start: {start}, Stop: {stop}.");

            if (stopAsLength)
                stop = start + stop - 1;

            if (start > stop)
            {
                Stop = start;
                Start = stop;
            }
            else
            {
                Start = start;
                Stop = stop;
            }
        }

        public int Len => Stop - Start + 1;

        public bool Intersects(TextRange range) =>
            (range.Start >= Start && range.Start <= Stop)
            || (range.Stop >= Start && range.Stop <= Stop)
            || (range.Start <= Start) && (range.Stop >= Stop)
            || (range.Start >= Start) && (range.Stop <= Stop);


        public bool Contains(TextRange range) => (range.Start >= Start && range.Stop <= Stop);

        public bool Contains(int pos) => (pos >= Start && pos <= Stop);

        /// <summary>
        /// Orders by Start
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is TextRange range)
            {
                return Start == range.Start ? 0 : Start > range.Start ? 1 : -1;
            }

            throw new ArgumentException("Not a range");
        }

        /// <summary>
        /// Returns a list with 0, 1, or 2 textRanges.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<TextRange> Subtract(TextRange excludeRange)
        {
            // Nothing excluded
            if (!Intersects(excludeRange))
                return new List<TextRange> { this };

            // Delete all
            if (excludeRange.Contains(this))
                return new List<TextRange>();

            // Handle left edge case
            if (excludeRange.Start <= Start)
            {
                // All excluded
                if (excludeRange.Stop >= Stop)
                    return new List<TextRange>();

                return new List<TextRange> { new TextRange(excludeRange.Stop + 1, Stop) };
            }

            // We know that excluded.Start is in range
            if (excludeRange.Stop >= Stop)
            {
                return new List<TextRange> { new TextRange(Start, excludeRange.Start - 1) };
            }

            // We now come to the cases where we get 2 return values
            // exclude range is within (Start + 1, Stop -1)
            var resultSet = new List<TextRange>();
            resultSet.Add(new TextRange(Start, excludeRange.Start - 1));
            resultSet.Add(new TextRange(excludeRange.Stop + 1, Stop));

            return resultSet;
        }


        /// <summary>
        /// Given this as the master range, apply a set of ranges within this range and 
        /// subtract these. Produce a set of new ranges.
        /// </summary>
        /// <param name="excludeRanges"></param>
        /// <returns></returns>
        public List<TextRange> SubtractSubRanges(List<TextRange> excludeRanges)
        {
            // Assert that no range is outside of this range
            foreach (var range in excludeRanges)
            {
                if (!Contains(range))
                    throw new ArgumentException("Sub range cannot be outside master range.");
            }

            // Assert that none of the ranges overlap



            return new List<TextRange>();
        }

        public static List<TextRange> MergeConnectedRanges(List<TextRange> ranges)
        {
            if (ranges.Count == 0)
                return new List<TextRange> { };

            if (ranges.Count == 1)
                return new List<TextRange> { ranges[0] };

            var super = new List<TextRange>();
            List<TextRange> sortedRanges = ranges.OrderBy(x => x).ToList();

            int i = 0, j = 1;
            bool done = false;
            while (!done)
            {
                var cur = sortedRanges[i];
                var next = sortedRanges[j];

                while (cur.Intersects(next))
                {
                    cur = Super(cur, next);

                    if (j + 1 == sortedRanges.Count)
                    {
                        done = true;
                        break;
                    }
                    next = sortedRanges[++j];
                }

                super.Add(cur);

                i = j;
                j++;

                if (done || j == sortedRanges.Count)
                {
                    if (!cur.Intersects(sortedRanges[i]))
                        super.Add(sortedRanges[i]);
                    done = true;
                }
            }

            return super;
        }

        public static List<TextRange> AddAndMerge(List<TextRange> existingRange, TextRange range)
        {
            var newRange = Add(existingRange, range);
            return MergeConnectedRanges(newRange);
        }

        public static List<TextRange> Add(List<TextRange> existingRange, TextRange range)
        {
            List<TextRange> textRanges = new List<TextRange>(existingRange);
            textRanges.Add(range);
            return textRanges;
        }

        /// <summary>
        /// Creates a super range that includes the two provided ranges.
        /// Includes any gap between the ranges,
        /// </summary>
        /// <param name="range1"></param>
        /// <param name="range2"></param>
        /// <returns></returns>
        public static TextRange Super(TextRange range1, TextRange range2)
        {
            int min = range1.Start < range2.Start ? range1.Start : range2.Start;
            int max = range1.Stop > range2.Stop ? range1.Stop : range2.Stop;
            return new TextRange(min, max);
        }


        public override bool Equals(object other) => (Start == ((TextRange)other).Start && Stop == ((TextRange)other).Stop);
        public bool Equals(TextRange other) => (Start == ((TextRange)other).Start && Stop == ((TextRange)other).Stop);

        public static bool operator ==(TextRange range1, TextRange range2) => range1.Equals(range2);
        public static bool operator !=(TextRange range1, TextRange range2) => !range1.Equals(range2);

        public override string ToString()
        {
            return $"({Start}, {Stop})";
        }

        public override int GetHashCode()
        {
            var hashCode = 402335580;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + Stop.GetHashCode();
            return hashCode;
        }
    }

    public static class TextRangeExtension
    {
        public static List<TextRange> AddAndMerge(this List<TextRange> me, TextRange range)
        {
            return TextRange.AddAndMerge(me, range);
        }

        /// <summary>
        /// Returns true if any range in list contians the provided range.
        /// </summary>
        public static bool ContainsRange(this List<TextRange> me, TextRange range)
        {
            foreach (TextRange r in me)
                if (r.Contains(range))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true if any range in list contians i.
        /// </summary>
        public static bool ContainsInteger(this List<TextRange> me, int i)
        {
            return me.ContainsRange(new TextRange(i, i));
        }

        //
        public static string ConvertToText(this List<TextRange> me)
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

        public static void LoadFromText(this List<TextRange> me, string text)
        {
            me = new List<TextRange>();
            if (string.IsNullOrWhiteSpace(text))
                return;

            string[] asdjh = text.Split(';');
            foreach(string s in asdjh)
            {
                string[] ns = s.Split(',');
                TextRange tr = new TextRange(int.Parse(ns[0]), int.Parse(ns[1]));
                me.Add(tr);
            }
        }

    }
}