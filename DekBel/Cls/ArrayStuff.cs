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

        public static int[] ConvertStringToArray(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return new int[0];

            string[] rects = str.Split(';');
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
