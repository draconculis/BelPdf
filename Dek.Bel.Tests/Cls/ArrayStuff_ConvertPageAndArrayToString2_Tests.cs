using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Cls
{
    public class ArrayStuff_ConvertPageAndArrayToString2_Tests
    {
        List<(int page, int[] rects)> pageRects1 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11 }),
        };
        string pageRectString1 = "#1!100,100,11,11;";

        List<(int page, int[] rects)> pageRects2 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11, 200, 200, 22, 22 }),
        };
        string pageRectString2 = "#1!100,100,11,11;200,200,22,22;";

        List<(int page, int[] rects)> pageRects3 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11, 200, 200, 22, 22, 300, 300, 33, 33 }),
        };
        string pageRectString3 = "#1!100,100,11,11;200,200,22,22;300,300,33,33;";

        // Out of whack
        List<(int page, int[] rects)> pageRects2_1 = new List<(int page, int[] rects)>
        {
            (1, new []{ 200, 200, 22, 22, 100, 100, 11, 11 }),
        };
        //string pageRectString2_1 = "#1!200,200,22,22;#2!100,100,11,11;";

        List<(int page, int[] rects)> pageRects2_2 = new List<(int page, int[] rects)>
        {
            (1, new []{ 100, 100, 11, 11, 300, 300, 33, 33, 200, 200, 22, 22 }),
        };
        //string pageRectString2_2 = "#1!100,100,11,11;300,300,33,33;#2!200,200,22,22;";


        [Test]
        public void SplitArrayIntoPageRects()
        {
            // Given
            // Lists above

            // When
            string res1 = ArrayStuff.ConvertPageAndArrayToString(pageRects1);
            string res2 = ArrayStuff.ConvertPageAndArrayToString(pageRects2);
            string res3 = ArrayStuff.ConvertPageAndArrayToString(pageRects3);
            //string res2_1 = ArrayStuff.ConvertPageAndArrayToString(pageRects2_1);
            //string res2_2 = ArrayStuff.ConvertPageAndArrayToString(pageRects2_2);

            // Then
            Assert.That(res1, Is.EqualTo(pageRectString1));
            Assert.That(res2, Is.EqualTo(pageRectString2));
            Assert.That(res3, Is.EqualTo(pageRectString3));
            //Assert.That(res2_1, Is.EqualTo(pageRectString2_1));
            //Assert.That(res2_2, Is.EqualTo(pageRectString2_2));
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
