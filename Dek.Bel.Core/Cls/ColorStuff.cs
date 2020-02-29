using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Dek.Cls
{
    public class ColorStuff
    {
        public static string ConvertColorToString(Color color)
        {
            return $"{color.R},{color.G},{color.B},{color.A};";
        }

        public static string ConvertColorsToString(Color color1, Color color2, Color color3)
        {
            return ConvertColorToString(color1) + ConvertColorToString(color2) + ConvertColorToString(color3);
        }

        public static string ConvertColorsToString(IEnumerable<Color> colors)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var color in colors)
                sb.Append(ConvertColorToString(color));

            return sb.ToString();
        }

        public static Color[] ConvertStringToColors(string str)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(str))
                    return new Color[0];

                string[] rects = str.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var res = new List<Color>();
                foreach (var r in rects)
                {
                    string[] values = r.Split(',');
                    if (values.Length != 4)
                        continue;

                    res.Add(GetColor(
                        int.Parse(values[0]),   // r
                        int.Parse(values[1]),   // g
                        int.Parse(values[2]),   // b
                        int.Parse(values[3]))); // a
                }

                return res.ToArray();
            }
            catch
            {

            }
            return new Color[0];
        }

        public static Color GetColor(int r, int g, int b, int a = 255)
        {
            return Color.FromArgb(a, r, g, b);
        }

        public static Color GetColor(float r, float g, float b, float a = 1)
        {
            return Color.FromArgb(f2b(a), f2b(r), f2b(g), f2b(b));
        }

        public static float b2f(byte b)
        {
            return (b == 0) ? 0f : (float)b / 255;
        }

        public static byte f2b(float f)
        {
            return (byte)(f * 255f);
        }

    }
}
