using Dek.Bel.DB;
using Dek.Bel.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> Categories { get; }
        void Add(Category cat);
        void Remove(Category cat);

        void LoadCategoriesFromDb();
        void AddCategoryToCitation(Id citationId, Id categoryId, int weight, bool isMain);
        void SetMainCategory(Id id, Category cat);
        void SetWeight(Id citationId, Id categoryId, int weight);
        List<CitationCategory> GetCitationCategories(Id citationId);

        Label CreateCategoryLabelControl(CitationCategory citCat, Category cat, ContextMenuStrip menu, ToolTip toolTip);
        void ClearMainStyleFromLabels(IEnumerable<Label> labels);
        void SetMainStyleOnLabel(Label label);
        void ClearMainStyleOnLabel(Label label);
        
    }
}