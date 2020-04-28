using Dek.DB;
using Dek.Bel.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using Dek.Cls;
using System.Linq;
using Dek.Bel.Core.Services;

namespace Dek.Bel.Services
{
    [Export]
    public class CategoryLabelService
    {
        private Color m_LabelDefaultColor = Color.Moccasin;

        private readonly ICategoryService m_CategoryService;
        private readonly IDBService m_DBService;

        private IEnumerable<Category> Categories; // Cached categories
        private BorderStyle m_DefaultBorderStyle;

        [ImportingConstructor]
        CategoryLabelService(ICategoryService categoryService, IDBService dbService)
        {
            m_DefaultBorderStyle = new Label().BorderStyle;

            m_CategoryService = categoryService;
            m_CategoryService.CategoryUpdated += OnCategoryUpdated;

            m_DBService = dbService;
            Categories = m_DBService.Select<Category>();
        }

        private void OnCategoryUpdated(object sender, CategoryEventArgs e)
        {
            Categories = m_DBService.Select<Category>();
        }

        Color labelColorMouseOver = Color.Orange;
        public Label CreateCategoryLabelControl(CitationCategory citCat, Category cat, ContextMenuStrip menu, ToolTip toolTip)
        {
            Label l = new Label();
            Color[] color = ColorStuff.ConvertStringToColors(cat.CategoryColor);
            Color catColor = color.Any() ? color[0] : m_LabelDefaultColor;

            Font newFont = new Font("Times New Roman", 10, FontStyle.Regular);
            l.Font = newFont;
            l.MouseHover += L_MouseHover;
            l.MouseEnter += L_MouseEnter;
            l.MouseLeave += L_MouseLeave;
            l.AutoSize = true;
            l.BackColor = catColor;
            l.Text = $"{cat.Code} [{citCat.Weight}]";
            l.ContextMenuStrip = menu;
            if (citCat.IsMain)
                SetMainStyleOnLabel(l);
            l.Tag = citCat;
            toolTip.SetToolTip(l, cat.Name + Environment.NewLine + cat.Description);
            return l;
        }

        private void L_MouseLeave(object sender, EventArgs e)
        {
            // Get back original category color
            if (!(sender is Label l))
                return;

            if (!(l.Tag is CitationCategory)) 
            {
                l.BackColor = m_LabelDefaultColor;
                return;
            }

            string colorString = Categories.SingleOrDefault(x => x.Id == ((CitationCategory)l.Tag).CategoryId)?.CategoryColor;
            if(string.IsNullOrWhiteSpace(colorString))
            {
                l.BackColor = m_LabelDefaultColor;
                return;
            }

            Color[] colors = ColorStuff.ConvertStringToColors(colorString);
            if (colors.Any())
                l.BackColor = colors[0];
            else
                l.BackColor = m_LabelDefaultColor;
        }

        private void L_MouseEnter(object sender, EventArgs e)
        {
            if (!(sender is Label l))
                return;

            l.BackColor = labelColorMouseOver;
        }

        private void L_MouseHover(object sender, EventArgs e)
        {
            
        }

        public void ClearMainStyleFromLabels(IEnumerable<Label> labels)
        {
            foreach(Label label in labels)
            {
                ClearMainStyleOnLabel(label);
            }
        }

        public void SetMainStyleOnLabel(Label label)
        {
            label.BorderStyle = BorderStyle.FixedSingle;
        }

        public void ClearMainStyleOnLabel(Label label)
        {
            label.BorderStyle = m_DefaultBorderStyle;
        }

    }
}
