using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dek.Cls
{
    [TestFixture]
    public class TextRangeListTest
    {
        List<TextRange> DisconnectedTextRanges1;
        List<TextRange> DisconnectedTextRanges2;

        [SetUp]
        public void Setup()
        {
            DisconnectedTextRanges1 = GenerateDisconnectedRange();
            DisconnectedTextRanges2 = GenerateDisconnectedRange2();
        }

        [Test]
        public void AddAndMerge_New_Inside_Range_Does_Not_Alter_Ranges()
        {
            // Given
            var testRange = new TextRange(1, 4);

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(GenerateDisconnectedRange(), newRange);
        }


        [Test]
        public void AddAndMerge_Overlapping_Ranges1()
        {
            // Given
            var testRange = new TextRange(3, 6);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 6),
                new TextRange(10, 15),
                new TextRange(20, 25)
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges2()
        {
            // Given
            var testRange = new TextRange(9, 11);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 5),
                new TextRange(9, 15),
                new TextRange(20, 25)
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges3()
        {
            // Given
            var testRange = new TextRange(11, 21);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 5),
                new TextRange(10, 25),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges4()
        {
            // Given
            var testRange = new TextRange(0, 25);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 25),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges5()
        {
            // Given
            var testRange = new TextRange(24, 26);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 5),
                new TextRange(10, 15),
                new TextRange(20, 26),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }


        [Test]
        public void AddAndMerge_Overlapping_Ranges6()
        {
            // Given
            var testRange = new TextRange(4, 21);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 25),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Overlapping_Ranges7()
        {
            // Given
            var testRange = new TextRange(1, 50); // Engulf all of testrange 2
            var expectedRange = new List<TextRange>
            {
                new TextRange(1, 50),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges2.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        #region Ranges touching ---------------------------

        [Test]
        public void AddAndMerge_Touching_Ranges1()
        {
            // Given
            var testRange = new TextRange(6, 7);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 5),
                new TextRange(6, 7),
                new TextRange(10, 15),
                new TextRange(20, 25),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Touching_Ranges2()
        {
            // Given
            var testRange = new TextRange(8, 9);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 5),
                new TextRange(8, 9),
                new TextRange(10, 15),
                new TextRange(20, 25),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Touching_Ranges3()
        {
            // Given
            var testRange = new TextRange(6, 9);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 5),
                new TextRange(6, 9),
                new TextRange(10, 15),
                new TextRange(20, 25),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges1.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        #endregion  Regions touching ------------------------

        #region non ovelapping ranges -----------------------

        [Test]
        public void AddAndMerge_Non_Overlapping_Ranges1()
        {
            // Given
            var testRange = new TextRange(0, 5);
            var expectedRange = new List<TextRange>
            {
                new TextRange(0, 5),
                new TextRange(10, 15),
                new TextRange(20, 25),
                new TextRange(30, 35),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges2.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void AddAndMerge_Non_Overlapping_Ranges2()
        {
            // Given
            var testRange = new TextRange(40, 45);
            var expectedRange = new List<TextRange>
            {
                new TextRange(10, 15),
                new TextRange(20, 25),
                new TextRange(30, 35),
                new TextRange(40, 45),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges2.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, newRange);
        }

        #endregion non ovelapping ranges --------------------


        #region Functional purity ---------------------------

        [Test]
        public void AddAndMerge_Is_Pure()
        {
            // Given
            var testRange = new TextRange(40, 45);
            var expectedRange = new List<TextRange>
            {
                new TextRange(10, 15),
                new TextRange(20, 25),
                new TextRange(30, 35),
            };

            // When
            List<TextRange> newRange = DisconnectedTextRanges2.AddAndMerge(testRange);

            // Then
            AssertRangesEqual(expectedRange, DisconnectedTextRanges2);
        }


        #endregion Functional purity ------------------------


        //==== HELPERS ============================================================

        List<TextRange> GenerateDisconnectedRange()
        {
            return new[]
            {
                new TextRange(0, 5),
                new TextRange(10, 15),
                new TextRange(20, 25),
            }.ToList();
        }

        List<TextRange> GenerateDisconnectedRange2()
        {
            return new[]
            {
                new TextRange(10, 15),
                new TextRange(20, 25),
                new TextRange(30, 35),
            }.ToList();
        }

        void AssertRangesEqual(List<TextRange> range1, List<TextRange> range2)
        {
            if (range1.Count != range2.Count)
                throw new Exception("Ranges are of different length");

            for(int i = 0; i < range1.Count; i++)
            {
                if(range1[i] != range2[i])
                    throw new Exception($"Range {range1[i]} != {range2[i]}.");
            }
        }
    }
}
