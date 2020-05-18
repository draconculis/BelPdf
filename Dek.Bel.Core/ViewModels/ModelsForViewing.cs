using BelManagedLib;
using Dek.Cls;
using Dek.Bel.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Dek.Bel.Core.ViewModels;

namespace Dek.Bel.Core.Services
{
    /// <summary>
    /// Collection class for common GUI state objects (current citation etc)
    /// </summary>
    [Export]
    public class ModelsForViewing
    {
        // Current data
        public EventData Message { get; set; }
        public Citation CurrentCitation { get; set; }
        public Storage CurrentStorage { get; set; }

        public List<DekRange> Emphasis { get; set; } = new List<DekRange>();
        public List<DekRange> Exclusion { get; set; } = new List<DekRange>();

        // This is a list of authors for the current volume
        public List<AuthorsGridViewModel> Authors { get; set; } = new List<AuthorsGridViewModel>();

        public void InitCitationData()
        {
            if (Exclusion == null)
                Exclusion = new List<DekRange>();
            else
                Exclusion.Clear();

            if (Emphasis == null)
                Emphasis = new List<DekRange>();
            else
                Emphasis.Clear();

            Exclusion.LoadFromText(CurrentCitation.Exclusion);
            Emphasis.LoadFromText(CurrentCitation.Emphasis);
        }
    }
}
