using Dek.Bel.DB;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Services
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

        /// <summary>
        /// Citation2 is shown in first rtf box. This makes citation2 = citation1 (the original citation)
        /// </summary>
        public void ResetCitation2()
        {
            var resetform = new Form_ResetCitation(VM.CurrentCitation.Citation1, m_UserSettingsService.CitationFont);

            if (resetform.ShowDialog() == DialogResult.Yes)
            {
                VM.CurrentCitation.Citation2 = VM.CurrentCitation.Citation1;
                VM.Exclusion.Clear();
                VM.CurrentCitation.Exclusion = null;
                m_DBService.InsertOrUpdate(VM.CurrentCitation);

                FireCitationChanged();
            }
        }

        public void ExcludeSelectedText(int from, int to)
        {
            var range = new DekRange(from, to);

            VM.Exclusion = VM.Exclusion.AddAndMerge(range);
            VM.CurrentCitation.Exclusion = VM.Exclusion.ConvertToText();

            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        public void AddEmphasis(int from, int to)
        {
            var range = new DekRange(from, to);

            VM.Emphasis = VM.Emphasis.AddAndMerge(range);
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
            //string s = VM.CurrentCitation.Citation2.Replace("\r\n", "\r");
            string s = VM.CurrentCitation.Citation2;

            //int lastIndex = Math.Max(to, VM.CurrentCitation.Citation2.Length - 1);
            //string s1 = VM.CurrentCitation.Citation2.Substring(0, from - 1);
            //string s2 = VM.CurrentCitation.Citation2.Substring(from, to);
            //string s3 = VM.CurrentCitation.Citation2.Substring(to == lastIndex ? to : to + 1);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (i >= from && i <= to && (s[i] == '\r' || s[i] == '\r'))
                {
                    AdjustExclusionRemoveOneCharAt(i);
                    continue;
                }
                sb.Append(s[i]);
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
                if (stop > stop)
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
                        //if (lastChar != ' ')
                        //    sb.Append(' ');
                        sb.Append(m_UserSettingsService.DeselectionMarker);
                    }
                    continue;
                }
                else
                {
                    if (inExclusion)
                    {
                        inExclusion = false;
                        
                        //if (lastChar != ' ' && !" ,.;:".Contains(c))
                        //    sb.Append(' ');
                    }
                }

                sb.Append(c);
                lastChar = c;
            }

            VM.CurrentCitation.Citation3 = sb.ToString().Replace("\r", "\r\n"); ;

            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        public string AdjustSpaces(string text)
        {
            return text
                .Trim()
                .Replace("            ", " ")
                .Replace("           ", " ")
                .Replace("          ", " ")
                .Replace("         ", " ")
                .Replace("        ", " ")
                .Replace("       ", " ")
                .Replace("      ", " ")
                .Replace("     ", " ")
                .Replace("    ", " ")
                .Replace("   ", " ")
                .Replace("  ", " ")
                .Trim();
        }


        void FireCitationChanged()
        {
            CitationChangedEventHandler.Invoke(this, EventArgs.Empty);
        }


    }
}
