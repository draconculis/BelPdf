using Dek.Cls;

namespace Dek.Cls
{
    public static class StringExtensions
    {
        public static string Range(this string me, int start, int stop)
        {
            if (string.IsNullOrEmpty(me))
                return string.Empty;

            int len = me.Length;
            int last = len - 1;

            // sane order
            if (start > stop)
            {
                int tmp = stop;
                stop = start;
                start = tmp;
            }

            // clamp
            if (stop < 0) stop = 0;
            if (stop > last) stop = last;
            if (start < 0) start = 0;
            if (start > last) start = last;

            return me.Substring(start, stop - start + 1);
        }

        public static string Range(this string me, TextRange range) => me.Range(range.Start, range.Stop);
    }
}
