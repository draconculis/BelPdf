using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
