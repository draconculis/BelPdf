﻿using System;
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


        public string CreateRtfWithExlusionsAndEmphasis(string text, List<TextRange> exclusion, List<TextRange> emphasis)
        {
            if (emphasis == null)
                emphasis = new List<TextRange>();

            if (exclusion == null)
                exclusion = new List<TextRange>();

            if (text.IsNullOrWhiteSpace())
                return @"{\rtf1\ansi }";

            StringBuilder rtfbuilder = new StringBuilder();
            rtfbuilder.Append(@"{\rtf1\ansi ");
            bool bold = false;
            bool strike = false;
            for(int i = 0; i < text.Length; i++)
            {
                if (emphasis.ContainsInteger(i) && !bold)
                {
                    rtfbuilder.Append(@"\b ");
                    bold = true;
                }
                if (!emphasis.ContainsInteger(i) && bold)
                {
                    rtfbuilder.Append(@"\b0 ");
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

                rtfbuilder.Append(text[i]);
            }

            rtfbuilder.Append(@" }");
            var ret = rtfbuilder.ToString();
            return ret;
        }



    }
}
