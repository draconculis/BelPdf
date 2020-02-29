using Dek.Cls;
using Dek.Bel.Core.Models;
using System;
using System.ComponentModel;

namespace Dek.Bel.Core.Services
{
    public class CitationSelectorModel
    {
        [Browsable(false)]
        public Citation Model { get; }

        public CitationSelectorModel(Citation model)
        {
            Model = model;
        }

        [Browsable(false)]
        public Id Id => Model.Id;

        public string ShortId => Model.Id.ToStringShort();

        public string Citation1 => Model.Citation1;
        //public string Citation2 => Model.Citation2;
        public string Citation3 => Model.Citation3;
        public DateTime CreatedDate => Model.CreatedDate;
        public DateTime EditedDate => Model.EditedDate;

        public int PhysicalPageStart => Model.PhysicalPageStart;
        public int GlyphStart => Model.GlyphStart;
        public string SortOrder => $"{PhysicalPageStart:D5}{GlyphStart:D4}";

        [Browsable(false)]
        public Category MainCategory { get; set; }
        [Browsable(false)]
        public CitationCategory MainCitationCategory { get; set; }
        public string MainCategoryName => MainCategory.Name;
        public int MainCategoryWeight => MainCitationCategory.Weight;

        [Browsable(false)]
        public bool HasMainCategory => MainCitationCategory.CategoryId != Id.Empty;

    }
}
