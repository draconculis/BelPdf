using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dek.Bel.Core.Models;
using Dek.Bel.Core.Services;
using Dek.Cls;
using Dek.DB;

namespace Dek.Bel.Services.Categories
{
    public partial class CategoryUserControl : UserControl
    {
        [Import] public ICategoryService m_CategoryService { get; set; }
        [Import] public CategoryLabelService m_CategoryLabelService { get; set; }
        [Import] public IDBService m_DBService { get; set; }

        private Id m_CurrentCitationId = Id.Null;
        public Id CurrentCitationId
        {
            get => m_CurrentCitationId;
            set
            {
                m_CurrentCitationId = value;

                if (value.IsNull)
                {
                    Enabled = false;
                }
                else
                {
                    Enabled = true;
                }

                if(m_CategoryService != null)
                    Update();
            }
        }

        public event EventHandler CategoryChanged;

        private const string CategoryAddText = "Add";
        private const string CategoryCreateText = "Create";

        private bool LoadingControls = true;
        FormCategory m_FormCategory = null;

        public CategoryUserControl()
        {
            InitializeComponent();

            comboBox_CategoryWeight.SelectedIndex = 2;

        }

        public void Update()
        {
            Enabled = CurrentCitationId.IsNotNull;

            LoadCategoryControl();
        }

        public void EditCategories()
        {
            button_Categories_Click(this, new EventArgs());
        }

        private void UserControl_Category_Load(object sender, EventArgs e)
        {
            if (!(Site?.DesignMode ?? false) && m_CategoryService == null)
                Mef.Compose(this);

            LoadCategoryControl();

            textBox_CategorySearch.Focus();
        }

        void LoadCategoryControl()
        {
            if (Site?.DesignMode ?? false)
                return;

            LoadingControls = true;
            flowLayoutPanel_Categories.Controls.Clear();
            if (CurrentCitationId.IsNull || m_CategoryService == null)
                return;

            var cgs = m_CategoryService.CitationCategoriesByCitation(CurrentCitationId);
            //var cgs = m_CategoryService.CitationCategoriesByCitation(VM.CurrentCitation.Id);
            var categories = m_CategoryService.Categories;

            foreach (var cg in cgs)
            {
                if (cg.CategoryId.IsNull)
                    continue;

                Category cat = categories.SingleOrDefault(x => x.Id == cg.CategoryId);
                if (cat != null)
                    AddCategoryLabel(cg, cat);
            }

            LoadingControls = false;
        }

        private void UserControl_Category_Enter(object sender, EventArgs e)
        {
            textBox_CategorySearch.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void FireCategoryChanged(CitationCategory citCat)
        {
            CategoryChanged?.Invoke(this, new CategoryChangedEventArgs(citCat));
        }

        private void button_CategoryAddCreate_Click(object sender, EventArgs e)
        {
            if (button_CategoryAddCreate.Text == CategoryAddText)
            {
                if (!(listBox_Categories.SelectedItem is Category cat))
                    return;

                bool hasNullCategory = m_CategoryService.CitationHasNullCategory(CurrentCitationId);
                bool hasMainCategory = m_CategoryService.CitationHasMainCategory(CurrentCitationId);

                bool isMain = (flowLayoutPanel_Categories.Controls.Count == 0) || hasNullCategory || !hasMainCategory;

                m_CategoryService.AddCategoryToCitation(CurrentCitationId, cat.Id, int.Parse((comboBox_CategoryWeight.SelectedItem as string) ?? "1"), isMain);
                LoadCategoryControl();

                textBox_CategorySearch.Text = "";
            }
            else
            {
                if (textBox_CategorySearch.Focused)
                    return;

                string s = textBox_CategorySearch.Text.Trim().Replace("-", " ").Replace("   ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
                if (string.IsNullOrWhiteSpace(s))
                    return;

                Regex regex = new Regex(@"^\w+ +\w+$");
                if (!regex.IsMatch(s))
                {
                    MessageBox.Show($"To create new category, please use the following format:{Environment.NewLine}Code Name", "Invalid format");
                    textBox_CategorySearch.Focus();
                    return;
                }

                string[] parts = s.Split(' ');

                // Create new category
                Category newCategory = m_CategoryService.CreateNewCategory(parts[0], parts[1]);
                if (newCategory == null)
                {
                    MessageBox.Show($"New category cannot be added, category with code '{parts[0]}' already exists.", "Category exists");
                    textBox_CategorySearch.Focus();
                    return;
                }

                bool hasNullCategory = m_CategoryService.CitationCategoriesByCitation(CurrentCitationId).Any(x => x.CitationId == Id.Null);
                bool hasMainCategory = m_CategoryService.CitationCategoriesByCitation(CurrentCitationId).Any(x => x.IsMain);

                bool isMain = (flowLayoutPanel_Categories.Controls.Count == 0) || hasNullCategory || !hasMainCategory;

                m_CategoryService.AddCategoryToCitation(CurrentCitationId, newCategory.Id, int.Parse((comboBox_CategoryWeight.SelectedItem as string) ?? "1"), isMain);
                LoadCategoryControl();

                textBox_CategorySearch.Text = "";
                button_CategoryAddCreate.Text = CategoryAddText;
            }

        }

        private void AddCategoryLabel(CitationCategory citationCat, Category cat)
        {
            Label l = m_CategoryLabelService.CreateCategoryLabelControl(citationCat, cat, contextMenuStrip_Category, toolTip1);

            if (citationCat.IsMain)
                m_CategoryLabelService.SetMainStyleOnLabel(l);

            flowLayoutPanel_Categories.Controls.Add(l);
            textBox_CategorySearch.Focus();
        }

        private void textBox_CategorySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
        }

        private void textBox_CategorySearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox textbox))
                return;

            if (e.KeyCode == Keys.Return)
            {
                if (button_CategoryAddCreate.Text == CategoryCreateText)
                {
                    button_CategoryAddCreate_Click(sender, e);
                }
                else
                {
                    if (listBox_Categories.Visible)
                        listBox_Categories_Click(sender, e);
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Down && listBox_Categories.Visible)
            {
                if (listBox_Categories.SelectedIndex + 1 == listBox_Categories.Items.Count)
                    return;

                listBox_Categories.SelectedIndex++;

                return;
            }

            if (e.KeyCode == Keys.Up && listBox_Categories.Visible)
            {
                if (listBox_Categories.SelectedIndex == 0)
                    return;

                listBox_Categories.SelectedIndex--;

                return;
            }

            listBox_Categories.Items.Clear();
            var citCats = m_CategoryService.CitationCategoriesByCitation(CurrentCitationId);
            //var cats = m_CategoryService.Categories
            //    .Where(x =>
            //    x.Code.ToLower().Contains(textbox.Text.ToLower())
            //    || x.Name.ToLower().Contains(textbox.Text.ToLower()))
            //    .Where(c => c.Id != Id.Null)
            //    .Where(d => !citCats.Any(f => f.CategoryId == d.Id))
            //    .ToList();

            if (textbox.Text.TrimStart().IndexOf(" ") > 1)
            {
                button_CategoryAddCreate.Text = CategoryCreateText;
                listBox_Categories.Visible = false;
            }
            else
            {
                button_CategoryAddCreate.Text = CategoryAddText;

                var cats = m_CategoryService.Categories
                    .Where(x =>
                        x.Code.ToLower().Contains(textbox.Text.ToLower())
                       || x.Name.ToLower().Contains(textbox.Text.ToLower()))
                    .Where(c => c.Id != Id.Null)
                    .Where(d => !citCats.Any(f => f.CategoryId == d.Id))
                    .ToList();

                if (cats.Count < 1 || textbox.Text.Length < 2)
                {
                    listBox_Categories.Visible = false;
                    return;
                }

                foreach (var c in cats)
                    listBox_Categories.Items.Add(c);

                listBox_Categories.SelectedIndex = 0;
                if (!listBox_Categories.Visible)
                {
                    listBox_Categories.SelectedIndex = 0;
                    listBox_Categories.Top = textbox.Top + textbox.Height;
                    listBox_Categories.Left = textbox.Left;
                    listBox_Categories.Width = textbox.Width;
                    listBox_Categories.Visible = true;
                }
            }

        }

        private void listBox_Categories_Click(object sender, EventArgs e)
        {
            if (!(listBox_Categories.SelectedItem is Category cat))
                return;

            bool hasNullCategory = m_CategoryService.CitationHasNullCategory(CurrentCitationId);
            bool hasMainCategory = m_CategoryService.CitationHasMainCategory(CurrentCitationId);

            bool isMain = (flowLayoutPanel_Categories.Controls.Count == 0) || hasNullCategory || !hasMainCategory;

            textBox_CategorySearch.Text = cat.ToString();

            listBox_Categories.Visible = false;
        }

        #region Context menu ==============================

        private void setAsMainCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((sender as ToolStripMenuItem)?.Owner as ContextMenuStrip)?.SourceControl is Label label)
            {
                m_CategoryService.SetMainCategory(label.Tag as CitationCategory);
                m_CategoryLabelService.ClearMainStyleFromLabels(flowLayoutPanel_Categories.Controls.OfType<Label>());
                m_CategoryLabelService.SetMainStyleOnLabel(label);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((sender as ToolStripMenuItem)?.Owner as ContextMenuStrip)?.SourceControl is Label label)
            {
                var cc = (CitationCategory)label.Tag;
                m_DBService.Delete<CitationCategory>($"`{nameof(CitationCategory.CitationId)}`='{cc.CitationId}' AND `{nameof(CitationCategory.CategoryId)}`='{cc.CategoryId}'");
                flowLayoutPanel_Categories.Controls.Remove(label);
            }
        }

        private void setWeight1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 1);
        }

        private void setWeight2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 2);
        }

        private void setWeight3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 3);
        }

        private void setWeight4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 4);
        }

        private void setWeight5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 5);
        }

        private void SetWeight(ToolStripMenuItem item, int weight)
        {
            if (!(((ContextMenuStrip)item.GetCurrentParent()).SourceControl.Tag is CitationCategory citationCategory))
                return;

            m_CategoryService.SetWeight(CurrentCitationId, citationCategory.CategoryId, weight);
            LoadCategoryControl();
        }

        #endregion Context menu

        // Form category (edit categories)
        private void button_Categories_Click(object sender, EventArgs e)
        {
            if (m_FormCategory == null)
            {
                m_FormCategory = new FormCategory(m_CategoryService);
                m_FormCategory.Show(this);
                m_FormCategory.CategoryChanged += OnCategoryChagedInFormCategory;
                m_FormCategory.FormClosed += (_, __) =>
                {
                    m_FormCategory.CategoryChanged -= OnCategoryChagedInFormCategory;
                    m_FormCategory = null;
                };
            }
            else
                m_FormCategory.Visible = !m_FormCategory.Visible;

        }

        private void OnCategoryChagedInFormCategory(object sender, CategoryEventArgs e)
        {
            LoadCategoryControl();
        }
    }

}
