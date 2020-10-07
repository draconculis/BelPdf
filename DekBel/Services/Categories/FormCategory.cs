using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dek.Cls;
using Dek.Bel.Core.Models;
using Dek.Bel.Core.Services;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace Dek.Bel.Services
{
    public partial class FormCategory : Form
    {
        public ICategoryService m_CategoryService;
        private IEnumerable<Category> m_FilteredCategories => m_CategoryService.Categories
            .Where(c => Filter(c))
            .Where(h => HideUncategorized(h))
            .ToList();
        private Predicate<Category> Filter;
        private Predicate<Category> HideUncategorized = x => x.Id != Id.Null;

        private const int ColorColIdx = 4;

        public event EventHandler<CategoryEventArgs> CategoryChanged;

        public FormCategory(ICategoryService categoryService)
        {
            InitializeComponent();
            m_CategoryService = categoryService;
            Filter = (c) => true;
            dataGridView1.DataSource = m_FilteredCategories;
            dataGridView1.SelectAll();
            dataGridView1.ClearSelection();
            UpdateCount();
            m_CategoryService.CategoryUpdated += OnCategoryUpdated;

            AdjustColumns();
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
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[ColorColIdx].Width = 25;

            int previouswidth = dataGridView1.Columns[1].Width + dataGridView1.Columns[2].Width + dataGridView1.Columns[ColorColIdx].Width;
            dataGridView1.Columns[3].Width = dataGridView1.Width - previouswidth;
            dataGridView1.Columns[3].DefaultCellStyle.BackColor = Color.White;

            dataGridView1.Columns[ColorColIdx].HeaderText = "";
        }

        private void UpdateCount()
        {
            label_count.Text = $"{m_FilteredCategories.Count()} / {m_CategoryService.Categories.Count() - 1}"; // Don't count the ubiquotus 'Uncategorized'
        }
        private void FireCategoryChanged(Category cat)
        {
            CategoryChanged?.Invoke(this, new CategoryEventArgs { Category = cat });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button_done_Click(object sender, EventArgs e)
        {
            this.Visible = false;
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
                Id = Id.NewId(dataGridView1.Rows[row].Cells[0].Value?.ToString()),
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

            FireCategoryChanged(cat);
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
                Id = Id.NewId(dataGridView1.Rows[row].Cells[0].Value?.ToString()),
                Code = dataGridView1.Rows[row].Cells[1].Value as string,
                Name = dataGridView1.Rows[row].Cells[2].Value as string,
                Description = dataGridView1.Rows[row].Cells[3].Value as string
            };

            List<CitationCategory> referencedCitations = m_CategoryService.CitationCategoriesByCategory(cat.Id);

            string idString = string.Join($"{Environment.NewLine}", referencedCitations.Select(x => x.CitationId.ToString()).ToArray());

            if (MessageBox.Show($"Delete category {cat}?" + Environment.NewLine + 
                $"It will be removed from these {referencedCitations.Count} Citations in the current and other Volumes: " + Environment.NewLine + 
                idString, 
                "Delete Category?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    m_CategoryService.Remove(cat, true);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = m_FilteredCategories;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Could not remove category!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                AdjustColumns();
            }

            FireCategoryChanged(cat);
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4)
            //foreach (DataGridViewRow Myrow in dataGridView1.Rows)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                var colors = ColorStuff.ConvertStringToColors((string)row.Cells[ColorColIdx].Value);
                if (colors.Any())
                {
                    row.Cells[ColorColIdx].Style.BackColor = colors[0];
                    row.Cells[ColorColIdx].Style.ForeColor = colors[0];
                    row.Cells[ColorColIdx].Style.SelectionBackColor = colors[0];
                    row.Cells[ColorColIdx].Style.SelectionForeColor = colors[0];
                }
                else
                {
                    row.Cells[ColorColIdx].Style.BackColor = Color.White;
                    row.Cells[ColorColIdx].Style.ForeColor = Color.White;
                    row.Cells[ColorColIdx].Style.SelectionBackColor = Color.White;
                    row.Cells[ColorColIdx].Style.SelectionForeColor = Color.White;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count)
                return;

            if (e.ColumnIndex == 4)
            {
                Id id = (Id)dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                Category cat =  m_FilteredCategories.SingleOrDefault(x => x.Id == id);

                ColorDialog dlg = new ColorDialog();
                Color[] colors = ColorStuff.ConvertStringToColors(cat.CategoryColor);
                dlg.Color = colors.Any() ? colors[0] : Color.White;
                DialogResult result =  dlg.ShowDialog(this);
                if (result == DialogResult.Cancel)
                    return;

                cat.CategoryColor = ColorStuff.ConvertColorsToString(new Color[] { dlg.Color });

                m_CategoryService.InsertOrUpdate(cat);

                FireCategoryChanged(cat);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button_update_Click(sender, e);
        }
    }
}
