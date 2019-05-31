using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Cls
{
    public class ArrayStuff_ConvertPageAndArrayToString_Tests
    {
        List<(int page, int[] rects)> pageRects1 = new List<(int page, int[] rects)>
        {
            (1, new []{ 1, 2, 3, 4 }),
        };
        List<(int page, int[] rects)> pageRects2 = new List<(int page, int[] rects)>
        {
            (1, new []{ 1, 2, 3, 4 }),
            (2, new []{ 11, 22, 33, 44 }),
        };
        List<(int page, int[] rects)> pageRects3 = new List<(int page, int[] rects)>
        {
            (1, new []{ 1, 2, 3, 4 }),
            (2, new []{ 11, 22, 33, 44 }),
            (3, new []{ 111, 222, 333, 444 }),
        };
        List<(int page, int[] rects)> pageRects4 = new List<(int page, int[] rects)>
        {
            (1, new []{ 1, 2, 3, 4 }),
            (2, new []{ 11, 22, 33, 44 }),
            (3, new []{ 111, 222, 333, 444 }),
            (4, new []{ 1111, 2222, 3333, 4444 }),
        };
        string pageRectString1 = "#1!1,2,3,4;";
        string pageRectString2 = "#1!1,2,3,4;#2!11,22,33,44;";
        string pageRectString3 = "#1!1,2,3,4;#2!11,22,33,44;#3!111,222,333,444;";
        string pageRectString4 = "#1!1,2,3,4;#2!11,22,33,44;#3!111,222,333,444;#4!1111,2222,3333,4444;";

        [Test]
        public void ConvertPageAndArrayToString_Rects_Valid()
        {
            // Given
            // Lists above

            // When
            string res1 = ArrayStuff.ConvertPageAndArrayToString(pageRects1);
            string res2 = ArrayStuff.ConvertPageAndArrayToString(pageRects2);
            string res3 = ArrayStuff.ConvertPageAndArrayToString(pageRects3);
            string res4 = ArrayStuff.ConvertPageAndArrayToString(pageRects4);

            // Then
            Assert.That(res1, Is.EqualTo(pageRectString1));
            Assert.That(res2, Is.EqualTo(pageRectString2));
            Assert.That(res3, Is.EqualTo(pageRectString3));
            Assert.That(res4, Is.EqualTo(pageRectString4));

        }

        [Test]
        public void ConvertPageAndArrayToString_TwoRects_Valid()
        {
            // Given
            // Strings above

            // When
            var res1 = ArrayStuff.ConvertStringToPagesAndArrays(pageRectString1);
            var res2 = ArrayStuff.ConvertStringToPagesAndArrays(pageRectString2);
            var res3 = ArrayStuff.ConvertStringToPagesAndArrays(pageRectString3);
            var res4 = ArrayStuff.ConvertStringToPagesAndArrays(pageRectString4);

            // Then
            AssertPageRectArray(res1, pageRects1);
            AssertPageRectArray(res2, pageRects2);
            AssertPageRectArray(res3, pageRects3);
            AssertPageRectArray(res4, pageRects4);

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
