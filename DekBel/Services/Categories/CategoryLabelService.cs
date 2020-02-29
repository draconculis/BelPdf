using Dek.DB;
using Dek.Bel.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    [Export]
    public class CategoryLabelService
    {
        public IEnumerable<Category> Categories => m_DBService.Select<Category>();
        private IDBService m_DBService;

        private BorderStyle m_DefaultBorderStyle;

        [ImportingConstructor]
        CategoryLabelService(IDBService dBService)
        {
            m_DefaultBorderStyle = new Label().BorderStyle;

            m_DBService = dBService;
        }

        Color labelColor = Color.Moccasin;
        Color labelColorMouseOver = Color.Orange;
        public Label CreateCategoryLabelControl(CitationCategory citCat, Category cat, ContextMenuStrip menu, ToolTip toolTip)
        {
            Label l = new Label();
            Font newFont = new Font("Times New Roman", 10, FontStyle.Regular);
            l.Font = newFont;
            l.MouseHover += L_MouseHover;
            l.MouseEnter += L_MouseEnter;
            l.MouseLeave += L_MouseLeave;
            l.AutoSize = true;
            l.BackColor = labelColor;
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
            if (!(sender is Label l))
                return;

            l.BackColor = labelColor;
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
