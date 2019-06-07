using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Cls
{
    public class ArrayStuff_SplitArrayIntoPageAndrects_Tests
    {
        // In order = generate one page
        List<(int page, int[] rects)> pageRects1 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11 }),
        };

        List<(int page, int[] rects)> pageRects2 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11, 200, 200, 22, 22 }),
        };

        List<(int page, int[] rects)> pageRects3 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11, 200, 200, 22, 22, 300, 300, 33, 33 }),
        };

        // Out of whack = generate two pages
        List<(int page, int[] rects)> pageRects2_1 = new List<(int page, int[] rects)>
        {
            (1, new []{ 200, 200, 22, 22, 100, 100, 11, 11 }),
        };
        List<(int page, int[] rects)> expectedPageRects2_1 = new List<(int page, int[] rects)>
        {
            (1, new []{ 200, 200, 22, 22 }),
            (2, new []{ 100, 100, 11, 11 }),
        };

        List<(int page, int[] rects)> pageRects2_2 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11, 300, 300, 33, 33, 200, 200, 22, 22 }),
        };
        List<(int page, int[] rects)> expectedPageRects2_2 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11, 300, 300, 33, 33 }),
            (2, new []{ 200, 200, 22, 22 }),
        };

        List<(int page, int[] rects)> pageRects2_3 = new List<(int page, int[] rects)>
        {
            (1, new []{ 200, 200, 22, 22, 100, 100, 11, 11, 200, 200, 22, 22 }),
        };
        List<(int page, int[] rects)> expectedPageRects2_3 = new List<(int page, int[] rects)>
        {
            (1, new []{ 200, 200, 22, 22 }),
            (2, new []{ 100, 100, 11, 11, 200, 200, 22, 22 }),
        };



        [Test]
        public void SplitRectsPerPage()
        {
            // Given
            // Lists above

            // When
            List<(int page, int[] rects)> res1 = ArrayStuff.SplitRectsPerPage(1, pageRects1[0].rects);
            List<(int page, int[] rects)> res2 = ArrayStuff.SplitRectsPerPage(1, pageRects2[0].rects);
            List<(int page, int[] rects)> res3 = ArrayStuff.SplitRectsPerPage(1, pageRects3[0].rects);
            List<(int page, int[] rects)> res2_1 = ArrayStuff.SplitRectsPerPage(1, pageRects2_1[0].rects);
            List<(int page, int[] rects)> res2_2 = ArrayStuff.SplitRectsPerPage(1, pageRects2_2[0].rects);
            List<(int page, int[] rects)> res2_3 = ArrayStuff.SplitRectsPerPage(1, pageRects2_3[0].rects);

            // Then
            AssertPageRectArray(res1, pageRects1);
            AssertPageRectArray(res2, pageRects2);
            AssertPageRectArray(res3, pageRects3);
            AssertPageRectArray(res2_1, expectedPageRects2_1);
            AssertPageRectArray(res2_2, expectedPageRects2_2);
            AssertPageRectArray(res2_3, expectedPageRects2_3);
        }


        private void AssertPageRectArray(List<(int page, int[] rects)> sut, List<(int page, int[] rects)> expected)
        {
            Assert.That(sut, Has.Count.EqualTo(expected.Count));

            for(int i = 0; i < sut.Count; i++)
            {
                var sutPageRect = sut[i];
                var expectedPageRect = expected[i];

                Assert.That(sutPageRect.page, Is.EqualTo(expectedPageRect.page));
                for (int j = 0; j < sut.Count; j++)
                {
                    Assert.That(sutPageRect.rects[j], Is.EqualTo(expectedPageRect.rects[j]));
                }

            }
        }

    }
}
