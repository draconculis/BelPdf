using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Dek.Bel.Helpers
{
    public class PdfMarginBoxSettings
    {
        public int Width { get; }
        public int Height { get; }
        public int Margin { get; }
        public float BorderThickness { get; }
        public string Font { get; }
        public float FontSize { get; }
        public bool RightMargin { get; }
        public string DisplayMode { get; }

        public  PdfMarginBoxSettings(
                int width,
                int height,
                int margin,
                float borderThickness,
                string font,
                float fontSize,
                bool rightMargin,
                string displayMode
            )
        {
            Width = width;
            Height = height;
            Margin = margin;
            BorderThickness = borderThickness;
            Font = font;
            FontSize = fontSize;
            RightMargin = rightMargin;
            DisplayMode = displayMode;
        }

        public PdfMarginBoxSettings()
        {
            Width = 56;
            Height = 13;
            Margin = 11;
            BorderThickness = 0;
            Font = Constants.PdfFont.TIMES_ROMAN;
            FontSize = 9;
            RightMargin = false;
            DisplayMode = Constants.MarginBoxVisualMode.Normal;
        }

        public PdfMarginBoxSettings(string s) : this()
        {
            if (string.IsNullOrWhiteSpace(s))
                return;

            Width = GetWidth(s);
            Height = GetHeight(s);
            Margin = GetMargin(s);
            BorderThickness = GetBorderThickness(s);
            Font = GetFont(s);
            FontSize = GetFontSize(s);
            RightMargin = GetRightMargin(s);
            DisplayMode = GetDisplayMode(s);
        }

        public override string ToString() => $"{Width};{Height};{Margin};{BorderThickness};{Font};{FontSize};{RightMargin};{DisplayMode};";

        public static void LoadAComboBoxWithPdfFonts(ComboBox theComboBox)
        {
            foreach (string item in Constants.PdfFonts)
            {
                theComboBox.Items.Add(item);
            }
        }

        public static void LoadAComboBoxWithDisplayModes(ComboBox theComboBox)
        {
            foreach (string item in Constants.MarginBoxVisualModes)
            {
                theComboBox.Items.Add(item);
            }
        }

        #region Helpers ==========================================================

        // Yes order is significant
        private enum BoxValue
        {
            Width = 0,
            Height = 1,
            Margin = 2,
            BorderThickness = 3,
            Font = 4,
            FontSize = 5,
            RightMargin = 6,
            DisplayMode = 7,
            Last = 8
        };

        public static string ConvertToString(
            int width,
            int height,
            int margin,
            float borderThickness,
            string font,
            int fontSize,
            bool rightMargin,
            string displayMode)
        {
            return $"{width};{height};{margin};{borderThickness};{font};{fontSize};{rightMargin};{displayMode};";
        }

        public static string GetDisplayMode(string settingString)
        {
            string defaultValue = Constants.MarginBoxVisualMode.Normal;
            int idx = (int)BoxValue.DisplayMode;
            string[] s = SplitValues(settingString);
            if (s.Length < (int)BoxValue.Last)
                return defaultValue;

            return string.IsNullOrWhiteSpace(s[idx]) ? defaultValue : s[idx];
        }

        public static bool GetRightMargin(string settingString)
        {
            bool defaultValue = false;
            int idx = (int)BoxValue.RightMargin;
            string[] s = SplitValues(settingString);
            if (s.Length < (int)BoxValue.Last)
                return defaultValue;

            return s[idx].ToLower().Equals("true");
        }

        public static string GetFont(string settingString)
        {
            string defaultValue = Constants.PdfFont.TIMES_ROMAN;
            int idx = (int)BoxValue.Font;
            string[] s = SplitValues(settingString);
            if (s.Length < (int)BoxValue.Last)
                return defaultValue;

            return string.IsNullOrWhiteSpace(s[idx]) ? defaultValue : s[idx];
        }

        public static int GetWidth(string settingString)
        {
            int defaultValue = 56;
            int idx = (int)BoxValue.Width;
            string[] s = SplitValues(settingString);
            if (s.Length < (int)BoxValue.Last)
                return defaultValue;

            int ret;
            try
            {
                ret = int.Parse(s[idx]);
            }
            catch
            {
                ret = defaultValue;
            }

            return ret == 0 ? defaultValue : ret;
        }

        public static int GetHeight(string settingString)
        {
            int defaultValue = 13;
            int idx = (int)BoxValue.Height;
            string[] s = SplitValues(settingString);
            if (s.Length < (int)BoxValue.Last)
                return defaultValue;

            int ret;
            try
            {
                ret = int.Parse(s[idx]);
            }
            catch
            {
                ret = defaultValue;
            }

            return ret == 0 ? defaultValue : ret;
        }

        public static int GetMargin(string settingString)
        {
            int defaultValue = 11;
            int idx = (int)BoxValue.Margin;
            string[] s = SplitValues(settingString);
            if (s.Length < (int)BoxValue.Last)
                return defaultValue;

            int ret;
            try
            {
                ret = int.Parse(s[idx]);
            }
            catch
            {
                ret = defaultValue;
            }

            return ret == 0 ? defaultValue : ret;
        }

        public static int GetFontSize(string settingString)
        {
            int defaultValue = 9;
            int idx = (int)BoxValue.FontSize;
            string[] s = SplitValues(settingString);
            if (s.Length < (int)BoxValue.Last)
                return defaultValue;

            int ret;
            try
            {
                ret = int.Parse(s[idx]);
            }
            catch
            {
                ret = defaultValue;
            }

            return ret == 0 ? defaultValue : ret;
        }

        public static float GetBorderThickness(string settingString)
        {
            int defaultValue = 0;
            int idx = (int)BoxValue.BorderThickness;
            string[] s = SplitValues(settingString);
            if (s.Length < (int)BoxValue.Last)
                return defaultValue;

            float ret;
            try
            {
                ret = float.Parse(s[idx]);
            }
            catch
            {
                ret = defaultValue;
            }

            return ret;
        }

        private static string[] SplitValues(string s)
        {
            return s.Split(new char[] { ';' }, StringSplitOptions.None);
        }

        #endregion Helpers =======================================================
    }

}
