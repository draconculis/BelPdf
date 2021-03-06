﻿using Dek.DB;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Dek.Bel.Core.Services
{
    /// <summary>
    /// Collection of methods for editing, formatting and gennerally mess around with the citation data.
    /// </summary>

    [Export]
    public class CitationManipulationService
    {
        [Import] public ModelsForViewing VM { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }

        EventHandler CitationChangedEventHandler;

        public event EventHandler CitationChanged
        {
            add => CitationChangedEventHandler += value;
            remove => CitationChangedEventHandler -= value;
        }

        public void ExcludeSelectedText(int from, int to)
        {
            var range = new DekRange(from, to);

            if (VM.Exclusion.ContainsInteger(from))
            {
                VM.Exclusion = VM.Exclusion.SubtractRange(range);
            }
            else
            {
                VM.Exclusion = VM.Exclusion.AddAndMerge(range);
            }

            VM.CurrentCitation.Exclusion = VM.Exclusion.ConvertToText();

            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        public void AddEmphasis(int from, int to)
        {
            var range = new DekRange(from, to);

            if (VM.Emphasis.ContainsInteger(from))
            {
                VM.Emphasis = VM.Emphasis.SubtractRange(range);
            }
            else
            {
                VM.Emphasis = VM.Emphasis.AddAndMerge(range);
            }

            VM.CurrentCitation.Emphasis = VM.Emphasis.ConvertToText();

            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        /// <summary>
        /// Removes linebreak in a segmens for Citation2
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void RemoveLinebreakInCitation2(int from, int to)
        {
            string s = VM.CurrentCitation.Citation2;

            StringBuilder sb = new StringBuilder();
            bool previousWasSpace = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (i >= from && i <= to)
                {
                    if (s[i] == ' ')
                    {
                        previousWasSpace = true;
                        sb.Append(" ");
                        continue;
                    }
                    else if (s[i] == '\r' || s[i] == '\n')
                    {
                        if (previousWasSpace)
                        {
                            AdjustExclusionRemoveOneCharAt(i);
                            continue;
                        }
                        else
                        {
                            sb.Append(" ");
                            previousWasSpace = true;
                            continue;
                        }
                    }
                    sb.Append(s[i]);
                    previousWasSpace = false;
                }


                //if (i >= from && i <= to && (s[i] == '\r' || s[i] == '\n'))
                //{
                //    AdjustExclusionRemoveOneCharAt(i);
                //}
                //sb.Append(s[i]);
            }

            VM.CurrentCitation.Citation2 = sb.ToString();

            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        public void AdjustSpacesInCitation2(int from, int to)
        {
            string s = VM.CurrentCitation.Citation2;

            StringBuilder sb = new StringBuilder();
            char lastChar = '-';
            for (int i = 0; i < s.Length; i++)
            {
                if (i >= from && i <= to && char.IsWhiteSpace(s[i]) && char.IsWhiteSpace(lastChar))
                {
                    AdjustExclusionRemoveOneCharAt(i);
                    continue;
                }
                sb.Append(s[i]);
                lastChar = s[i];
            }

            //VM.CurrentCitation.Citation2 = sb.ToString().Replace("\r", "\r\n");

            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        /// <summary>
        /// Adjusts exclusion when removiung a char
        /// </summary>
        /// <param name="position"></param>
        public void AdjustExclusionRemoveOneCharAt(int position)
        {
            List<DekRange> ranges = new List<DekRange>();
            List<DekRange> newRanges = new List<DekRange>();
            ranges.LoadFromText(VM.CurrentCitation.Exclusion);

            foreach(DekRange range in ranges)
            {
                int start = range.Start;
                int stop = range.Stop;

                if (start > position)
                    start--;
                if (stop > position)
                    stop--;

                DekRange newRange = new DekRange(start, stop);
                newRanges.Add(newRange);
            }

            VM.CurrentCitation.Exclusion = newRanges.ConvertToText();
        }

        public void BeginEdit()
        {
            StringBuilder sb = new StringBuilder();
            bool inExclusion = false;
            List<DekRange> ex = VM.Exclusion;
            char lastChar = '#'; // Remember last char
            string s = VM.CurrentCitation.Citation2.Replace("\r\n", "\r").Replace("\n", "\r");
            int len = s.Length;
            for (int i = 0; i < len; i++)
            {
                char c = s[i];
                if (ex.ContainsInteger(i))
                {
                    if(!inExclusion)
                    {
                        inExclusion = true;
                        if (lastChar != ' ')
                            sb.Append(' ');
                        sb.Append(m_UserSettingsService.DeselectionMarker);
                    }
                    continue;
                }
                else
                {
                    if (inExclusion)
                    {
                        inExclusion = false;
                        
                        if (c != ' ')
                            sb.Append(' ');
                    }
                }

                sb.Append(c);
                lastChar = c;
            }

            VM.CurrentCitation.Citation3 = sb.ToString().Replace("\r", "\r\n"); ;
            VM.CurrentCitation.Emphasis = string.Empty;

            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        public void FireCitationChanged()
        {
            CitationChangedEventHandler(this, EventArgs.Empty);
        }


    }
}
