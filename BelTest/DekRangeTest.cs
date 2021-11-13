using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dek.Cls
{
    [TestFixture]
    public class DekRangeTest
    {
        DekRange sutRange;

        List<DekRange> DekRanges = new[]
            {
                new DekRange(0, 0),
                new DekRange(0, 1),
                new DekRange(0, 2),
                new DekRange(0, 3),
                new DekRange(0, 4),
                new DekRange(0, 400),
                new DekRange(1, 1),
                new DekRange(1, 2),
                new DekRange(1, 5),
                new DekRange(2, 2),
                new DekRange(3, 3),
                new DekRange(100, 101),
                new DekRange(100, 1000),
                new DekRange(1, 0),
                new DekRange(2, 0),
                new DekRange(3, 0),
                new DekRange(4, 0),
                new DekRange(400, 0),
                new DekRange(2, 1),
                new DekRange(3, 2),
                new DekRange(10, 0),
                new DekRange(10, 2),
                new DekRange(10, 9),
                new DekRange(101, 100),
                new DekRange(100, 1000),

            }.ToList();

        [SetUp]
        public void Setup()
        {
            sutRange = new DekRange(0, 4);
        }

        #region Equals --------------------------

        [Test]
        public void EqualityTests()
        {
            var equals = new List<bool>();

            // Given
            var rangeA = new DekRange(1, 4);
            var rangeB = new DekRange(1, 4); // equal range
            var range1 = new DekRange(1, 3); // non equal range
            var range2 = new DekRange(2, 4); // non equal range
            var range3 = new DekRange(2, 3); // non equal range

            // When
#pragma warning disable CS1718 // Comparison made to same variable

            equals.Add(rangeA == rangeA);
            equals.Add(rangeB == rangeB);
            equals.Add(rangeB == rangeA);
            equals.Add(rangeA == rangeB);

            equals.Add(!(rangeA != rangeA));
            equals.Add(!(rangeB != rangeB));
            equals.Add(!(rangeB != rangeA));
            equals.Add(!(rangeA != rangeB));

            equals.Add(!(rangeA == range1));
            equals.Add(!(rangeA == range2));
            equals.Add(!(rangeA == range3));

            equals.Add(rangeA != range1);
            equals.Add(rangeA != range2);
            equals.Add(rangeA != range3);

#pragma warning restore CS1718 // Comparison made to same variable


            // Then
            foreach(bool equal in equals)
                Assert.That(equal,  Is.True);
            
        }

        #endregion Equals -----------------------

        #region Contains ----------------

        [Test]
        public void ContainsItself_ReturnsTrue()
        {
            foreach (var DekRange in DekRanges)
            {
                Assert.That(DekRange.Contains(DekRange), Is.True);
            }
        }

        [Test]
        public void BiggerContainsSmaller_And_NotTheOpposite()
        {
            foreach (var DekRange in DekRanges)
            {
                var biggerDekRange = new DekRange(DekRange.Start, DekRange.Stop + 1);
                Assert.That(biggerDekRange.Contains(DekRange), Is.True);
                Assert.That(DekRange.Contains(biggerDekRange), Is.False);

                // reverse
                var biggerReverseDekRange = new DekRange(DekRange.Stop + 1, DekRange.Start);
                Assert.That(biggerReverseDekRange.Contains(DekRange), Is.True);
                Assert.That(DekRange.Contains(biggerReverseDekRange), Is.False);
            }
        }

        [Test]
        public void Range_14_Contians_11_and_44()
        {
            var testRange = new DekRange(1, 4);
            var otherRange1 = new DekRange(1, 1);
            var otherRange2 = new DekRange(4, 4);

            // Assert
            Assert.That(testRange.Contains(otherRange1), Is.True);
            Assert.That(testRange.Contains(otherRange2), Is.True);
        }

        [Test]
        public void Range_14_DoesNotContians_01_nor_45()
        {
            var testRange = new DekRange(1, 4);
            var otherRange1 = new DekRange(0, 1);
            var otherRange2 = new DekRange(4, 5);

            // Assert
            Assert.That(testRange.Contains(otherRange1), Is.False);
            Assert.That(testRange.Contains(otherRange2), Is.False);
        }

        #endregion

        [Test]
        public void Range_14_Intersect_11_and_44_and_vice_versa()
        {
            var testRange = new DekRange(1, 4);
            var otherRange1 = new DekRange(1, 1);
            var otherRange2 = new DekRange(4, 4);

            // Assert
            Assert.That(testRange.Intersects(otherRange1), Is.True);
            Assert.That(testRange.Intersects(otherRange2), Is.True);
            Assert.That(otherRange1.Intersects(testRange), Is.True);
            Assert.That(otherRange2.Intersects(testRange), Is.True);
        }

        public void Range_14_Intersect_01_and_45_and_vice_versa()
        {
            var testRange = new DekRange(1, 4);
            var otherRange1 = new DekRange(0, 1);
            var otherRange2 = new DekRange(4, 5);

            // Assert
            Assert.That(testRange.Intersects(otherRange1), Is.True);
            Assert.That(testRange.Intersects(otherRange2), Is.True);
            Assert.That(otherRange1.Intersects(testRange), Is.True);
            Assert.That(otherRange2.Intersects(testRange), Is.True);
        }

        [Test]
        public void Range_00_or_55_DoesNotIntersect_14_vice_versa()
        {
            var testRange = new DekRange(1, 4);
            var otherRange2 = new DekRange(0, 0);
            var otherRange1 = new DekRange(5, 5);

            // Assert
            Assert.That(testRange.Intersects(otherRange1), Is.False);
            Assert.That(otherRange1.Intersects(testRange), Is.False);
            Assert.That(testRange.Intersects(otherRange2), Is.False);
            Assert.That(otherRange2.Intersects(testRange), Is.False);
        }

        [Test]
        public void Range_14_DoesNotOverlap_59_vice_versa()
        {
            var testRange = new DekRange(1, 4);
            var otherRange = new DekRange(5, 9);

            // Assert
            Assert.That(testRange.Intersects(otherRange), Is.False);
            Assert.That(otherRange.Intersects(testRange), Is.False);
        }


        [Test]
        public void Range_19_Overlaps_46_vice_versa()
        {
            var testRange = new DekRange(1, 9);
            var otherRange = new DekRange(4, 6);

            // Assert
            Assert.That(testRange.Intersects(otherRange), Is.True);
            Assert.That(otherRange.Intersects(testRange), Is.True);
        }


        #region Overlaps ----------------



        #endregion ----------------------

        #region ctor ArgumentException --
        [Test]
        public void ThrowsArgExceptionForNegativeStartOrStop()
        {
            Assert.Throws<ArgumentException>(() => new DekRange(-1, 1));
            Assert.Throws<ArgumentException>(() => new DekRange(1, -1));
            Assert.Throws<ArgumentException>(() => new DekRange(-1, -1));
            Assert.Throws<ArgumentException>(() => new DekRange(-1, 0));
            Assert.Throws<ArgumentException>(() => new DekRange(0, -1));
        }

        [Test]
        public void DoesNotThrow_ForValidRanges()
        {
            Assert.DoesNotThrow(() => new DekRange(0, 0));
            Assert.DoesNotThrow(() => new DekRange(0, 1));
            Assert.DoesNotThrow(() => new DekRange(1, 0));
            Assert.DoesNotThrow(() => new DekRange(1, 1));
            Assert.DoesNotThrow(() => new DekRange(1000000, 0));
            Assert.DoesNotThrow(() => new DekRange(0, 1000000));
            Assert.DoesNotThrow(() => new DekRange(1000000, 1000000));
            Assert.DoesNotThrow(() => new DekRange(100000000, 1000000));
            Assert.DoesNotThrow(() => new DekRange(1000000, 100000000));
        }

        #endregion ----------------------

        [Test]
        public void ContainsInt_ReturnsTrue_IfInsideRange()
        {
            // Arrange
            var DekRange = new DekRange(5, 10);

            // Assert & Act

            // Values before range
            foreach (int i in new int[] { 0, 1, 2, 3, 4 })
            {
                Assert.That(DekRange.Contains(i), Is.False);
            }

            // Inside range
            foreach (int i in new int[] { 5, 6, 7, 8, 9, 10 })
            {
                Assert.That(DekRange.Contains(i), Is.True);
            }

            // After range
            foreach (int i in new int[] { 11, 12, 13, 100000 })
            {
                Assert.That(DekRange.Contains(i), Is.False);
            }

            // Special case
            Assert.DoesNotThrow(() => DekRange.Contains(-1));
        }

        #region Subtract Left edge ----------------
        [Test]
        public void Subtract_00_from_04_Returns_14()
        {
            // Arrange
            var DekRange = new DekRange(0, 4);
            var subtractor = new DekRange(0, 0);
            var expected = new DekRange(1, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_00_from_14_Returns_14()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(0, 0);
            var expected = new DekRange(1, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_01_from_14_Returns_24()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(0, 1);
            var expected = new DekRange(2, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_02_from_14_Returns_34()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(0, 2);
            var expected = new DekRange(3, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_12_from_14_Returns_34()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(1, 2);
            var expected = new DekRange(3, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_03_from_14_Returns_44()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(0, 3);
            var expected = new DekRange(4, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_04_from_14_Returns_EmptySet()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(0, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Subtract_14_from_14_Returns_EmptySet()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(1, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Subtract_05_from_14_Returns_EmptySet()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(0, 5);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }


        [Test]
        public void Subtract_15_from_14_Returns_EmptySet()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(1, 5);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        #endregion ----------------------

        #region Subtract right case

        [Test]
        public void Subtract_55_from_14_Returns_14()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(5, 5);
            var expected = new DekRange(1, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_45_from_14_Returns_13()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(4, 5);
            var expected = new DekRange(1, 3);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_44_from_14_Returns_13()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(4, 4);
            var expected = new DekRange(1, 3);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_34_from_14_Returns_12()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(3, 4);
            var expected = new DekRange(1, 2);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_35_from_14_Returns_12()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(3, 5);
            var expected = new DekRange(1, 2);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_25_from_14_Returns_11()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(2, 5);
            var expected = new DekRange(1, 1);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_24_from_14_Returns_11()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(2, 4);
            var expected = new DekRange(1, 1);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }


        #endregion

        #region Subtract - middle cases -

        [Test]
        public void Subtract_23_from_14_Returns_11_and_44()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(2, 3);
            var expected1 = new DekRange(1, 1);
            var expected2 = new DekRange(4, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }

        [Test]
        public void Subtract_22_from_14_Returns_11_and_34()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(2, 2);
            var expected1 = new DekRange(1, 1);
            var expected2 = new DekRange(3, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }

        [Test]
        public void Subtract_33_from_14_Returns_12_and_44()
        {
            // Arrange
            var DekRange = new DekRange(1, 4);
            var subtractor = new DekRange(3, 3);
            var expected1 = new DekRange(1, 2);
            var expected2 = new DekRange(4, 4);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }

        [Test]
        public void Subtract_22_from_13_Returns_11_and_33()
        {
            // Arrange
            var DekRange = new DekRange(1, 3);
            var subtractor = new DekRange(2, 2);
            var expected1 = new DekRange(1, 1);
            var expected2 = new DekRange(3, 3);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }


        [Test]
        public void Subtract_12_from_03_Returns_00_and_33()
        {
            // Arrange
            var DekRange = new DekRange(0, 3);
            var subtractor = new DekRange(1, 2);
            var expected1 = new DekRange(0, 0);
            var expected2 = new DekRange(3, 3);

            // Act
            var result = DekRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }


        #endregion

    }
}
