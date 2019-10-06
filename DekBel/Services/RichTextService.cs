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
            string s = text.Replace("\r\n", "\r").Replace("\n", "\r");//.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", "");

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
            bool inEmphasis = false;
            bool inExclusion = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (emphasis.ContainsInteger(i) && !inEmphasis)
                {
                    if(boldEmphasis)
                        rtfbuilder.Append(@"\b ");
                    if(underlineEmphasis)
                        rtfbuilder.Append(@"\u ");

                    inEmphasis = true;
                }
                if (!emphasis.ContainsInteger(i) && inEmphasis)
                {
                    if (underlineEmphasis)
                        rtfbuilder.Append(@"\u0 ");
                    if (boldEmphasis)
                        rtfbuilder.Append(@"\b0 ");

                    inEmphasis = false;
                }

                if (exclusion.ContainsInteger(i) && !inExclusion)
                {
                    rtfbuilder.Append(@"\strike ");
                    inExclusion = true;
                }
                if (!exclusion.ContainsInteger(i) && inExclusion)
                {
                    rtfbuilder.Append(@"\strike0 ");
                    inExclusion = false;
                }

                rtfbuilder.Append(GetRtfUnicodeEscapedChar(s[i]));
                //rtfbuilder.Append(s[i]);
            }

            rtfbuilder.Append(@" }");
            var ret = rtfbuilder.ToString();
            ret = ret.Replace("\r", @"\line ");
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
