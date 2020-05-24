using Dek.Bel.Core.Models;
using Dek.Bel.Core.Services;
using Dek.Bel.Services.Authors;
using Dek.Cls;
using Dek.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel
{
    public partial class FormAuthors : Form
    {
        [Import] public IDBService m_DBService { get; set; }
        [Import] public AuthorService m_AuthorService { get; set; }

        IEnumerable<Author> m_Authors;

        public FormAuthors()
        {
            if (m_DBService == null)
                Mef.Compose(this);

            InitializeComponent();

            LoadAuthors();
        }

        private void LoadAuthors()
        {
            dataGridView1.DataSource = null;
            m_Authors = m_DBService.Select<Author>();
            dataGridView1.DataSource = m_Authors;

            dataGridView1.Columns[0].Visible = false;
        }

        private void FormAuthors_Load(object sender, EventArgs e)
        {

        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_new_Click(object sender, EventArgs e)
        {
            var fe = new FormEditAuthor();
            fe.ShowDialog();

            if (fe.Author.Id.IsNull)
                return;

            m_DBService.InsertOrUpdate(fe.Author);

            LoadAuthors();
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;

            int row = dataGridView1.SelectedCells[0].RowIndex;
            var currentAuthor = new Author
            {
                Id = new Id(dataGridView1.Rows[row].Cells[0].Value.ToString()),
                Name = dataGridView1.Rows[row].Cells[1].Value as string,
                Born = dataGridView1.Rows[row].Cells[2].Value as string,
                Dead = dataGridView1.Rows[row].Cells[3].Value as string,
                Notes = dataGridView1.Rows[row].Cells[4].Value as string,
            };

            var fe = new FormEditAuthor(currentAuthor);
            if (fe.ShowDialog(this) == DialogResult.OK)
            {
                m_DBService.InsertOrUpdate(fe.Author);

                LoadAuthors();
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;

            int row = dataGridView1.SelectedCells[0].RowIndex;
            var currentAuthor = new Author
            {
                Id = new Id(dataGridView1.Rows[row].Cells[0].Value.ToString()),
                Name = dataGridView1.Rows[row].Cells[1].Value as string,
                Born = dataGridView1.Rows[row].Cells[2].Value as string,
                Dead = dataGridView1.Rows[row].Cells[3].Value as string,
                Notes = dataGridView1.Rows[row].Cells[4].Value as string,
            };

            if (MessageBox.Show($"Author {currentAuthor.Name} will be deleted (and removed from all Series, Volumes and Books).{Environment.NewLine}", "Delete Author?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    m_AuthorService.DetachAuthor(currentAuthor.Id);
                    m_AuthorService.RemoveAuthor(currentAuthor.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Could not remove author {currentAuthor.Name}!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadAuthors();
        }
    }
}
