using Dek.Bel.DB;
using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> Categories { get; }
        event EventHandler<CategoryEventArgs> CategoryUpdated;

        Category InsertOrUpdate(Category cat);
        void InsertOrUpdate(string code, string name, string desc);
        void Remove(Category cat);

        void AddCategoryToCitation(Id citationId, Id categoryId, int weight, bool isMain);
        void SetMainCategory(CitationCategory citationCategory);
        void SetWeight(Id citationId, Id categoryId, int weight);
        List<CitationCategory> GetCitationCategories(Id citationId);

        Label CreateCategoryLabelControl(CitationCategory citCat, Category cat, ContextMenuStrip menu, ToolTip toolTip);
        void ClearMainStyleFromLabels(IEnumerable<Label> labels);
        void SetMainStyleOnLabel(Label label);
        void ClearMainStyleOnLabel(Label label);
        
    }
}