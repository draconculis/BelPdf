using System.Collections.Generic;
using System.Windows.Forms;

namespace Dek.Bel.Categories
{
    public interface ICategoryService
    {
        IEnumerable<CategoryModel> Categories { get; }
        void Add(CategoryModel cat);
        void Remove(CategoryModel cat);

        Label CreateCategoryLabelControl(string text, bool isMain, ContextMenuStrip menu);
        void ClearMainStyleFromLabels(IEnumerable<Label> labels);
        void SetMainStyleOnLabel(Label label);
        void ClearMainStyleOnLabel(Label label);
    }
}