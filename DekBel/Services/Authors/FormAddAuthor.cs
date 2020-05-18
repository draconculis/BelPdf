using Dek.Bel.Core.Models;
using Dek.Bel.Core.Services;
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

namespace Dek.Bel.Services.Authors
{
    public partial class FormAddAuthor : Form
    {
        [Import] public IDBService m_DBService { get; set; }
        [Import] public AuthorService m_AuthorService { get; set; }


        AuthorWithType SelectedAuthor = null;
        IEnumerable<Author> m_Authors { get; set; }

        public FormAddAuthor()
        {
            if (m_DBService == null)
                Mef.Compose(this);

            InitializeComponent();
        }

        private void LoadAuthors()
        {
            dataGridView1.DataSource = null;
            m_Authors = m_DBService.Select<Author>();
            dataGridView1.DataSource = m_Authors;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {

        }
    }
}
