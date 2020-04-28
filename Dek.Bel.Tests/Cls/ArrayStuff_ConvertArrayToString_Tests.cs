using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Bel.Core.Cls;

namespace Dek.Bel.Cls
{
    public class ArrayStuff_ConvertArrayToString_Tests
    {
        [Test]
        public void ConvertArrayToString_OneRect_Valid()
        {
            // Given
            int[] rects = new[] { 1, 2, 3, 4 };

            // When
            string res = ArrayStuff.ConvertArrayToString(rects);

            // Then
            Assert.That(res, Is.EqualTo("1,2,3,4;"));

        }

        [Test]
        public void ConvertArrayToString_TwoRects_Valid()
        {
            // Given
            int[] rects = new[] { 1, 2, 3, 4, 11, 22, 33, 44 };

            // When
            string res = ArrayStuff.ConvertArrayToString(rects);

            // Then
            Assert.That(res, Is.EqualTo("1,2,3,4;11,22,33,44;"));

        }

        [Test]
        public void ConvertArrayToString_FourRects_Valid()
        {
            // Given
            int[] rects = new[] { 1, 2, 3, 4, 11, 22, 33, 44, 111, 222, 333, 444, 1111, 2222, 3333, 4444 };

            // When
            string res = ArrayStuff.ConvertArrayToString(rects);

            // Then
            Assert.That(res, Is.EqualTo("1,2,3,4;11,22,33,44;111,222,333,444;1111,2222,3333,4444;"));

        }

        [Test]
        public void ConvertArrayToString_NoRects_Valid()
        {
            // Given
            int[] rects = new int[0];

            // When
            string res = ArrayStuff.ConvertArrayToString(rects);

            // Then
            Assert.That(res, Is.EqualTo(""));

        }
    }
}
