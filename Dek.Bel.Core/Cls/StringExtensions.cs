﻿using Dek.Cls;
using System;

namespace Dek.Cls
{
    public static class StringExtensions
    {
        public static string Range(this string me, int start, int stop)
        {
            if (string.IsNullOrEmpty(me))
                return string.Empty;

            // sane order
            if (start > stop)
                return me.Range(stop, start);

            int len = me.Length;
            int last = len - 1;

            // clamp
            if (stop < 0) stop = 0;
            if (stop > last) stop = last;
            if (start < 0) start = 0;
            if (start > last) start = last;

            return me.Substring(start, stop - start + 1);
        }

        public static string Range(this string me, DekRange range) => me.Range(range.Start, range.Stop);

        public static Guid ToGuid(this string me)
        {
            try
            {
                Guid.TryParse(me, out Guid guid);
                return guid;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static bool IsNullOrEmpty(this string me)
        {
            return string.IsNullOrEmpty(me);
        }

        public static bool IsNullOrWhiteSpace(this string me)
        {
            return string.IsNullOrWhiteSpace(me);
        }

        public static string RemoveCRLF(this string me)
        {
            string ret = me.Replace("\r", "¤¤¤¤").Replace("\n", "¤¤¤¤")
                .Replace(" ¤¤¤¤ ", " ")
                .Replace(" ¤¤¤¤", " ")
                .Replace("¤¤¤¤ ", " ")
                .Replace("¤¤¤¤", " ");

            return ret;
        }

        public static string RemoveDoubleSpace(this string me)
        {
            string ret = me
                .Replace("          ", " ")
                .Replace("          ", " ")
                .Replace("          ", " ")
                .Replace("          ", " ")
                .Replace("          ", " ")
                .Replace("         ", " ")
                .Replace("        ", " ")
                .Replace("       ", " ")
                .Replace("      ", " ")
                .Replace("     ", " ")
                .Replace("    ", " ")
                .Replace("   ", " ")
                .Replace("  ", " ")
                .Replace(" ", " ");

            return ret;
        }
        public static string RemoveLineBreaks(this string me)
        {
            string replacer = "¤#¤£$#¤";

            string ret = me
                .Replace("\r", replacer)
                .Replace("\n", replacer)
                .Replace($"{replacer}{replacer}", replacer)
                .Replace($" {replacer} ", replacer)
                .Replace($" {replacer}", replacer)
                .Replace($"{replacer} ", replacer)
                .Replace(replacer, " ");

            return ret;
        }

        public static string Left(this string me, int noOfChars, bool ellipsis = false)
        {
            int len = me.Length;
            if (len == 0 || noOfChars == 0)
                return string.Empty;
            if (len > noOfChars)
                return me.Substring(0, noOfChars);

            if (len == noOfChars)
                return me;

            return me + (ellipsis ? "…" : "");
        }


    }
}
