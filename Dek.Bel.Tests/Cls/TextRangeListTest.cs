using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dek.Cls
{
    [TestFixture]
    public class DekRangeListTest
    {
        List<DekRange> DisconnectedDekRanges1 => DekRangeListTestHelper.DisconnectedDekRanges1;
        List<DekRange> DisconnectedDekRanges2 => DekRangeListTestHelper.DisconnectedDekRanges2;

        [SetUp]
        public void Setup()
        {
            DekRangeListTestHelper.Setup();
        }

        [Test]
        public void AddAndMerge_New_Inside_Range_Does_Not_Alter_Ranges()
        {
            // Given
            var testRange = new DekRange(1, 4);

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(DisconnectedDekRanges1, newRange);
        }


        [Test]
        public void AddAndMerge_Overlapping_Ranges1()
        {
            // Given
            var testRange = new DekRange(3, 6);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 6),
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges2()
        {
            // Given
            var testRange = new DekRange(9, 11);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(9, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges3()
        {
            // Given
            var testRange = new DekRange(11, 21);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 25),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges4()
        {
            // Given
            var testRange = new DekRange(0, 25);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 25),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges5()
        {
            // Given
            var testRange = new DekRange(24, 26);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 15),
                new DekRange(20, 26),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }


        [Test]
        public void AddAndMerge_Overlapping_Ranges6()
        {
            // Given
            var testRange = new DekRange(4, 21);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 25),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges7()
        {
            // Given
            var testRange = new DekRange(1, 50); // Engulf all of testrange 2
            var expectedRange = new List<DekRange>
            {
                new DekRange(1, 50),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges2.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        #region Ranges touching ---------------------------

        [Test]
        public void AddAndMerge_Touching_Ranges1()
        {
            // Given
            var testRange = new DekRange(6, 7);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(6, 7),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Touching_Ranges2()
        {
            // Given
            var testRange = new DekRange(8, 9);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(8, 9),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Touching_Ranges3()
        {
            // Given
            var testRange = new DekRange(6, 9);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(6, 9),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        #endregion  Regions touching ------------------------

        #region non ovelapping ranges -----------------------

        [Test]
        public void AddAndMerge_Non_Overlapping_Ranges1()
        {
            // Given
            var testRange = new DekRange(0, 5);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 15),
                new DekRange(20, 25),
                new DekRange(30, 35),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges2.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Non_Overlapping_Ranges2()
        {
            // Given
            var testRange = new DekRange(40, 45);
            var expectedRange = new List<DekRange>
            {
                new DekRange(10, 15),
                new DekRange(20, 25),
                new DekRange(30, 35),
                new DekRange(40, 45),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges2.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        #endregion non ovelapping ranges --------------------


        #region Functional purity ---------------------------

        [Test]
        public void AddAndMerge_Is_Pure()
        {
            // Given
            var testRange = new DekRange(40, 45);
            var expectedRange = new List<DekRange>
            {
                new DekRange(10, 15),
                new DekRange(20, 25),
                new DekRange(30, 35),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges2.AddAndMerge(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, DisconnectedDekRanges2);
        }


        #endregion Functional purity ------------------------


    }
}
