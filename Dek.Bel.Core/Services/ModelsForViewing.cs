using BelManagedLib;
using Dek.Cls;
using Dek.Bel.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Dek.Bel.Core.Services
{
    [Export]
    public class ModelsForViewing
    {
        // Current data
        public EventData Message { get; set; }
        public Citation CurrentCitation { get; set; }
        public Storage CurrentStorage { get; set; }

        public List<DekRange> Emphasis { get; set; } = new List<DekRange>();
        public List<DekRange> Exclusion { get; set; } = new List<DekRange>();

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
