﻿using Dek.Bel.Models;
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

        // Update
        public FormCategoryEdit(IEnumerable<Category> categories, Category cat)
        {
            Categories = categories;
            InitializeComponent();
            textBoxCode.ReadOnly = true;

            textBoxCode.Text = cat.Code;
            textBoxName.Text = cat.Name;
            textBoxDesc.Text = cat.Description;
        }

        // Add
        public FormCategoryEdit(IEnumerable<Category> categories)
        {
            Categories = categories;
            InitializeComponent();
        }

        private FormCategoryEdit() { }

        private void FormCategoryEdit_Load(object sender, EventArgs e)
        {
            IsOKEnabled();
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

        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
            if ((!textBoxCode.ReadOnly) && Categories.Any(c => c.Code.ToLower() == textBoxCode.Text.Trim().ToLower()))
            {
                buttonOK.Enabled = false;
                textBoxCode.BackColor = Color.Pink;
                label_warn.Visible = true;
                label_warn.Text = "Code must be unique";
                return;
            }

            IsOKEnabled();
            textBoxCode.BackColor = textBoxDesc.BackColor;
            label_warn.Visible = false;

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if ((!textBoxCode.ReadOnly) && Categories.Any(c => c.Name.ToLower() == textBoxName.Text.Trim().ToLower()))
            {
                buttonOK.Enabled = false;
                textBoxName.BackColor = Color.Pink;
                label_warn.Visible = true;
                label_warn.Text = "Name must be unique";
                return;
            }

            IsOKEnabled();
            textBoxName.BackColor = textBoxDesc.BackColor;
            label_warn.Visible = false;
        }

        void IsOKEnabled()
        {
            buttonOK.Enabled = !(string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxCode.Text));
        }
    }
}
