using Dek.Bel.Cls;
using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    public interface ICategoryService
    {
        // When set of categories change
        event EventHandler<CategoryEventArgs> CategoryUpdated;

        IEnumerable<Category> Categories { get; }
        List<CitationCategory> CitationCategories(Id citationId);

        Category CreateNewCategory(string code, string name, string description = null);
        Category InsertOrUpdate(Category cat);
        Category InsertOrUpdate(string code, string name, string desc);
        void Remove(Category cat);

        void AddCategoryToCitation(Id citationId, Id categoryId, int weight, bool isMain);
        void SetMainCategory(CitationCategory citationCategory);
        Category GetMainCategory(string citationId);
        Category GetMainCategory(Id citationId);
        CitationCategory GetMainCitationCategory(string citationId);
        CitationCategory GetMainCitationCategory(Id citationId);
        void SetWeight(Id citationId, Id categoryId, int weight);

        Label CreateCategoryLabelControl(CitationCategory citCat, Category cat, ContextMenuStrip menu, ToolTip toolTip);
        void ClearMainStyleFromLabels(IEnumerable<Label> labels);
        void SetMainStyleOnLabel(Label label);
        void ClearMainStyleOnLabel(Label label);
    }
}