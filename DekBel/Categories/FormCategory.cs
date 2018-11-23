using Dek.Bel.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Categories
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
        }

        private void FormCategory_Load(object sender, EventArgs e)
        {
            int previouswidth = dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width;
            dataGridView1.Columns[2].Width = dataGridView1.Width - previouswidth;

        }

        private void UpdateCount()
        {
            label_count.Text = $"{m_FilteredCategories.Count()} / {m_CategoryService.Categories.Count().ToString()}";
        }

        //void LoadListView()
        //{
        //    listView1.Items.Clear();

        //    string filter = textBox_search.Text.Trim().ToLower();
        //    foreach(var cat in m_CategoryService.Categories)
        //    {
        //        if (!string.IsNullOrWhiteSpace(filter) && 
        //            !(cat.Code.ToLower().Contains(filter) || cat.Name.ToLower().Contains(filter) || cat.Description.ToLower().Contains(filter)))
        //            continue;

        //        var lvi = new ListViewItem(new[] { cat.Code, cat.Name, cat.Description });
        //        listView1.Items.Add(lvi);
        //    }
        //}

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
            int previouswidth = dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width;
            dataGridView1.Columns[2].Width = dataGridView1.Width - previouswidth;
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;


            int row = dataGridView1.SelectedCells[0].RowIndex;
            var cat = new Category(
                dataGridView1.Rows[row].Cells[0].Value as string,
                dataGridView1.Rows[row].Cells[1].Value as string,
                dataGridView1.Rows[row].Cells[2].Value as string);

            FormCategoryEdit f = new FormCategoryEdit(m_CategoryService.Categories, cat);
            if(f.ShowDialog(this) == DialogResult.OK)
            {
                m_CategoryService.Remove(cat);
                m_CategoryService.Add(f.Category);
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            FormCategoryEdit f = new FormCategoryEdit(m_CategoryService.Categories);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                m_CategoryService.Add(f.Category);
            }

        }
    }
}
