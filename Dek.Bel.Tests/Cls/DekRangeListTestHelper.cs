using Dek.Cls;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dek.Cls
{
    class DekRangeListTestHelper
    {
        public static List<DekRange> DisconnectedDekRanges1;
        public static List<DekRange> DisconnectedDekRanges2;
        public static List<DekRange> Touchingrange;

        public static void Setup()
        {
            DisconnectedDekRanges1 = GenerateDisconnectedRange1();
            DisconnectedDekRanges2 = GenerateDisconnectedRange2();
            Touchingrange = GenerateTouchingRange();
        }

        static List<DekRange> GenerateDisconnectedRange1()
        {
            return new[]
            {
                new DekRange(0, 5),
                new DekRange(10, 15),
                new DekRange(20, 25),
            }.ToList();
        }

        static List<DekRange> GenerateDisconnectedRange2()
        {
            return new[]
            {
                new DekRange(10, 15),
                new DekRange(20, 25),
                new DekRange(30, 35),
            }.ToList();
        }

        static List<DekRange> GenerateTouchingRange()
        {
            return new[]
            {
                new DekRange(0, 5),
                new DekRange(6, 7),
                new DekRange(10, 15),
                new DekRange(20, 25),
            }.ToList();
        }

        public static void AssertRangesEqual(List<DekRange> range1, List<DekRange> range2)
        {
            Assert.That(range1, Has.Count.EqualTo(range2.Count));

            for (int i = 0; i < range1.Count; i++)
            {
                if (range1[i] != range2[i])
                    throw new Exception($"Range {range1[i]} != {range2[i]}.");
            }
        }


    }
}
