using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Cls;

namespace Dek.Bel.Services
{
    [Export]
    public class RichTextService
    {
        public const char BelBold = '☺';
        public const char BelStrike = '☻';
        [Import] IUserSettingsService m_UserSettingsService { get; set; }

        public string CreateRtfWithExlusionsAndEmphasis(string text, List<DekRange> exclusion, List<DekRange> emphasis)
        {
            string s = text.Replace("\r", "").Replace("\n", "");

            if (emphasis == null)
                emphasis = new List<DekRange>();

            if (exclusion == null)
                exclusion = new List<DekRange>();

            if (text.IsNullOrWhiteSpace())
                return @"{\rtf1\ansi }";

            bool boldEmphasis = m_UserSettingsService.BoldEmphasis;
            bool underlineEmphasis = m_UserSettingsService.UnderlineEmphasis;

            StringBuilder rtfbuilder = new StringBuilder();
            rtfbuilder.Append(@"{\rtf1\ansi ");
            bool bold = false;
            bool strike = false;
            for(int i = 0; i < s.Length; i++)
            {
                if (emphasis.ContainsInteger(i) && !bold)
                {
                    if(boldEmphasis)
                        rtfbuilder.Append(@"\b ");
                    if(underlineEmphasis)
                        rtfbuilder.Append(@"\u ");
                    bold = true;
                }
                if (!emphasis.ContainsInteger(i) && bold)
                {
                    if (boldEmphasis)
                        rtfbuilder.Append(@"\b0 ");
                    if (underlineEmphasis)
                        rtfbuilder.Append(@"\u0 ");

                    bold = false;
                }

                if (exclusion.ContainsInteger(i) && !strike)
                {
                    rtfbuilder.Append(@"\strike ");
                    strike = true;
                }
                if (!exclusion.ContainsInteger(i) && strike)
                {
                    rtfbuilder.Append(@"\strike0 ");
                    strike = false;
                }

                if (s[i] == '\n')
                {
                    
                }

                if (s[i] == '\r')
                {
                }

                rtfbuilder.Append(GetRtfUnicodeEscapedChar(s[i]));
            }

            rtfbuilder.Append(@" }");
            var ret = rtfbuilder.ToString();
            return ret;
        }

        static string GetRtfUnicodeEscapedChar(char c)
        {
                if (c <= 0x7f)
                    return c.ToString();
                else
                    return "\\u" + Convert.ToUInt32(c) + "?";
        }

        static string GetRtfUnicodeEscapedString(string s)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (c <= 0x7f)
                    sb.Append(c);
                else
                    sb.Append("\\u" + Convert.ToUInt32(c) + "?");
            }
            return sb.ToString();
        }

    }
}
