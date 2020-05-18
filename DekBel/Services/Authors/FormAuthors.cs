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
            var currentAuthor = (Author)dataGridView1.SelectedRows[0].DataBoundItem;
            if (currentAuthor == null)
                return;

            var fe = new FormEditAuthor(currentAuthor);
            fe.ShowDialog();

            if (fe.Author.Id.IsNull)
                return;

            m_DBService.InsertOrUpdate(fe.Author);

            LoadAuthors();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            var currentAuthor = (Author)dataGridView1.SelectedRows[0].DataBoundItem;
            if (currentAuthor == null)
                return;

            m_DBService.Delete(currentAuthor);

            LoadAuthors();
        }
    }
}
