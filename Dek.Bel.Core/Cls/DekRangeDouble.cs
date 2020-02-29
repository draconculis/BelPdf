using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dek.Cls
{
    public class DekRangeDouble : IComparable, IEquatable<DekRangeDouble>
    {
        public double Start { get; }
        public double Stop { get; }

        public DekRangeDouble(double start, double stop, bool stopAsLength = false)
        {
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

        public double Len => Stop - Start + 1;

        public bool Intersects(DekRangeDouble range) =>
            (range.Start >= Start && range.Start <= Stop)
            || (range.Stop >= Start && range.Stop <= Stop)
            || (range.Start <= Start) && (range.Stop >= Stop)
            || (range.Start >= Start) && (range.Stop <= Stop);


        public bool Contains(DekRangeDouble range) => (range.Start >= Start && range.Stop <= Stop);

        public bool Contains(double pos) => (pos >= Start && pos <= Stop);

        /// <summary>
        /// Orders by Start
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is DekRangeDouble range)
            {
                return Start == range.Start ? 0 : Start > range.Start ? 1 : -1;
            }

            throw new ArgumentException("Not a range");
        }

        /// <summary>
        /// Returns a list with 0, 1, or 2 DekRangeDoubles.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<DekRangeDouble> Subtract(DekRangeDouble excludeRange)
        {
            // Nothing excluded
            if (!Intersects(excludeRange))
                return new List<DekRangeDouble> { this };

            // Delete all
            if (excludeRange.Contains(this))
                return new List<DekRangeDouble>();

            // Handle left edge case
            if (excludeRange.Start <= Start)
            {
                // All excluded
                if (excludeRange.Stop >= Stop)
                    return new List<DekRangeDouble>();

                return new List<DekRangeDouble> { new DekRangeDouble(excludeRange.Stop + 1, Stop) };
            }

            // We know that excluded.Start is in range
            if (excludeRange.Stop >= Stop)
            {
                return new List<DekRangeDouble> { new DekRangeDouble(Start, excludeRange.Start - 1) };
            }

            // We now come to the cases where we get 2 return values
            // exclude range is within (Start + 1, Stop -1)
            var resultSet = new List<DekRangeDouble>();
            resultSet.Add(new DekRangeDouble(Start, excludeRange.Start - 1));
            resultSet.Add(new DekRangeDouble(excludeRange.Stop + 1, Stop));

            return resultSet;
        }


        /// <summary>
        /// Given this as the master range, apply a set of ranges within this range and 
        /// subtract these. Produce a set of new ranges.
        /// </summary>
        /// <param name="excludeRanges"></param>
        /// <returns></returns>
        public List<DekRangeDouble> SubtractSubRanges(List<DekRangeDouble> excludeRanges)
        {
            // Assert that no range is outside of this range
            foreach (var range in excludeRanges)
            {
                if (!Contains(range))
                    throw new ArgumentException("Sub range cannot be outside master range.");
            }

            // Assert that none of the ranges overlap



            return new List<DekRangeDouble>();
        }

        public static List<DekRangeDouble> MergeConnectedRanges(List<DekRangeDouble> ranges)
        {
            if (ranges.Count == 0)
                return new List<DekRangeDouble> { };

            if (ranges.Count == 1)
                return new List<DekRangeDouble> { ranges[0] };

            var super = new List<DekRangeDouble>();
            List<DekRangeDouble> sortedRanges = ranges.OrderBy(x => x).ToList();

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

        public static List<DekRangeDouble> AddAndMerge(List<DekRangeDouble> existingRange, DekRangeDouble range)
        {
            var newRange = Add(existingRange, range);
            return MergeConnectedRanges(newRange);
        }

        public static List<DekRangeDouble> Add(List<DekRangeDouble> existingRange, DekRangeDouble range)
        {
            List<DekRangeDouble> DekRangeDoubles = new List<DekRangeDouble>(existingRange);
            DekRangeDoubles.Add(range);
            return DekRangeDoubles;
        }

        /// <summary>
        /// Creates a super range that includes the two provided ranges.
        /// Includes any gap between the ranges,
        /// </summary>
        /// <param name="range1"></param>
        /// <param name="range2"></param>
        /// <returns></returns>
        public static DekRangeDouble Super(DekRangeDouble range1, DekRangeDouble range2)
        {
            double min = range1.Start < range2.Start ? range1.Start : range2.Start;
            double max = range1.Stop > range2.Stop ? range1.Stop : range2.Stop;
            return new DekRangeDouble(min, max);
        }


        public override bool Equals(object other) => (Start == ((DekRangeDouble)other).Start && Stop == ((DekRangeDouble)other).Stop);
        public bool Equals(DekRangeDouble other) => (Start == ((DekRangeDouble)other).Start && Stop == ((DekRangeDouble)other).Stop);

        public static bool operator ==(DekRangeDouble range1, DekRangeDouble range2) => range1.Equals(range2);
        public static bool operator !=(DekRangeDouble range1, DekRangeDouble range2) => !range1.Equals(range2);

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

    public static class DekRangeDoubleExtension
    {
        public static List<DekRangeDouble> AddAndMerge(this List<DekRangeDouble> me, DekRangeDouble range)
        {
            return DekRangeDouble.AddAndMerge(me, range);
        }

        /// <summary>
        /// Returns true if any range in list contians the provided range.
        /// </summary>
        public static bool ContainsRange(this List<DekRangeDouble> me, DekRangeDouble range)
        {
            foreach (DekRangeDouble r in me)
                if (r.Contains(range))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true if any range in list contians i.
        /// </summary>
        public static bool ContainsDouble(this List<DekRangeDouble> me, Double i)
        {
            return me.ContainsRange(new DekRangeDouble(i, i));
        }

        //
        public static string ConvertToText(this List<DekRangeDouble> me)
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

        public static void LoadFromText(this List<DekRangeDouble> me, string text)
        {
            me = new List<DekRangeDouble>();
            if (string.IsNullOrWhiteSpace(text))
                return;

            string[] asdjh = text.Split(';');
            foreach(string s in asdjh)
            {
                string[] ns = s.Split(',');
                DekRangeDouble tr = new DekRangeDouble(int.Parse(ns[0]), int.Parse(ns[1]));
                me.Add(tr);
            }
        }
    }
}