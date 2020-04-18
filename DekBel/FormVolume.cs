using Dek.Bel.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Dek.DB;
using Dek.Bel.Core.DB;
using Dek.Bel.Core.Services;
using Dek.Cls;

namespace Dek.Bel
{
    /// <summary>
    /// Opened from Dek Bel GUI
    /// </summary>

    public partial class FormVolume : Form
    {
        [Import] public IDBService m_DBService { get; set; }
        [Import] public HistoryRepo m_HistoryRepo { get; set; }
        [Import] public VolumeService m_VolumeService { get; set; }

        #region References and listbox databindings ========================================

        private const string ListboxItem_AllTitle = "All";
        List<Book> m_Books;
        List<Chapter> m_Chapters;
        List<SubChapter> m_SubChapters;
        List<Paragraph> m_Paragraphs;

        List<Book> Books
        {
            get
            {
                if (m_VolumeService == null)
                    return null;

                if (m_Books == null)
                    m_Books = InitReference(m_VolumeService.Books);

                return m_Books;
            }
        }

        List<Chapter> Chapters
        {
            get
            {
                if (m_VolumeService == null)
                    return null;

                if (m_Chapters == null)
                    m_Chapters = InitReference(m_VolumeService.Chapters);

                return m_Chapters;
            }
        }

        List<SubChapter> SubChapters
        {
            get
            {
                if (m_VolumeService == null)
                    return null;

                if (m_SubChapters == null)
                    m_SubChapters = InitReference(m_VolumeService.SubChapters);

                return m_SubChapters;
            }
        }

        List<Paragraph> Paragraphs
        {
            get
            {
                if (m_VolumeService == null)
                    return null;

                if (m_Paragraphs == null)
                    m_Paragraphs = InitReference(m_VolumeService.Paragraphs);

                return m_Paragraphs;
            }
        }

        /// <summary>
        /// Populate the list with an 'All' item and the contents of the volume.
        /// </summary>
        List<T> InitReference<T>(List<T> theVolumeList) where T : Reference, new()
        {
            var theList = new List<T> { new T { Title = $"{ListboxItem_AllTitle} {new T().GetType().Name}s" } };

            theList.AddRange(theVolumeList);

            return theList;
        }

        #endregion References and listbox databindings =====================================

        // Currently selected
        Volume CurrentVolume => m_VolumeService?.CurrentVolume;
        Book SelectedBook { get; set; }
        Chapter SelectedChapter { get; set; }
        SubChapter SelectedSubChapter { get; set; }
        Paragraph SelectedParagraph { get; set; }

        public Citation SelectedCitation { get; set; }
        public List<CitationWithReferences> Citations { get; set; }

        public FormVolume()
        {
            InitializeComponent();

            if (m_DBService == null)
                Mef.Initialize(this, new List<Type> { GetType(), typeof(ModelsForViewing) });

        }

        private void FormVolume_Load(object sender, EventArgs e)
        {
            var lastHistory = m_HistoryRepo.GetLastOpened();
            var volume = m_DBService.SelectById<Volume>(lastHistory.VolumeId);
            m_VolumeService.LoadVolume(volume.Id);

            textBox_title.Text = CurrentVolume.Title;

            listBox_books.DataSource = Books;
            listBox_chapters.DataSource = Chapters;
            listBox_subchapters.DataSource = SubChapters;
            listBox_paragraphs.DataSource = Paragraphs;

            listBox_books.SelectedIndex = 0;
            listBox_chapters.SelectedIndex = 0;
            listBox_subchapters.SelectedIndex = 0;
            listBox_paragraphs.SelectedIndex = 0;

            Citations = m_VolumeService.GetCitationWithReferences();
            LoadCitationControl();
        }

        void LoadCitationControl()
        {
            if (Citations == null || !Citations.Any())
                return;

            List<CitationWithReferences> filteredCitations = Citations
                .Where(a => (listBox_books.SelectedIndex < 1) || a.Book?.Id == ((Book)listBox_books.SelectedItem).Id)
                .Where(b => (listBox_chapters.SelectedIndex < 1) || b.Chapter?.Id == ((Chapter)listBox_chapters.SelectedItem).Id)
                .Where(c => (listBox_subchapters.SelectedIndex < 1) || c.SubChapter?.Id == ((SubChapter)listBox_subchapters.SelectedItem).Id)
                .Where(d => (listBox_paragraphs.SelectedIndex < 1) || d.Paragraph?.Id == ((Paragraph)listBox_paragraphs.SelectedItem).Id)
                .ToList();

            listBox_CitationsWithReferences.Items.Clear();
            foreach (var c in filteredCitations)
                listBox_CitationsWithReferences.Items.Add(c);
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Button_selectAndClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            SelectedCitation = listBox_CitationsWithReferences.SelectedItem as Citation;
            Close();
        }

        private void TextBox_title_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_title.Text))
                return;

            CurrentVolume.Title = textBox_title.Text;
            m_DBService.InsertOrUpdate(CurrentVolume);
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedBook = (Book)listBox_books.SelectedItem;
            SelectedChapter = (Chapter)listBox_chapters.SelectedItem;
            SelectedSubChapter = (SubChapter)listBox_subchapters.SelectedItem;
            SelectedParagraph = (Paragraph)listBox_paragraphs.SelectedItem;
            LoadCitationControl();
        }
    }
}
