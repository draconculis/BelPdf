using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dek.Cls
{
    /// <summary>
    /// This tests Removing some part of the selection
    /// </summary>

    [TestFixture]
    public class DekRangeListSubtractTest
    {
        List<DekRange> DisconnectedDekRanges1 => DekRangeListTestHelper.DisconnectedDekRanges1;
        List<DekRange> DisconnectedDekRanges2 => DekRangeListTestHelper.DisconnectedDekRanges2;

        [SetUp]
        public void Setup()
        {
            DekRangeListTestHelper.Setup();
        }


        [Test]
        public void Subtract_InsideRange()
        {
            // Given
            var subtractor = new DekRange(1, 2);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 0),
                new DekRange(3, 5),
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_StartOfRange1()
        {
            // Given
            var subtractor = new DekRange(0, 1);
            var expectedRange = new List<DekRange>
            {
                new DekRange(2, 5),
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_StartOfRange2()
        {
            // Given
            var subtractor = new DekRange(0, 2);
            var expectedRange = new List<DekRange>
            {
                new DekRange(3, 5),
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_EndOfRange()
        {
            // Given
            var subtractor = new DekRange(5, 5);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 4),
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_EndOfRange2()
        {
            // Given
            var subtractor = new DekRange(4, 5);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 3),
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_EndOfRangePlusSome()
        {
            // Given
            var subtractor = new DekRange(4, 6);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 3),
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_EndOfRangePlusMore()
        {
            // Given
            var subtractor = new DekRange(1, 9);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 0),
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_AllOfRange()
        {
            // Given
            var subtractor = new DekRange(0, 5);
            var expectedRange = new List<DekRange>
            {
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_AllOfRangeAndSome()
        {
            // Given
            var subtractor = new DekRange(0, 5);
            var expectedRange = new List<DekRange>
            {
                new DekRange(10, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_AllOfRangeAndSomeOfNext()
        {
            // Given
            var subtractor = new DekRange(0, 10);
            var expectedRange = new List<DekRange>
            {
                new DekRange(11, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_SomeOfRangeAndSomeOfNext()
        {
            // Given
            var subtractor = new DekRange(5, 10);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 4),
                new DekRange(11, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_SomeOfRangeAndSomeOfNext2()
        {
            // Given
            var subtractor = new DekRange(1, 14);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 0),
                new DekRange(15, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }


        [Test]
        public void Subtract_AllOfRangeAndAllOfNext1()
        {
            // Given
            var subtractor = new DekRange(0, 15);
            var expectedRange = new List<DekRange>
            {
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }


        [Test]
        public void Subtract_SomeOfRangeAndAllOfNext1()
        {
            // Given
            var subtractor = new DekRange(1, 15);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 0),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_SomeOfRangeAndAllOfNextAndMore()
        {
            // Given
            var subtractor = new DekRange(1, 16);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 0),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_AlmostAll()
        {
            // Given
            var subtractor = new DekRange(1, 24);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 0),
                new DekRange(25, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_All()
        {
            // Given
            var subtractor = new DekRange(0, 25);
            var expectedRange = new List<DekRange>
            {
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Last1()
        {
            // Given
            var subtractor = new DekRange(20, 25);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 15)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Last2()
        {
            // Given
            var subtractor = new DekRange(20, 125);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 15)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Last3()
        {
            // Given
            var subtractor = new DekRange(19, 25);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 15)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Last4()
        {
            // Given
            var subtractor = new DekRange(19, 26);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 15)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_MiddleOfMiddle()
        {
            // Given
            var subtractor = new DekRange(11, 14);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 10),
                new DekRange(15, 15),
                new DekRange(20, 25)
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.SubtractRange(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }





        #region Ranges touching ---------------------------

        [Test]
        public void Subtract_Touching_Ranges1()
        {
            // Given
            var testRange = new DekRange(6, 6);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(7, 7),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DekRangeListTestHelper.Touchingrange.SubtractRange(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Touching_Ranges2()
        {
            // Given
            var testRange = new DekRange(6, 7);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DekRangeListTestHelper.Touchingrange.SubtractRange(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Touching_Ranges3()
        {
            // Given
            var testRange = new DekRange(5, 5);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 4),
                new DekRange(6, 7),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DekRangeListTestHelper.Touchingrange.SubtractRange(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Touching_Ranges4()
        {
            // Given
            var testRange = new DekRange(4, 5);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 3),
                new DekRange(6, 7),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DekRangeListTestHelper.Touchingrange.SubtractRange(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }


        [Test]
        public void Subtract_Touching_Ranges5()
        {
            // Given
            var testRange = new DekRange(5, 6);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 4),
                new DekRange(7, 7),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DekRangeListTestHelper.Touchingrange.SubtractRange(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }


        [Test]
        public void Subtract_Touching_Ranges6()
        {
            // Given
            var testRange = new DekRange(5, 7);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 4),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DekRangeListTestHelper.Touchingrange.SubtractRange(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Touching_Ranges7()
        {
            // Given
            var testRange = new DekRange(0, 7);
            var expectedRange = new List<DekRange>
            {
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DekRangeListTestHelper.Touchingrange.SubtractRange(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }

        [Test]
        public void Subtract_Touching_Ranges8()
        {
            // Given
            var testRange = new DekRange(1, 100);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 0),
            };

            // When
            List<DekRange> newRange = DekRangeListTestHelper.Touchingrange.SubtractRange(testRange);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, newRange);
        }


        #endregion  Regions touching ------------------------


        #region Functional purity ---------------------------

        [Test]
        public void Subtract_Is_Pure()
        {
            // Given
            var subtractor = new DekRange(11, 12);
            var expectedRange = new List<DekRange>
            {
                new DekRange(0, 5),
                new DekRange(10, 15),
                new DekRange(20, 25),
            };

            // When
            List<DekRange> newRange = DisconnectedDekRanges1.AddAndMerge(subtractor);

            // Then
            DekRangeListTestHelper.AssertRangesEqual(expectedRange, DisconnectedDekRanges1);
        }


        #endregion Functional purity ------------------------


        //==== HELPERS ============================================================

    }
}
