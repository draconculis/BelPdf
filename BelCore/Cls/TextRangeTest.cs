using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dek.Cls
{
    [TestFixture]
    public class TextRangeTest
    {
        TextRange sutRange;

        List<TextRange> TextRanges = new[]
            {
                new TextRange(0, 0),
                new TextRange(0, 1),
                new TextRange(0, 2),
                new TextRange(0, 3),
                new TextRange(0, 4),
                new TextRange(0, 400),
                new TextRange(1, 1),
                new TextRange(1, 2),
                new TextRange(1, 5),
                new TextRange(2, 2),
                new TextRange(3, 3),
                new TextRange(100, 101),
                new TextRange(100, 1000),
                new TextRange(1, 0),
                new TextRange(2, 0),
                new TextRange(3, 0),
                new TextRange(4, 0),
                new TextRange(400, 0),
                new TextRange(2, 1),
                new TextRange(3, 2),
                new TextRange(10, 0),
                new TextRange(10, 2),
                new TextRange(10, 9),
                new TextRange(101, 100),
                new TextRange(100, 1000),

            }.ToList();

        [SetUp]
        public void Setup()
        {
            sutRange = new TextRange(0, 4);
        }

        #region Contains ----------------

        [Test]
        public void ContainsItself_ReturnsTrue()
        {
            foreach (var textRange in TextRanges)
            {
                Assert.That(textRange.Contains(textRange), Is.True);
            }
        }

        [Test]
        public void BiggerContainsSmaller_And_NotTheOpposite()
        {
            foreach (var textRange in TextRanges)
            {
                var biggerTextRange = new TextRange(textRange.Start, textRange.Stop + 1);
                Assert.That(biggerTextRange.Contains(textRange), Is.True);
                Assert.That(textRange.Contains(biggerTextRange), Is.False);

                // reverse
                var biggerReverseTextRange = new TextRange(textRange.Stop + 1, textRange.Start);
                Assert.That(biggerReverseTextRange.Contains(textRange), Is.True);
                Assert.That(textRange.Contains(biggerReverseTextRange), Is.False);
            }
        }

        [Test]
        public void Range_14_Contians_11_and_44()
        {
            var testRange = new TextRange(1, 4);
            var otherRange1 = new TextRange(1, 1);
            var otherRange2 = new TextRange(4, 4);

            // Assert
            Assert.That(testRange.Contains(otherRange1), Is.True);
            Assert.That(testRange.Contains(otherRange2), Is.True);
        }

        [Test]
        public void Range_14_DoesNotContians_01_nor_45()
        {
            var testRange = new TextRange(1, 4);
            var otherRange1 = new TextRange(0, 1);
            var otherRange2 = new TextRange(4, 5);

            // Assert
            Assert.That(testRange.Contains(otherRange1), Is.False);
            Assert.That(testRange.Contains(otherRange2), Is.False);
        }

        #endregion



        [Test]
        public void Range_14_Intersect_11_and_44_and_vice_versa()
        {
            var testRange = new TextRange(1, 4);
            var otherRange1 = new TextRange(1, 1);
            var otherRange2 = new TextRange(4, 4);

            // Assert
            Assert.That(testRange.Intersects(otherRange1), Is.True);
            Assert.That(testRange.Intersects(otherRange2), Is.True);
            Assert.That(otherRange1.Intersects(testRange), Is.True);
            Assert.That(otherRange2.Intersects(testRange), Is.True);
        }

        public void Range_14_Intersect_01_and_45_and_vice_versa()
        {
            var testRange = new TextRange(1, 4);
            var otherRange1 = new TextRange(0, 1);
            var otherRange2 = new TextRange(4, 5);

            // Assert
            Assert.That(testRange.Intersects(otherRange1), Is.True);
            Assert.That(testRange.Intersects(otherRange2), Is.True);
            Assert.That(otherRange1.Intersects(testRange), Is.True);
            Assert.That(otherRange2.Intersects(testRange), Is.True);
        }

        [Test]
        public void Range_00_or_55_DoesNotIntersect_14_vice_versa()
        {
            var testRange = new TextRange(1, 4);
            var otherRange2 = new TextRange(0, 0);
            var otherRange1 = new TextRange(5, 5);

            // Assert
            Assert.That(testRange.Intersects(otherRange1), Is.False);
            Assert.That(otherRange1.Intersects(testRange), Is.False);
            Assert.That(testRange.Intersects(otherRange2), Is.False);
            Assert.That(otherRange2.Intersects(testRange), Is.False);
        }

        [Test]
        public void Range_14_DoesNotOverlap_59_vice_versa()
        {
            var testRange = new TextRange(1, 4);
            var otherRange = new TextRange(5, 9);

            // Assert
            Assert.That(testRange.Intersects(otherRange), Is.False);
            Assert.That(otherRange.Intersects(testRange), Is.False);
        }


        [Test]
        public void Range_19_Overlaps_46_vice_versa()
        {
            var testRange = new TextRange(1, 9);
            var otherRange = new TextRange(4, 6);

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
            Assert.Throws<ArgumentException>(() => new TextRange(-1, 1));
            Assert.Throws<ArgumentException>(() => new TextRange(1, -1));
            Assert.Throws<ArgumentException>(() => new TextRange(-1, -1));
            Assert.Throws<ArgumentException>(() => new TextRange(-1, 0));
            Assert.Throws<ArgumentException>(() => new TextRange(0, -1));
        }

        [Test]
        public void DoesNotThrow_ForValidRanges()
        {
            Assert.DoesNotThrow(() => new TextRange(0, 0));
            Assert.DoesNotThrow(() => new TextRange(0, 1));
            Assert.DoesNotThrow(() => new TextRange(1, 0));
            Assert.DoesNotThrow(() => new TextRange(1, 1));
            Assert.DoesNotThrow(() => new TextRange(1000000, 0));
            Assert.DoesNotThrow(() => new TextRange(0, 1000000));
            Assert.DoesNotThrow(() => new TextRange(1000000, 1000000));
            Assert.DoesNotThrow(() => new TextRange(100000000, 1000000));
            Assert.DoesNotThrow(() => new TextRange(1000000, 100000000));
        }

        #endregion ----------------------

        [Test]
        public void ContainsInt_ReturnsTrue_IfInsideRange()
        {
            // Arrange
            var textRange = new TextRange(5, 10);

            // Assert & Act

            // Values before range
            foreach (int i in new int[] { 0, 1, 2, 3, 4 })
            {
                Assert.That(textRange.Contains(i), Is.False);
            }

            // Inside range
            foreach (int i in new int[] { 5, 6, 7, 8, 9, 10 })
            {
                Assert.That(textRange.Contains(i), Is.True);
            }

            // After range
            foreach (int i in new int[] { 11, 12, 13, 100000 })
            {
                Assert.That(textRange.Contains(i), Is.False);
            }

            // Special case
            Assert.DoesNotThrow(() => textRange.Contains(-1));
        }

        #region Subtract Left edge ----------------
        [Test]
        public void Subtract_00_from_04_Returns_14()
        {
            // Arrange
            var textRange = new TextRange(0, 4);
            var subtractor = new TextRange(0, 0);
            var expected = new TextRange(1, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_00_from_14_Returns_14()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(0, 0);
            var expected = new TextRange(1, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_01_from_14_Returns_24()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(0, 1);
            var expected = new TextRange(2, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_02_from_14_Returns_34()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(0, 2);
            var expected = new TextRange(3, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_12_from_14_Returns_34()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(1, 2);
            var expected = new TextRange(3, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_03_from_14_Returns_44()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(0, 3);
            var expected = new TextRange(4, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_04_from_14_Returns_EmptySet()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(0, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Subtract_14_from_14_Returns_EmptySet()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(1, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Subtract_05_from_14_Returns_EmptySet()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(0, 5);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }


        [Test]
        public void Subtract_15_from_14_Returns_EmptySet()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(1, 5);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        #endregion ----------------------

        #region Subtract right case

        [Test]
        public void Subtract_55_from_14_Returns_14()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(5, 5);
            var expected = new TextRange(1, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_45_from_14_Returns_13()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(4, 5);
            var expected = new TextRange(1, 3);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_44_from_14_Returns_13()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(4, 4);
            var expected = new TextRange(1, 3);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_34_from_14_Returns_12()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(3, 4);
            var expected = new TextRange(1, 2);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_35_from_14_Returns_12()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(3, 5);
            var expected = new TextRange(1, 2);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_25_from_14_Returns_11()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(2, 5);
            var expected = new TextRange(1, 1);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Subtract_24_from_14_Returns_11()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(2, 4);
            var expected = new TextRange(1, 1);

            // Act
            var result = textRange.Subtract(subtractor);

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
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(2, 3);
            var expected1 = new TextRange(1, 1);
            var expected2 = new TextRange(4, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }

        [Test]
        public void Subtract_22_from_14_Returns_11_and_34()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(2, 2);
            var expected1 = new TextRange(1, 1);
            var expected2 = new TextRange(3, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }

        [Test]
        public void Subtract_33_from_14_Returns_12_and_44()
        {
            // Arrange
            var textRange = new TextRange(1, 4);
            var subtractor = new TextRange(3, 3);
            var expected1 = new TextRange(1, 2);
            var expected2 = new TextRange(4, 4);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }

        [Test]
        public void Subtract_22_from_13_Returns_11_and_33()
        {
            // Arrange
            var textRange = new TextRange(1, 3);
            var subtractor = new TextRange(2, 2);
            var expected1 = new TextRange(1, 1);
            var expected2 = new TextRange(3, 3);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }


        [Test]
        public void Subtract_12_from_03_Returns_00_and_33()
        {
            // Arrange
            var textRange = new TextRange(0, 3);
            var subtractor = new TextRange(1, 2);
            var expected1 = new TextRange(0, 0);
            var expected2 = new TextRange(3, 3);

            // Act
            var result = textRange.Subtract(subtractor);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(expected1));
            Assert.That(result[1], Is.EqualTo(expected2));
        }


        #endregion

    }
}
