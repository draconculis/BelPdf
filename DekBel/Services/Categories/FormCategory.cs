using Dek.Bel.Services;
using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dek.Bel.DB;

namespace Dek.Bel.Services
{
    public partial class FormCategory : Form
    {
        public ICategoryService m_CategoryService;
        private IEnumerable<Category> m_FilteredCategories => m_CategoryService.Categories.Where(c => Filter(c)).ToList();
        private Predicate<Category> Filter;

        public FormCategory(ICategoryService categoryService)
        {
            InitializeComponent();
            m_CategoryService = categoryService;
            Filter = (c) => true;
            dataGridView1.DataSource = m_FilteredCategories;
            UpdateCount();
            m_CategoryService.CategoryUpdated += OnCategoryUpdated;
            
        }

        public void OnCategoryUpdated(object sender, CategoryEventArgs args)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = m_FilteredCategories;
            AdjustColumns();
        }

        private void FormCategory_Load(object sender, EventArgs e)
        {
            AdjustColumns();
        }

        private void AdjustColumns()
        {
            int previouswidth = dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width + dataGridView1.Columns[2].Width;
            dataGridView1.Columns[3].Width = dataGridView1.Width - previouswidth;

        }

        private void UpdateCount()
        {
            label_count.Text = $"{m_FilteredCategories.Count()} / {m_CategoryService.Categories.Count().ToString()}";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button_done_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_search.Text = string.Empty;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_search.Text))
            {
                Filter = (c) => true;
                UpdateCount();
            }
            else
            {
                Filter = (cat) =>
                {
                    string filter = textBox_search.Text;
                    return cat.Code.ToLower().Contains(filter)
                        || cat.Name.ToLower().Contains(filter)
                        || cat.Description.ToLower().Contains(filter);
                };

                
            }
            dataGridView1.DataSource = m_FilteredCategories;
            UpdateCount();

            timer1.Stop();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            AdjustColumns();
        }

        // Edit
        private void button_update_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;

            int row = dataGridView1.SelectedCells[0].RowIndex;
            var cat = new Category {
                Code = dataGridView1.Rows[row].Cells[1].Value as string,
                Name = dataGridView1.Rows[row].Cells[2].Value as string,
                Description = dataGridView1.Rows[row].Cells[3].Value as string
            };

            FormCategoryEdit f = new FormCategoryEdit(m_CategoryService.Categories, cat);
            if(f.ShowDialog(this) == DialogResult.OK)
            {
                m_CategoryService.InsertOrUpdate(f.Category);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = m_FilteredCategories;
                AdjustColumns();
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            FormCategoryEdit f = new FormCategoryEdit(m_CategoryService.Categories);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                m_CategoryService.InsertOrUpdate(f.Category);
                AdjustColumns();
            }
        }

        private void Button_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;

            int row = dataGridView1.SelectedCells[0].RowIndex;
            var cat = new Category
            {
                Id = Id.NewId(dataGridView1.Rows[row].Cells[0].Value as string),
                Code = dataGridView1.Rows[row].Cells[1].Value as string,
                Name = dataGridView1.Rows[row].Cells[2].Value as string,
                Description = dataGridView1.Rows[row].Cells[3].Value as string
            };

            if (MessageBox.Show($"Delete this category {cat}?", "Delete Category?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    m_CategoryService.Remove(cat);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = m_FilteredCategories;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Could not remove category!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                AdjustColumns();
            }

        }
    }
}
