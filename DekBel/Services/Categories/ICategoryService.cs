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

        Label CreateCategoryLabelControl(string text, bool isMain, ContextMenuStrip menu);
        void ClearMainStyleFromLabels(IEnumerable<Label> labels);
        void SetMainStyleOnLabel(Label label);
        void ClearMainStyleOnLabel(Label label);
    }
}