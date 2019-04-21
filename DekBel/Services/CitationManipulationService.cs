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
        [Import] public IDBService DBService { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }

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
            var resetform = new Form_ResetCitation(VM.CurrentCitation.Citation1, UserSettingsService.CitationFont);

            if (resetform.ShowDialog() == DialogResult.Yes)
            {
                VM.CurrentCitation.Citation2 = VM.CurrentCitation.Citation2;
                VM.Exclusion.Clear();
                VM.CurrentCitation.Exclusion = null;
                DBService.InsertOrUpdate(VM.CurrentCitation);
                FireCitationChanged();
            }
        }

        public void ExcludeSelectedText(int from, int to)
        {
            var range = new DekRange(from, to);

            VM.Exclusion = VM.Exclusion.AddAndMerge(range);
            VM.CurrentCitation.Exclusion = VM.Exclusion.ConvertToText();

            DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        public void AddEmphasis(int from, int to)
        {
            var range = new DekRange(from, to);

            VM.Emphasis = VM.Emphasis.AddAndMerge(range);
            VM.CurrentCitation.Emphasis = VM.Emphasis.ConvertToText();

            DBService.InsertOrUpdate(VM.CurrentCitation);
            FireCitationChanged();
        }

        public void BeginEdit()
        {
            VM.CurrentCitation.Citation3 = VM.CurrentCitation.Citation2;
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
