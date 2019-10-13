using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Bel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.CitationSelector
{
    public partial class FormCitationSelector : Form
    {
        public Citation SelectedCitation { get; set; }

        public readonly ModelsForViewing VM;
        public readonly VolumeService m_VolumeService;
        public readonly ICategoryService m_CategoryService;


        public List<CitationSelectorModel> m_Citations { get; set; }
        public IEnumerable<CitationSelectorModel> m_FilteredCitations => m_Citations.Where(c =>
            (!checkBox_hideCategorized.Checked || c.HasMainCategory)).ToList();
        
        public FormCitationSelector(ModelsForViewing vm, VolumeService volumeService, ICategoryService categoryService) : this()
        {
            VM = vm;
            m_VolumeService = volumeService;
            m_CategoryService = categoryService;

            SelectedCitation = vm.CurrentCitation;
            LoadCitations();
            dataGridView1.DataSource = m_FilteredCitations;
            UpdateCount();
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        public FormCitationSelector()
        {
            InitializeComponent();
        }

        void LoadCitations()
        {
            if (m_Citations == null)
                m_Citations = new List<CitationSelectorModel>();

            foreach(Citation cit in m_VolumeService.Citations)
            {
                m_Citations.Add(new CitationSelectorModel(cit)
                {
                    MainCategory = m_CategoryService.GetMainCategory(cit.Id),
                    MainCitationCategory = m_CategoryService.GetMainCitationCategory(cit.Id),
                });
            }
        }

        private void FormSelectCitation_Load(object sender, EventArgs e)
        {
        }

        private void UpdateCount()
        {
            label_count.Text = $"{m_FilteredCitations.Count()} / {m_Citations.Count().ToString()}";
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            var row = dataGridView1.CurrentRow;
            if(row != null && row.Index -1 >= 0)
            {
                Id id = ((CitationSelectorModel)row.DataBoundItem)?.Id ?? Id.Empty;
                if (!id.IsNull)
                {
                    SelectedCitation = m_VolumeService.Citations.SingleOrDefault(c => c.Id == id);
                }
            }

            if (SelectedCitation == null)
                SelectedCitation = VM.CurrentCitation;

            if (SelectedCitation == null)
                SelectedCitation = m_VolumeService.Citations.FirstOrDefault();

            Close();
        }

        private void CheckBox_hideCategorized_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_FilteredCitations;
        }
    }
}
