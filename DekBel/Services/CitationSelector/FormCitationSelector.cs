using Dek.Cls;
using Dek.Bel.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Dek.Bel.Core.Services;

namespace Dek.Bel.CitationSelector
{
    public partial class FormCitationSelector : Form
    {
        public Citation SelectedCitation { get; set; }
        public bool Cancel { get; set; } = true;

        public readonly ModelsForViewing VM;
        
        private readonly VolumeService m_VolumeService;
        private readonly ICategoryService m_CategoryService;
        private readonly CitationDeleterService m_CitationDeleterService;

        // Column constants-----
        private const int COLUMN_ID = 0;
        //----------------------

        public List<CitationSelectorModel> m_Citations { get; set; }

        public IEnumerable<CitationSelectorModel> m_FilteredCitations => m_Citations.Where(c =>
            (!checkBox_hideCategorized.Checked || c.HasMainCategory)).ToList();
        
        public FormCitationSelector(ModelsForViewing vm,
                    VolumeService volumeService,
                    ICategoryService categoryService,
                    CitationDeleterService citationDeleterService
            ) : this()
        {
            VM = vm;
            m_CategoryService = categoryService;
            m_CitationDeleterService = citationDeleterService;
            m_VolumeService = volumeService;

            if (!m_VolumeService.Citations.Any())
                return;

            SelectedCitation = vm.CurrentCitation ?? m_VolumeService.Citations.FirstOrDefault();
            LoadCitations();
            dataGridView1.DataSource = m_FilteredCitations;
            UpdateCount();
            if(!SelectRowById(SelectedCitation.Id))
                SelectRow(0);
        }

        public FormCitationSelector()
        {
            InitializeComponent();
        }

        void LoadCitations()
        {
            if (m_Citations == null)
                m_Citations = new List<CitationSelectorModel>();

            m_Citations.Clear();

            foreach(Citation cit in m_VolumeService.Citations)
            {
                m_Citations.Add(new CitationSelectorModel(cit)
                {
                    MainCategory = m_CategoryService.GetMainCategory(cit.Id),
                    MainCitationCategory = m_CategoryService.GetMainCitationCategory(cit.Id),
                });
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = m_FilteredCitations;
        }

        private bool SelectRowById(Id id)
        {
            if ((dataGridView1.Rows?.Count ?? 0) < 1)
                return false;

            bool found = false;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Index < 0) // archaic shit
                    continue;

                CitationSelectorModel cit = (CitationSelectorModel)row.DataBoundItem;
                if ((cit?.Model.Id ?? Id.Empty) == id) // everything is possible
                {
                    SelectRow(row.Index);
                    found = true;
                    break;
                }
            }

            return found;
        }

        private void SelectRow(int idx)
        {
            dataGridView1.ClearSelection();
            dataGridView1.Rows[idx].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[idx].Cells[0];

            CitationSelectorModel cit = (CitationSelectorModel)dataGridView1.Rows[idx].DataBoundItem;
            SelectedCitation = (Citation)cit.Model;
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
            Cancel = false;
            Close();
        }

        private void CheckBox_hideCategorized_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_FilteredCitations;
            UpdateCount();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1 && dataGridView1.SelectedCells.Count < 1)
                return;
            
            // Some stuff to get selected rows from the collection of selected cells...
            var selectedCells = new List<DataGridViewCell>();
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                selectedCells.Add(cell);

            IEnumerable<int> selectedRowIdxs = selectedCells.Select(c => c.RowIndex).Distinct();
            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if(selectedRowIdxs.Contains(row.Index))
                    selectedRows.Add(row);

            Citation oldCitation = SelectedCitation;

            var citationsToDelete = new List<Citation>();
            foreach (DataGridViewRow row in selectedRows)
                citationsToDelete.Add(((CitationSelectorModel)row.DataBoundItem).Model);

            // Delete. This might mess things up.
            m_CitationDeleterService.DeleteCitationsById(citationsToDelete.Select(c => c.Id));

            m_VolumeService.LoadCitations();

            if (!m_VolumeService.Citations.Any())
            {
                SelectedCitation = null;
                return;
            }

            if (m_VolumeService.Citations.Any(c => c.Id == oldCitation.Id))
                SelectRowById(oldCitation.Id);
            else
                SelectRowById(m_VolumeService.Citations.First().Id);

            LoadCitations();
            UpdateCount();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedCitation = GetSelectedCitationFromGrid();
        }

        private Citation GetSelectedCitationFromGrid()
        {
            Citation result = null;
            var row = dataGridView1.CurrentRow;
            if (row != null && row.Index - 1 >= 0)
            {
                Id id = ((CitationSelectorModel)row.DataBoundItem)?.Id ?? Id.Empty;
                if (id.IsNotNull)
                {
                    result = m_VolumeService.Citations.SingleOrDefault(c => c.Id == id);
                }
            }
            
            if (result == null)
                result = VM.CurrentCitation;

            if (result == null)
                result = m_VolumeService.Citations.FirstOrDefault();

            return result;
        }
    }
}
