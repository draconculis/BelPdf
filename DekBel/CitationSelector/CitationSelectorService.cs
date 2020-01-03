using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Bel.Services;
using Dek.Bel.Services.CitationDeleterService;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return f.SelectedCitation;
        }
    }
}
