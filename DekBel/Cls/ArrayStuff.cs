using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Dek.Bel.Cls
{
    public class ArrayStuff
    {
        public static string ConvertArrayToString(int[] rects)
        {
            string res = "";
            int counter = 1;
            foreach (int i in rects)
                res += i.ToString() + $"{(counter++ % 4 == 0 ? ";" : ",")}";

            return res;
        }


        // Format #pageA!a1,b1,c1,d1;a2,b2,c2,d2;
        public static string ConvertPageAndArrayToString(int page, int[] rects)
        {
            string res = $"#{page}!";
            int counter = 1;
            foreach (int i in rects)
                res += i.ToString() + $"{(counter++ % 4 == 0 ? ";" : ",")}";

            return res;
        }

        // Format #pageA!a1,b1,c1,d1;a2,b2,c2,d2;
        public static string ConvertPageAndArrayToString(List<(int page, int[] rects)> pageRects)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pageRect in pageRects)
            {
                sb.Append(ConvertPageAndArrayToString(pageRect.page, pageRect.rects));
            }
            return sb.ToString();
        }

        // Format #pageA!a1,b1,c1,d1;a2,b2,c2,d2;#pageB!a1,b1,c1,d1;
        public static List<(int page, int[] rects)> ConvertStringToPagesAndArrays(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return new List<(int page, int[] rects)> { (0, new int[0]) };

            List<string> rectSets = str.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<(int page, int[] rects)> res = new List<(int page, int[] rects)>();
            foreach (string rectSet in rectSets)
            {
                string[] split = rectSet.Split('!');

                int page = int.Parse(split[0]);

                int[] rectsInPage = ConvertStringToArray(split[1]);

                List<(int page, int[] rects)> pageRects = SplitRectsPerPage(page, rectsInPage);

                res.AddRange(pageRects);
            }

            return res;
        }

        // This takes a rect and splits it into several pages, by when a rect crosses over to next page.
        // Detected by y coord suddenly becoming smaller than previous y coord.
        public static List<(int page, int[] rects)> SplitRectsPerPage(int firstPage, int[] rects)
        {
            int curPage = firstPage;
            List<(int page, int[] rects)> pageRects = new List<(int page, int[] rects)>();
            int oldYCoord = int.MinValue / 2;
            List<int> agga = new List<int>();
            for (int i = 0; i < rects.Length / 4; i++)
            {
                int yCoord = rects[i * 4 + 1];
                int rowHeight = rects[i * 4 + 3];
                if (yCoord - oldYCoord > -(rowHeight / 2))
                {
                    agga.AddRange(new List<int> { rects[i * 4 + 0], rects[i * 4 + 1], rects[i * 4 + 2], rects[i * 4 + 3] });
                }
                else
                {
                    pageRects.Add((curPage++, agga.ToArray()));
                    agga.Clear();
                    agga.AddRange(new List<int> { rects[i * 4 + 0], rects[i * 4 + 1], rects[i * 4 + 2], rects[i * 4 + 3] });
                }
                oldYCoord = yCoord;
            }

            // Finish up
            pageRects.Add((curPage, agga.ToArray()));
            return pageRects;
        }

        private static int[] ConvertStringToArray(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return new int[0];

            string[] rects = str.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            List<int> res = new List<int>();
            foreach (var r in rects)
            {
                string[] values = r.Split(',');
                if (values.Length != 4)
                    continue;

                for (int j = 0; j < 4; j++)
                    res.Add(int.Parse(values[j]));
            }

            return res.ToArray();
        }

        public static int[] ExtractArrayFromIntPtr(IntPtr ptr, int len)
        {
            if (ptr == IntPtr.Zero)
                return new int[0];

            IntPtr start = ptr;
            int[] result = new int[len];
            Marshal.Copy(start, result, 0, len);
            return result;
        }

    }
}
