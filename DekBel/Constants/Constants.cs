using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel
{
    public static class Constants
    {
        public const string DbName = "DB";

        public static class MarginBoxVisualMode
        {
            public const string Normal = nameof(Normal);
            public const string Compact = nameof(Compact);
            public const string Minimal = nameof(Minimal);
            public const string Hidden = nameof(Hidden);
        }

        public static List<string> MarginBoxVisualModes =>
            new List<string>
            {
                MarginBoxVisualMode.Normal,
                MarginBoxVisualMode.Compact,
                MarginBoxVisualMode.Minimal,
                MarginBoxVisualMode.Hidden,
            };

        // From iText.IO.Font.Constants
        public static class PdfFont
        {
            public const string COURIER = "Courier";
            public const string COURIER_BOLD = "Courier-Bold";
            public const string COURIER_OBLIQUE = "Courier-Oblique";
            public const string COURIER_BOLDOBLIQUE = "Courier-BoldOblique";
            public const string HELVETICA = "Helvetica";
            public const string HELVETICA_BOLD = "Helvetica-Bold";
            public const string HELVETICA_OBLIQUE = "Helvetica-Oblique";
            public const string HELVETICA_BOLDOBLIQUE = "Helvetica-BoldOblique";
            public const string SYMBOL = "Symbol";
            public const string TIMES_ROMAN = "Times-Roman";
            public const string TIMES_BOLD = "Times-Bold";
            public const string TIMES_ITALIC = "Times-Italic";
            public const string TIMES_BOLDITALIC = "Times-BoldItalic";
            public const string ZAPFDINGBATS = "ZapfDingbats";
        }

        public static List<string> PdfFonts =>
            new List<string>
            {
                PdfFont.COURIER,
                PdfFont.COURIER_BOLD,
                PdfFont.COURIER_OBLIQUE,
                PdfFont.COURIER_BOLDOBLIQUE,
                PdfFont.HELVETICA,
                PdfFont.HELVETICA_BOLD,
                PdfFont.HELVETICA_OBLIQUE,
                PdfFont.HELVETICA_BOLDOBLIQUE,
                PdfFont.SYMBOL,
                PdfFont.TIMES_ROMAN,
                PdfFont.TIMES_BOLD,
                PdfFont.TIMES_ITALIC,
                PdfFont.TIMES_BOLDITALIC,
                PdfFont.ZAPFDINGBATS,
            };
    }

}

