using Dek.Bel.Core.Models;
using Dek.Bel.Core.Services;
using System.ComponentModel.Composition;

namespace Dek.Bel.CitationSelector
{
    [Export]
    public class CitationSelectorService
    {
        [Import] public VolumeService m_VolumeService { get; set; }
        [Import] public ICategoryService m_CategoryService { get; set; }
        [Import] public CitationDeleterService m_CitationDeleterService { get; set; }

        public Citation ShowSelector(ModelsForViewing vm)
        {
            FormCitationSelector f = new FormCitationSelector(vm, m_VolumeService, m_CategoryService, m_CitationDeleterService);
            f.ShowDialog();
            return f.Cancel ? null : f.SelectedCitation;
        }
    }
}
