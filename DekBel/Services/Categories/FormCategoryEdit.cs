using Dek.Bel.Core.Models;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    public partial class FormCategoryEdit : Form
    {
        public Category Category { get; private set; }
        IEnumerable<Category> Categories;

        // Track Update 
        private Category OriginalCategory;

        // Colors
        Color NormalColor = Color.White;
        Color ErrorColor = Color.MistyRose;
        Color WarnColor = Color.LightYellow;

        // Update
        public FormCategoryEdit(IEnumerable<Category> categories, Category cat)
        {
            Categories = categories;
            OriginalCategory = cat;
            InitializeComponent();
            IsUpdate = true;

            textBoxCode.Text = cat.Code;
            textBoxName.Text = cat.Name;
            textBoxDesc.Text = cat.Description;

            Text = $"Edit Category {cat}";
        }

        // Add
        public FormCategoryEdit(IEnumerable<Category> categories)
        {
            Categories = categories;
            OriginalCategory = new Category();
            OriginalCategory.Id = Id.Null;
            InitializeComponent();

            Text = $"New Category";
        }

        private bool IsUpdate { get; set; }

        private FormCategoryEdit() { }

        private void FormCategoryEdit_Load(object sender, EventArgs e)
        {
            IsOKEnabled();
            IsRestoreEnabled();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Category = new Category{
                Code = textBoxCode.Text.Trim(),
                Name = textBoxName.Text.Trim(),
                Description = textBoxDesc.Text.Trim(),
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            textBoxCode.Text = OriginalCategory.Code;
            textBoxName.Text = OriginalCategory.Name;
            textBoxDesc.Text = OriginalCategory.Description;
        }

        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdate && Categories.Any(c => c.Code.ToLower() == textBoxCode.Text.Trim().ToLower()))
            {
                buttonOK.Enabled = false;
                textBoxCode.BackColor = ErrorColor;
                label_warn.Visible = true;
                label_warn.Text = "Code must be unique";
                return;
            }
            else if (CodeChanged() &&
                Categories
                .Where(x => x.Id != OriginalCategory.Id)
                .Any(c => c.Code.ToLower() == textBoxCode.Text.Trim().ToLower()))
            {
                buttonOK.Enabled = false;
                textBoxCode.BackColor = ErrorColor;
                label_warn.Visible = true;
                label_warn.Text = "New code must be unique";
                return;
            }

            label_warn.Visible = false;
            IsOKEnabled();
            IsRestoreEnabled();
            textBoxCode.BackColor = CodeChanged() ? WarnColor : NormalColor;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdate && 
                Categories
                .Any(c => c.Name.ToLower() == textBoxName.Text.Trim().ToLower()))
            {
                buttonOK.Enabled = false;
                textBoxName.BackColor = ErrorColor;
                label_warn.Visible = true;
                label_warn.Text = "Name must be unique";
                return;
            }
            else if (NameChanged() && 
                Categories
                .Where(x => x.Id != OriginalCategory.Id)
                .Any(c => c.Name.ToLower() == textBoxName.Text.Trim().ToLower()))
            {
                buttonOK.Enabled = false;
                textBoxName.BackColor = ErrorColor;
                label_warn.Visible = true;
                label_warn.Text = "New name must be unique";
                return;
            }

            label_warn.Visible = false;
            IsOKEnabled();
            IsRestoreEnabled();
            textBoxName.BackColor = NameChanged() ? WarnColor : NormalColor;
        }

        private void textBoxDesc_TextChanged(object sender, EventArgs e)
        {
            label_warn.Visible = false;
            IsOKEnabled();
            IsRestoreEnabled();
            textBoxDesc.BackColor = DescriptionChanged() ? WarnColor : NormalColor;
        }

        void IsOKEnabled()
        {
            buttonOK.Enabled =
                !string.IsNullOrWhiteSpace(textBoxName.Text)
                && !string.IsNullOrWhiteSpace(textBoxCode.Text)
                && !label_warn.Visible;
        }

        void IsRestoreEnabled()
        {
            buttonRestore.Visible = IsUpdate;

            buttonRestore.Enabled =
                IsUpdate &&
                (CodeChanged()
                || NameChanged()
                || DescriptionChanged());
        }

        /// <summary>
        /// Only return true if IsUpdate
        /// </summary>
        bool CodeChanged()
        {
            if (!IsUpdate)
                return false;

            return !textBoxCode.Text.Trim().Equals(OriginalCategory.Code);
        }

        /// <summary>
        /// Only return true if IsUpdate
        /// </summary>
        bool NameChanged()
        {
            if (!IsUpdate)
                return false;

            return !textBoxName.Text.Trim().Equals(OriginalCategory.Name);
        }

        /// <summary>
        /// Only return true if IsUpdate
        /// </summary>
        bool DescriptionChanged()
        {
            if (!IsUpdate)
                return false;

            return !textBoxDesc.Text.Equals(OriginalCategory.Description);
        }

    }
}
