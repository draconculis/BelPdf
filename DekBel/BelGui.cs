using BelManagedLib;
using Dek.Bel.Services;
using Dek.Bel.Cls;
using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel
{
    public partial class BelGui : Form
    {
        public ResultData Result { get; set; }
            = new ResultData
            {
                Message = "Hullo",
                Code = 123,
                Cancel = false,
            };

        [Import] public ModelsForViewing VM { get; set; }
        [Import] public VolumeService m_VolumeService { get; set; }
        [Import] public ICategoryService m_CategoryService { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public CategoryRepo m_CategoryRepo { get; set; }
        private StorageService m_StorageService { get; } = new StorageService();
        [Import] public HistoryRepo HistoryRepo { get; set; }
        [Import] public CitationService m_CitationService { get; set; }
        [Import] public RichTextService RtfService { get; set; }
        [Import] public PdfService PdfService { get; set; }
        [Import] public CitationManipulationService m_CitationManipulationService{ get; set;}


        /// <summary>
        /// Coming in here means add a new citation. We are called from Sumatra.
        /// </summary>
        /// <param name="message"></param>
        public BelGui(EventData message) : this()
        {
            if (m_DBService == null)
                Mef.Initialize(this);

            VM.Message = message;

            // Get volume and storage
            History history = HistoryRepo.GetLastOpened(); // Our currently open file in Sumatra
            m_VolumeService.LoadVolume(history.VolumeId);
            VM.CurrentStorage = m_DBService.SelectById<Storage>(history.StorageId);

            if (message.Code == (int)InterOp.CodesEnum.DEKBELCODE_SHOWBEL)
            {
                m_VolumeService.LoadCitations(history.VolumeId);
                VM.CurrentCitation = m_VolumeService.Citations?.FirstOrDefault();
                if(VM.CurrentCitation == null)
                {
                    MessageBox.Show(null, "There are no citations for the current volume.", "No citations found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                LoadCitations();
            }
            else if (message.Code == (int)InterOp.CodesEnum.DEKBELCODE_ADDANDSHOWCITATION)
            {
                List<RawCitation> rawCitations = m_DBService.Select<RawCitation>();
                VM.CurrentCitation = m_CitationService.CreateNewCitation(rawCitations, message, m_VolumeService.CurrentVolume.Id);
                m_VolumeService.LoadCitations(history.VolumeId);
                LoadCitations(); // Into status strip citations context menu
            }
            else if (message.Code == (int)InterOp.CodesEnum.DEKBELCODE_EDITCITATION)
            {
                m_VolumeService.LoadCitations(history.VolumeId);
                string cmd = message.Text;
                string citationId;
                if(string.IsNullOrEmpty(cmd))
                {
                    MessageBox.Show(null, "Something went wrong trying to edit citation: Text was empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                try
                {
                    citationId = cmd.Split(':')[1];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(null, $"Something went wrong trying to edit citation. {Environment.NewLine}Cmd: {cmd}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                VM.CurrentCitation = m_VolumeService.Citations?.SingleOrDefault(x => x.Id.ToString() == citationId);
                if (VM.CurrentCitation == null)
                {
                    MessageBox.Show(null, $"Something went wrong trying to edit citation: Could not load citation {citationId}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                LoadCitations();
            }

            m_DBService.DeleteAll<RawCitation>(); // These need to go now

            m_CitationManipulationService.CitationChanged += CitationService_CitationChanged;
            m_CategoryService.LoadCategoriesFromDb();
            comboBox_CategoryWeight.SelectedIndex = 2;
            LoadControls();
        }

        private void CreateCitation()
        {

        }

        private void EditCitation()
        {

        }

        private void CitationService_CitationChanged(object sender, EventArgs e)
        {
            LoadControls();
        }

        // Test
        public BelGui()
        {
            InitializeComponent();
            this.Text = $"Bel {AssemblyStuff.AssemblyVersion}";
            toolStripTextBox1.Text = (string)Properties.Settings.Default["DeselectionMarker"];
        }

        /// <summary>
        /// Load data into controls
        /// </summary>
        private void LoadControls()
        {
            // Show which citation is current
            toolStripDropDownButton_Citation.Text = VM.CurrentCitation.ToString();

            // Load data from citation
            richTextBox1.Rtf = RtfService.CreateRtfWithExlusionsAndEmphasis(VM.CurrentCitation.Citation2, VM.Exclusion, null);
            richTextBox2.Rtf = RtfService.CreateRtfWithExlusionsAndEmphasis(VM.CurrentCitation.Citation3, null, VM.Emphasis);

            //label1_MD5.Text = VM.CurrentCitation.Id.ToString();

            label_CitationLength.Text = $"{VM.CurrentCitation.Citation1.Length}";
            label_CitationCreated.Text = $"{VM.CurrentCitation.CreatedDate.ToSaneStringShort()}";
            label_CitationEdited.Text = $"{VM.CurrentCitation.EditedDate.ToSaneStringShort()}";
            textBox_CitationNotes.Text = $"{VM.CurrentCitation.Notes}";

            // Volume data
            textBox_VolumeTitle.Text = m_VolumeService.CurrentVolume.Title;
            textBox_VolumeNotes.Text = m_VolumeService.CurrentVolume.Notes;
            textBox_volumePublicationDate.Text = m_VolumeService.CurrentVolume.PublicationDate.ToSaneStringDateOnly();


            // Load data from storage
            label_fileName.Text = Path.GetFileName(VM.CurrentStorage.FileName);
            label_storageName.Text = Path.GetFileName(VM.CurrentStorage.StorageName);
            label1_MD5.Text = VM.CurrentStorage.Hash;

            LoadCategoryControl();
            LoadReferences();
        }

        void LoadCategoryControl()
        {
            flowLayoutPanel_Categories.Controls.Clear();
            var cgs = m_CategoryService.GetCitationCategories(VM.CurrentCitation.Id);
            var categories = m_CategoryService.Categories;

            foreach (var cg in cgs)
            {
                Category cat = categories.SingleOrDefault(x => x.Id == cg.CategoryId);
                if(cat != null)
                    AddCategoryLabel(cg, cat);
            }
        }

        void LoadReferences()
        {
            // Book
            textBox_Book.Text = m_VolumeService.GetBook(VM.CurrentCitation.PhysicalPageStart, VM.CurrentCitation.GlyphStart)?.Title ?? "-";

            // Chapter
            textBox_Chapter.Text = m_VolumeService.GetChapter(VM.CurrentCitation.PhysicalPageStart, VM.CurrentCitation.GlyphStart)?.Title ?? "-";

            // Subchapter
            textBox_SubChapter.Text = m_VolumeService.GetSubChapter(VM.CurrentCitation.PhysicalPageStart, VM.CurrentCitation.GlyphStart)?.Title ?? "-";

            // Paragraph
            textBox_Paragraph.Text = m_VolumeService.GetParagraph(VM.CurrentCitation.PhysicalPageStart, VM.CurrentCitation.GlyphStart)?.Title ?? "-";

            // Page
            int startPage = m_VolumeService.GetPageNumber(VM.CurrentCitation.PhysicalPageStart, VM.CurrentCitation.GlyphStart);
            int stopPage = m_VolumeService.GetPageNumber(VM.CurrentCitation.PhysicalPageStop, VM.CurrentCitation.GlyphStop);
            label_citationStart.Text = $"Page: {startPage} (physical page: {VM.CurrentCitation.PhysicalPageStart}), Character: {VM.CurrentCitation.GlyphStart}";
            label_CitationStop.Text = $"Page: {stopPage} (physical page: {VM.CurrentCitation.PhysicalPageStop}), Character: {VM.CurrentCitation.GlyphStop}";
        }

        /// <summary>
        /// Populate citations into status strip contextMenuStrip_Citations
        /// </summary>
        void LoadCitations()
        {
            contextMenuStrip_Citations.Items.Clear();

            foreach(var citation in m_VolumeService.Citations)
            {
                var item = new ToolStripMenuItem(citation.ToString(), null, CitationsToolStripMenuItem_Click, citation.Id.ToString());
                contextMenuStrip_Citations.Items.Add(item);
            }
        }
        
        /// <summary>
        /// Citations dropdown status strip menu click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CitationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item))
                return;

            Citation citation = m_VolumeService.Citations.SingleOrDefault(x => x.Id == Id.NewId(item.Name));
            if(citation == null)
            {
                MessageBox.Show("Error loading citation");
                return;
            }

            VM.CurrentCitation = citation;
            LoadControls();
        }


        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void BelGui_Load(object sender, EventArgs e)
        {
            Font font = UserSettingsService.CitationFont;

            richTextBox1.Font = font;
            richTextBox2.Font = font;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = richTextBox1.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
                richTextBox2.Font = fontDialog1.Font;
                UserSettingsService.CitationFont = fontDialog1.Font;
            }
        }

        private const float FontScaleFactor = 0.15f;
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Font f = richTextBox1.Font;
            Font newFont = new Font(f.FontFamily, f.Size * (1 + FontScaleFactor), f.Style, f.Unit);
            richTextBox1.Font = newFont;
            richTextBox2.Font = newFont;
            UserSettingsService.CitationFont = newFont;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Font f = richTextBox1.Font;
            float size = (f.Size * (1 - FontScaleFactor)) <= 4.0f ? 4.0f : f.Size * (1 - FontScaleFactor);
            Font newFont = new Font(f.FontFamily, size, f.Style, f.Unit);
            richTextBox1.Font = newFont;
            richTextBox2.Font = newFont;
            UserSettingsService.CitationFont = newFont;
        }


        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void showTextRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{new DekRange(richTextBox1.SelectionStart, richTextBox1.SelectionLength, true)}");

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = "…";
            Properties.Settings.Default["DeselectionMarker"] = toolStripTextBox1.Text;
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Properties.Settings.Default["DeselectionMarker"] = toolStripTextBox1.Text;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((sender as ToolStripMenuItem)?.Owner as ContextMenuStrip)?.SourceControl is Label label)
            {
                var cc = (CitationCategory)label.Tag;
                m_DBService.Delete<CitationCategory>($"`{nameof(CitationCategory.CitationId)}`='{cc.CitationId}' AND `{nameof(CitationCategory.CategoryId)}`='{cc.CategoryId}'");
                flowLayoutPanel_Categories.Controls.Remove(label);
            }
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCategory fc = new FormCategory(m_CategoryService);
            fc.ShowDialog();
        }

        private void button_category_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitContainer2_MouseClick(object sender, MouseEventArgs e)
        {
            var split = sender as SplitContainer;
            

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
        }

        private void statusStrip1_Click(object sender, EventArgs e)
        {
            //if (toolStripDropDownButton1.Pressed)
            //    return;

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        #region Category logic =============================================================================

        private void setAsMainCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((sender as ToolStripMenuItem)?.Owner as ContextMenuStrip)?.SourceControl is Label label)
            {
                m_CategoryService.SetMainCategory(label.Tag as CitationCategory);
                m_CategoryService.ClearMainStyleFromLabels(flowLayoutPanel_Categories.Controls.OfType<Label>());
                m_CategoryService.SetMainStyleOnLabel(label);
            }
        }

        private void textBox1_CategorySearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_CategorySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            //if (string.IsNullOrWhiteSpace(textBox_CategorySearch.Text))
            //    return;
        }

        private void textBox1_CategorySearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox textbox))
                return;

            if (e.KeyCode == Keys.Return)
            {
                listBox1_Click(sender, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Down && listBox1.Visible)
            {
                if (listBox1.SelectedIndex + 1 == listBox1.Items.Count)
                    return;

                listBox1.SelectedIndex++;

                return;
            }

            if (e.KeyCode == Keys.Up && listBox1.Visible)
            {
                if (listBox1.SelectedIndex == 0)
                    return;

                listBox1.SelectedIndex--;

                return;
            }

            listBox1.Items.Clear();
            var cats = m_CategoryService.Categories
                .Where(x => 
                x.Code.ToLower().Contains(textbox.Text.ToLower()) 
                || x.Name.ToLower().Contains(textbox.Text.ToLower())).ToList();

            if (cats.Count < 1 || textbox.Text.Length < 2)
            {
                listBox1.Visible = false;
                return;
            }

            foreach (var c in cats)
                listBox1.Items.Add(c);

            listBox1.SelectedIndex = 0;
            if (!listBox1.Visible)
            {
                listBox1.SelectedIndex = 0;
                listBox1.Top = textbox.Top + textbox.Height;
                listBox1.Left = textbox.Left;
                listBox1.Width = textbox.Width;
                listBox1.Visible = true;
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (!(listBox1.SelectedItem is Category cat))
                return;

            bool isMain = flowLayoutPanel_Categories.Controls.Count == 0;

            m_CategoryService.AddCategoryToCitation(VM.CurrentCitation.Id, cat.Id, int.Parse((comboBox_CategoryWeight.SelectedItem as string)??"1"), isMain);
            LoadCategoryControl();

            listBox1.Visible = false;
            textBox_CategorySearch.Text = "";
        }

        private void textBox_CategorySearch_TextChanged(object sender, EventArgs e)
        {
        }

        // Called from load controls
        private void AddCategoryLabel(CitationCategory citationCat, Category cat)
        {
            Label l = m_CategoryService.CreateCategoryLabelControl(citationCat, cat, contextMenuStrip_Category, toolTip1);

            if (citationCat.IsMain)
                m_CategoryService.SetMainStyleOnLabel(l);

            flowLayoutPanel_Categories.Controls.Add(l);
            textBox_CategorySearch.Focus();
        }

        // Weights
        private void SetWeight1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 1);
        }

        private void SetWeight2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 2);
        }

        private void SetWeight3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 3);
        }

        private void SetWeight4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 4);
        }

        private void SetWeight5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
                SetWeight(item, 5);
        }

        private void SetWeight(ToolStripMenuItem item, int weight)
        {
            if (!(((System.Windows.Forms.ContextMenuStrip)item.GetCurrentParent()).SourceControl.Tag is CitationCategory citationCategory))
                return;

            m_CategoryService.SetWeight(VM.CurrentCitation.Id, citationCategory.CategoryId, weight);
            LoadCategoryControl();
        }

        #endregion Category logic ==============================================================

        private void copyGUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // toolstrip

        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            // Exclude
            if(richTextBox1.SelectionLength > 0)
                m_CitationManipulationService.ExcludeSelectedText(richTextBox1.SelectionStart, richTextBox1.SelectionStart + richTextBox1.SelectionLength - 1);
        }

        private void richTextBox2_Enter(object sender, EventArgs e)
        {
            toolStripButton8.Enabled = true;
        }

        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            if (toolStripButton8.Selected)
                return;

            toolStripButton8.Enabled = false;

            VM.CurrentCitation.Citation3 = richTextBox2.Text;

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            m_CitationManipulationService.BeginEdit();
        }

        /// <summary>
        /// Copies rtf 1 -> rtf 2
        /// </summary>
        public void BeginEdit()
        {
            // Begin edit - e.g copy rtf2 to rtf3
            string text = VM.CurrentCitation.Citation2;
            StringBuilder sb = new StringBuilder();
            bool excluded = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (!VM.Exclusion.ContainsInteger(i) && excluded)
                {
                    excluded = false;
                    sb.Append(" ");
                    sb.Append(UserSettingsService.DeselectionMarker);
                    sb.Append(" ");
                }

                if (VM.Exclusion.ContainsInteger(i))
                {
                    excluded = true;
                    continue;
                }

                sb.Append(text[i]);
            }

            VM.CurrentCitation.Citation3 = sb.ToString();
            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            LoadControls();

        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (richTextBox2.SelectionLength > 0)
                m_CitationManipulationService.AddEmphasis(richTextBox2.SelectionStart, richTextBox2.SelectionStart + richTextBox2.SelectionLength - 1);

            richTextBox2.Focus();
        }


        // --------------------------

        private void excludeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void emphasisToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void doneToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_CitationManipulationService.ResetCitation2();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            m_CitationManipulationService.ResetCitation2();
        }

        private void LeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void SplitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DeselectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void RichTextBox1_Leave(object sender, EventArgs e)
        {
            if (toolStripButton6.Selected)
                return;

            toolStripButton6.Enabled = false;
        }

        private void RichTextBox1_Enter(object sender, EventArgs e)
        {
            toolStripButton6.Enabled = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            PdfService.ManipulatePdf(Path.Combine(UserSettingsService.StorageFolder, VM.CurrentStorage.StorageName), VM.CurrentCitation.SelectionRects);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var mainCitCat = m_CategoryService.GetCitationCategories(VM.CurrentCitation.Id).Where(x => x.IsMain).SingleOrDefault();
            var mainCategory = (mainCitCat != null) 
                ? m_CategoryService.Categories.Where(x => x.Id == mainCitCat.CategoryId).SingleOrDefault()
                : null;
            PdfService.AddCitationToPdfDoc(VM.CurrentStorage.StorageName, VM.CurrentCitation.PhysicalPageStart, mainCategory, mainCitCat, VM.CurrentCitation.SelectionRects);
        }

        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
            if (splitContainer2.Panel2Collapsed)
                toolStripStatusLabel1.Image = global::Dek.Bel.Properties.Resources.metaopen;
            else
                toolStripStatusLabel1.Image = global::Dek.Bel.Properties.Resources.metaclose;
        }

        private void ToolStripDropDownButton_Citation_Click(object sender, EventArgs e)
        {

        }

        private void TextBox_Book_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Remove duplicate spaces from rtf box 1 + 2
        /// </summary>
        private void AdjustSpacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VM.CurrentCitation.Citation2 = m_CitationManipulationService.AdjustSpaces(VM.CurrentCitation.Citation2);
            VM.CurrentCitation.Citation3 = m_CitationManipulationService.AdjustSpaces(VM.CurrentCitation.Citation3);
            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            LoadControls();
        }

        /// <summary>
        /// Toolstrip menu item BOLD emphasis
        /// </summary>
        private void BoldEmphasisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item))
                return;

            UserSettingsService.BoldEmphasis = item.Checked;
            LoadControls();
        }

        /// <summary>
        /// Toolstrip menu item UNDERLINE emphasis
        /// </summary>
        private void UnderlineEmphasisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item))
                return;

            UserSettingsService.UnderlineEmphasis = item.Checked;
            LoadControls();

        }

        private void TextBox_CitationNotes_Leave(object sender, EventArgs e)
        {
            VM.CurrentCitation.Notes = textBox_CitationNotes.Text;
            m_DBService.InsertOrUpdate(VM.CurrentCitation);
        }

        /// <summary>
        /// Update volume data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_VolumeTitle_Leave(object sender, EventArgs e)
        {
            m_VolumeService.CurrentVolume.Title = textBox_VolumeTitle.Text;
            m_VolumeService.CurrentVolume.PublicationDate = textBox_volumePublicationDate.Text.ToSaneDateTime();
            m_VolumeService.CurrentVolume.Notes = textBox_VolumeNotes.Text;
            m_DBService.InsertOrUpdate(m_VolumeService.CurrentVolume);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            FormVolume fm = new FormVolume();
            fm.ShowDialog();
        }


        private void TextChanged_ValidateTextBoxDate(object sender, EventArgs e)
        {
            if (!(sender is TextBox tb))
                return;

            if (tb.Text.IsValidSaneDateTime())
                tb.BackColor = textBox_VolumeTitle.BackColor;
            else
                tb.BackColor = Color.LightPink;
        }

        private void ToolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            FormVolume fm = new FormVolume();

            if(fm.ShowDialog() == DialogResult.OK)
            {
                VM.CurrentCitation = fm.SelectedCitation ?? VM.CurrentCitation;
                LoadControls();
            }

        }



        // --------------------------
        /*************************************************
         
        ***************************************************/


    }
}
