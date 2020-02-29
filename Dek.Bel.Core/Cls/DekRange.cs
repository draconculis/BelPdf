using System;
using System.Collections.Generic;
using System.Linq;

namespace Dek.Cls
{
    public class DekRange : IComparable, IEquatable<DekRange>
    {
        public int Start { get; }
        public int Stop { get; }

        public DekRange(int start, int stop, bool stopAsLength = false)
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

        public bool Intersects(DekRange range) =>
            (range.Start >= Start && range.Start <= Stop)
            || (range.Stop >= Start && range.Stop <= Stop)
            || (range.Start <= Start) && (range.Stop >= Stop)
            || (range.Start >= Start) && (range.Stop <= Stop);


        public bool Contains(DekRange range) => (range.Start >= Start && range.Stop <= Stop);

        public bool Contains(int pos) => (pos >= Start && pos <= Stop);

        /// <summary>
        /// Orders by Start
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is DekRange range)
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
        public List<DekRange> Subtract(DekRange excludeRange)
        {
            // Nothing excluded
            if (!Intersects(excludeRange))
                return new List<DekRange> { this };

            // Delete all
            if (excludeRange.Contains(this))
                return new List<DekRange>();

            // Handle left edge case
            if (excludeRange.Start <= Start)
            {
                // All excluded
                if (excludeRange.Stop >= Stop)
                    return new List<DekRange>();

                return new List<DekRange> { new DekRange(excludeRange.Stop + 1, Stop) };
            }

            // We know that excluded.Start is in range
            if (excludeRange.Stop >= Stop)
            {
                return new List<DekRange> { new DekRange(Start, excludeRange.Start - 1) };
            }

            // We now come to the cases where we get 2 return values
            // exclude range is within (Start + 1, Stop -1)
            var resultSet = new List<DekRange>();
            resultSet.Add(new DekRange(Start, excludeRange.Start - 1));
            resultSet.Add(new DekRange(excludeRange.Stop + 1, Stop));

            return resultSet;
        }


        /// <summary>
        /// Given this as the master range, apply a set of ranges within this range and 
        /// subtract these. Produce a set of new ranges.
        /// </summary>
        /// <param name="excludeRanges"></param>
        /// <returns></returns>
        public List<DekRange> SubtractSubRanges(List<DekRange> excludeRanges)
        {
            // Assert that no range is outside of this range
            foreach (var range in excludeRanges)
            {
                if (!Contains(range))
                    throw new ArgumentException("Sub range cannot be outside master range.");
            }

            // Assert that none of the ranges overlap



            return new List<DekRange>();
        }

        public static List<DekRange> MergeConnectedRanges(List<DekRange> ranges)
        {
            if (ranges.Count == 0)
                return new List<DekRange> { };

            if (ranges.Count == 1)
                return new List<DekRange> { ranges[0] };

            var super = new List<DekRange>();
            List<DekRange> sortedRanges = ranges.OrderBy(x => x).ToList();

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

        public static List<DekRange> AddAndMerge(List<DekRange> existingRange, DekRange range)
        {
            var newRange = Add(existingRange, range);
            return MergeConnectedRanges(newRange);
        }

        public static List<DekRange> Add(List<DekRange> existingRange, DekRange range)
        {
            List<DekRange> textRanges = new List<DekRange>(existingRange);
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
        public static DekRange Super(DekRange range1, DekRange range2)
        {
            int min = range1.Start < range2.Start ? range1.Start : range2.Start;
            int max = range1.Stop > range2.Stop ? range1.Stop : range2.Stop;
            return new DekRange(min, max);
        }


        public override bool Equals(object other) => (Start == ((DekRange)other).Start && Stop == ((DekRange)other).Stop);
        public bool Equals(DekRange other) => (Start == ((DekRange)other).Start && Stop == ((DekRange)other).Stop);

        public static bool operator ==(DekRange range1, DekRange range2) => range1.Equals(range2);
        public static bool operator !=(DekRange range1, DekRange range2) => !range1.Equals(range2);

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
}