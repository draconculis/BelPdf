using Dek.Bel.Cls;
using Dek.Bel.DB;
using Dek.Bel.Models;
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
    /// <summary>
    /// Opened from Dek Bel GUI
    /// </summary>

    public partial class FormVolume : Form
    {
        [Import] public IDBService m_DBService { get; set; }
        [Import] public VolumeService m_VolumeService { get; set; }

        private const string ListboxItem_AllTitle = "All";

        // Currently selected
        Book CurrentBook { get; set; }
        Chapter CurrentChapter { get; set; }
        SubChapter CurrentSubchaoter { get; set; }
        Paragraph CurrentParagraph { get; set; }

        public FormVolume()
        {
            InitializeComponent();

            if (m_DBService == null)
                Mef.Initialize(this);

        }

        private void FormVolume_Load(object sender, EventArgs e)
        {
            textBox_title.Text = m_VolumeService.CurrentVolume.Title;

        }

        void LodListBoxes()
        {
            listBox_books.Items.Add(new Book
            {
                Id = Id.Null,
                Title = ListboxItem_AllTitle,
            });

            foreach(var book in m_VolumeService.Books)
            {
                listBox_books.Items.Add(book);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Button_selectAndClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
