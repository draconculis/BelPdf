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

                res.Add((page, ConvertStringToArray(split[1])));
            }

            return res;
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
