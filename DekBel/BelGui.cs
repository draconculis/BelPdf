using BelManagedLib;
using Dek.Bel.Services;
using Dek.Bel.Cls;
using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dek.Bel.Services.Report;
using System.Text.RegularExpressions;
using Dek.Bel.Helpers;
using Dek.Bel.CitationSelector;

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

        [Import] public IMessageboxService m_MessageboxService { get; set; }
        [Import] public ModelsForViewing VM { get; set; }
        [Import] public VolumeService m_VolumeService { get; set; }
        [Import] public ICategoryService m_CategoryService { get; set; }
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public CategoryRepo m_CategoryRepo { get; set; }
        private StorageService m_StorageService { get; } = new StorageService();
        [Import] public HistoryRepo m_HistoryRepo { get; set; }
        [Import] public CitationService m_CitationService { get; set; }
        [Import] public RichTextService RtfService { get; set; }
        [Import] public IPdfService PdfService { get; set; }
        [Import] public CitationManipulationService m_CitationManipulationService { get; set; }
        [Import] public CitationSelectorService m_CitationSelectorService { get; set; }
        [Import] public DatabaseAdminService m_DatabaseAdminService { get; set; }

        private bool LoadingControls = false;

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
            History history = m_HistoryRepo.GetLastOpened(); // Our currently open file in Sumatra
            m_VolumeService.LoadVolume(history.VolumeId);
            VM.CurrentStorage = m_DBService.SelectById<Storage>(history.StorageId);

            if (message.Code == (int)InterOp.CodesEnum.DEKBELCODE_SHOWBEL)
            {
                m_VolumeService.LoadCitations(history.VolumeId);
                if (m_VolumeService.Citations.Count > 1)
                {
                    VM.CurrentCitation = SelectCitation();
                }
                else if (m_VolumeService.Citations.Count < 1)
                {
                    MessageBox.Show(null, "There are no citations for the current volume.", "No citations found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else
                {
                    VM.CurrentCitation = m_VolumeService.Citations?.FirstOrDefault();
                }

                if (VM.CurrentCitation == null)
                {
                    VM.CurrentCitation = m_VolumeService.Citations?.FirstOrDefault();
                }
            }
            else if (message.Code == (int)InterOp.CodesEnum.DEKBELCODE_ADDANDSHOWCITATION)
            {
                List<RawCitation> rawCitations = m_CitationService.GetRawCitations(history.VolumeId).ToList();
                VM.CurrentCitation = m_CitationService.CreateNewCitation(rawCitations, message, m_VolumeService.CurrentVolume.Id);
                m_VolumeService.LoadCitations(history.VolumeId);
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
            }

            m_DBService.DeleteAll<RawCitation>(); // These need to go now

            m_CitationManipulationService.CitationChanged += OnCitationChanged;
            comboBox_CategoryWeight.SelectedIndex = 2;
        }


        private void BelGui_Load(object sender, EventArgs e)
        {
            LoadingControls = true;

            label_citationNotes.Font = new Font(Font, FontStyle.Bold);
            label_citationVolume.Font = new Font(Font, FontStyle.Bold);

            Font font = m_UserSettingsService.CitationFont;

            richTextBox1.Font = font;
            richTextBox2.Font = font;

            // Init margin box dropdowns
            PdfMarginBoxSettings.LoadAComboBoxWithPdfFonts(comboBox_PdfBoxFont);
            SetComboBoxSelectedIndex(comboBox_PdfBoxFont, Constants.PdfFont.TIMES_ROMAN);
            PdfMarginBoxSettings.LoadAComboBoxWithDisplayModes(comboBox_PdfMarginBoxDisplayMode);
            SetComboBoxSelectedIndex(comboBox_PdfBoxFont, Constants.MarginBoxVisualMode.Normal);

            LoadControls();

            splitContainer2.Focus();
            splitContainer2.Panel2.Focus();
            groupBox1.Focus();

            ActiveControl = textBox_CategorySearch;
            textBox_CategorySearch.Focus();

            
        }

        /// <summary>
        /// Event fired from CitationManipulationService.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCitationChanged(object sender, EventArgs e)
        {
            m_VolumeService.ReloadCitation(VM.CurrentCitation); // Need to update its list of all citations when current citatation changed

            LoadControls();
        }

        // Test
        public BelGui()
        {
            InitializeComponent();
            this.Text = $"Bel {AssemblyStuff.AssemblyVersion}";
            toolStripTextBox1.Text = (string)Properties.Settings.Default["DeselectionMarker"];
        }

        #region Control Loading =========================================
        /// <summary>
        /// Load data into controls
        /// </summary>
        private void LoadControls()
        {
            LoadingControls = true;

            // Show which citation is current
            int len = Math.Min(VM.CurrentCitation.Citation1.Length, 40);
            toolStripStatusLabel_CitationSelector.Text = $"{VM.CurrentCitation.Id.ToStringShort()} - {VM.CurrentCitation.Citation1.Substring(0,len)}";

            // Load data from citation
            VM.InitCitationData();
            richTextBox1.Rtf = RtfService.CreateRtfWithExlusionsAndEmphasis(VM.CurrentCitation.Citation2, VM.Exclusion, null);
            richTextBox2.Rtf = RtfService.CreateRtfWithExlusionsAndEmphasis(VM.CurrentCitation.Citation3, null, VM.Emphasis);

            //label1_MD5.Text = VM.CurrentCitation.Id.ToString();
            label_volumeTitle.Text = m_VolumeService.CurrentVolume.Title;
            label_CitationLength.Text = $"{VM.CurrentCitation.Citation1.Length}";
            label_CitationCreated.Text = $"{VM.CurrentCitation.CreatedDate.ToSaneStringShort()}";
            label_CitationEdited.Text = $"{VM.CurrentCitation.EditedDate.ToSaneStringShort()}";
            textBox_CitationNotes.Text = $"{VM.CurrentCitation.Notes}";

            // Volume data
            textBox_VolumeTitle.Text = m_VolumeService.CurrentVolume.Title;
            textBox_VolumeNotes.Text = m_VolumeService.CurrentVolume.Notes;
            textBox_volumePublicationDate.Text = m_VolumeService.CurrentVolume.PublicationDate.ToSaneStringDateOnly();
            numericUpDown_offsetX.Value = (decimal)m_VolumeService.CurrentVolume.OffsetX;
            numericUpDown_offsetY.Value = (decimal)m_VolumeService.CurrentVolume.OffsetY;


            // Load data from storage
            label_fileName.Text = Path.GetFileName(VM.CurrentStorage.FileName);
            label_storageName.Text = Path.GetFileName(VM.CurrentStorage.StorageName);
            label1_MD5.Text = VM.CurrentStorage.Hash;

            LoadCategoryControl();
            LoadReferences();
            LoadPdfMarginBoxControls();

            toolStripButton_AutoUpdate.Checked = m_UserSettingsService.AutoWritePdfOnClose;

            LoadingControls = false;
        }

        void LoadCategoryControl()
        {
            flowLayoutPanel_Categories.Controls.Clear();
            var cgs = m_CategoryService.CitationCategories(VM.CurrentCitation.Id);
            var categories = m_CategoryService.Categories;

            foreach (var cg in cgs)
            {
                if (cg.CategoryId.IsNull)
                    continue;

                Category cat = categories.SingleOrDefault(x => x.Id == cg.CategoryId);
                if(cat != null)
                    AddCategoryLabel(cg, cat);
            }
        }

        void LoadPdfMarginBoxControls()
        {
            string settingsStr = VM.CurrentCitation.MarginBoxSettings;
            PdfMarginBoxSettings settings = new PdfMarginBoxSettings(settingsStr);

            SetComboBoxSelectedIndex(comboBox_PdfBoxFont, settings.Font);
            SetComboBoxSelectedIndex(comboBox_PdfMarginBoxDisplayMode, settings.DisplayMode);

            checkBox_right.Checked = settings.RightMargin;
            numericUpDown_FontSize.Value = (decimal)settings.FontSize;
            numericUpDown_borderThickness.Value = (decimal)settings.BorderThickness;
            numericUpDown_PdfBoxMargin.Value = (decimal)settings.Margin;
            NumericUpDown_pdfMarginBoxWidth.Value = (decimal)settings.Width;

            // Set pdf colors
            System.Drawing.Color ch, cu, cm;
            Color[] colors = ColorStuff.ConvertStringToColors(VM.CurrentCitation.CitationColors);
            ch = (colors.Length >= 1)
                ? colors[0]
                : m_UserSettingsService.PdfHighLightColor;
            cu = (colors.Length >= 2)
                ? colors[1]
                : m_UserSettingsService.PdfUnderlineColor;
            cm = (colors.Length >= 3)
                ? colors[2]
                : m_UserSettingsService.PdfMarginBoxColor;

            label_PdfHighlightColor.BackColor = ch;
            label_PdfUnderLineColor.ForeColor = cu;
            label_PdfMarginBoxColor.BackColor = cm;

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
            int startPage = m_VolumeService.GetPageNumber(VM.CurrentCitation.PhysicalPageStart);
            int stopPage = m_VolumeService.GetPageNumber(VM.CurrentCitation.PhysicalPageStop);
            label_citationStart.Text = $"Page: {startPage} (physical page: {VM.CurrentCitation.PhysicalPageStart}), Character: {VM.CurrentCitation.GlyphStart}";
            label_CitationStop.Text = $"Page: {stopPage} (physical page: {VM.CurrentCitation.PhysicalPageStop}), Character: {VM.CurrentCitation.GlyphStop}";
        }
        #endregion Load Controls ===========================================

        /// <summary>
        /// Citations dropdown status strip menu click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void CitationsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (!(sender is ToolStripMenuItem item))
        //        return;

        //    Citation citation = m_VolumeService.Citations.SingleOrDefault(x => x.Id == Id.NewId(item.Name));
        //    if(citation == null)
        //    {
        //        MessageBox.Show("Error loading citation");
        //        return;
        //    }

        //    VM.CurrentCitation = citation;
        //    LoadControls();
        //}

        #region Citation text boxes =====================================================

        private void richTextBox2_KeyUp(object sender, KeyEventArgs e)
        {

        }

        #endregion Citation text boxes ==================================================


        #region Toolstrip 1 (top menu row) ==============================================

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = richTextBox1.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
                richTextBox2.Font = fontDialog1.Font;
                m_UserSettingsService.CitationFont = fontDialog1.Font;
            }
        }

        private const float FontScaleFactor = 0.15f;
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Font f = richTextBox1.Font;
            Font newFont = new Font(f.FontFamily, f.Size * (1 + FontScaleFactor), f.Style, f.Unit);
            richTextBox1.Font = newFont;
            richTextBox2.Font = newFont;
            m_UserSettingsService.CitationFont = newFont;
        }


        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Font f = richTextBox1.Font;
            float size = (f.Size * (1 - FontScaleFactor)) <= 4.0f ? 4.0f : f.Size * (1 - FontScaleFactor);
            Font newFont = new Font(f.FontFamily, size, f.Style, f.Unit);
            richTextBox1.Font = newFont;
            richTextBox2.Font = newFont;
            m_UserSettingsService.CitationFont = newFont;
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

        #endregion Toolstrip 1 (top menu row) ===========================================


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
        // *************************************************************************************************
        static string CategoryAddText = "Add";
        static string CategoryCreateText = "Create";

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
                if (button_CategoryAddCreate.Text == CategoryCreateText)
                {
                    Button_CategoryAddCreate_Click(sender, e);
                }
                else
                {
                    if(listBox1.Visible)
                        listBox1_Click(sender, e);
                }

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
            var citCats = m_CategoryService.CitationCategories(VM.CurrentCitation.Id);
            //var cats = m_CategoryService.Categories
            //    .Where(x =>
            //    x.Code.ToLower().Contains(textbox.Text.ToLower())
            //    || x.Name.ToLower().Contains(textbox.Text.ToLower()))
            //    .Where(c => c.Id != Id.Null)
            //    .Where(d => !citCats.Any(f => f.CategoryId == d.Id))
            //    .ToList();

            if(textbox.Text.TrimStart().IndexOf(" ") > 1)
            {
                button_CategoryAddCreate.Text = CategoryCreateText;
                listBox1.Visible = false;
            }
            else
            {
                button_CategoryAddCreate.Text = CategoryAddText;

                var cats = m_CategoryService.Categories
                    .Where(x =>
                        x.Code.ToLower().Contains(textbox.Text.ToLower())
                       || x.Name.ToLower().Contains(textbox.Text.ToLower()))
                    .Where(c => c.Id != Id.Null)
                    .Where(d => !citCats.Any(f => f.CategoryId == d.Id))
                    .ToList();

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

        }


        // Add category
        private void listBox1_Click(object sender, EventArgs e)
        {
            if (!(listBox1.SelectedItem is Category cat))
                return;

            bool hasNullCategory = m_CategoryService.CitationCategories(VM.CurrentCitation.Id).Any(x => x.CitationId == Id.Null);
            bool hasMainCategory = m_CategoryService.CitationCategories(VM.CurrentCitation.Id).Any(x => x.IsMain);

            //bool hasNullCategory = flowLayoutPanel_Categories.Controls.Count == 1
            //    && ((CitationCategory)((Label)flowLayoutPanel_Categories.Controls[0]).Tag).CategoryId == Id.Null;

            bool isMain = (flowLayoutPanel_Categories.Controls.Count == 0) || hasNullCategory || !hasMainCategory;

            //m_CategoryService.AddCategoryToCitation(VM.CurrentCitation.Id, cat.Id, int.Parse((comboBox_CategoryWeight.SelectedItem as string)??"1"), isMain);
            //LoadCategoryControl();

            textBox_CategorySearch.Text = cat.ToString();

            listBox1.Visible = false;
            //textBox_CategorySearch.Text = "";
        }

        private void Button_CategoryAddCreate_Click(object sender, EventArgs e)
        {
            if (button_CategoryAddCreate.Text == CategoryAddText)
            {
                if (!(listBox1.SelectedItem is Category cat))
                    return;

                bool hasNullCategory = m_CategoryService.CitationCategories(VM.CurrentCitation.Id).Any(x => x.CitationId == Id.Null);
                bool hasMainCategory = m_CategoryService.CitationCategories(VM.CurrentCitation.Id).Any(x => x.IsMain);

                bool isMain = (flowLayoutPanel_Categories.Controls.Count == 0) || hasNullCategory || !hasMainCategory;

                m_CategoryService.AddCategoryToCitation(VM.CurrentCitation.Id, cat.Id, int.Parse((comboBox_CategoryWeight.SelectedItem as string) ?? "1"), isMain);
                LoadCategoryControl();

                textBox_CategorySearch.Text = "";
            }
            else
            {
                if (textBox_CategorySearch.Focused)
                    return;

                string s = textBox_CategorySearch.Text.Trim().Replace("-", " ").Replace("   ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
                if (string.IsNullOrWhiteSpace(s))
                    return;

                Regex regex = new Regex(@"^\w+ +\w+$");
                if(!regex.IsMatch(s))
                {
                    MessageBox.Show($"To create new category, please use the following format:{Environment.NewLine}Code Name", "Invalid format");
                    textBox_CategorySearch.Focus();
                    return;
                }

                string[] parts = s.Split(' ');

                // Create new category
                Category newCategory = m_CategoryService.CreateNewCategory(parts[0], parts[1]);
                if(newCategory == null)
                {
                    MessageBox.Show($"New category cannot be added, category with code '{parts[0]}' already exists.", "Category exists");
                    textBox_CategorySearch.Focus();
                    return;
                }

                bool hasNullCategory = m_CategoryService.CitationCategories(VM.CurrentCitation.Id).Any(x => x.CitationId == Id.Null);
                bool hasMainCategory = m_CategoryService.CitationCategories(VM.CurrentCitation.Id).Any(x => x.IsMain);

                bool isMain = (flowLayoutPanel_Categories.Controls.Count == 0) || hasNullCategory || !hasMainCategory;

                m_CategoryService.AddCategoryToCitation(VM.CurrentCitation.Id, newCategory.Id, int.Parse((comboBox_CategoryWeight.SelectedItem as string) ?? "1"), isMain);
                LoadCategoryControl();

                textBox_CategorySearch.Text = "";
            }
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

        // *************************************************************************************************
        #endregion Category logic ==========================================================================

        private void copyGUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // toolstrip

        private void richTextBox2_Enter(object sender, EventArgs e)
        {
            toolStripButton8.Enabled = true;
        }

        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            VM.CurrentCitation.Citation3 = richTextBox2.Text;
            m_VolumeService.SaveAndReloadCitation(VM.CurrentCitation);

            if (toolStripButton8.Selected)
                return;

            toolStripButton8.Enabled = false;
        }

        /// Exclusion
        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            // Exclude
            if (richTextBox1.SelectionLength > 0)
                m_CitationManipulationService.ExcludeSelectedText(richTextBox1.SelectionStart, richTextBox1.SelectionStart + richTextBox1.SelectionLength - 1);
        }

        /// Begin edit
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (VM.CurrentCitation.Citation3.Length > 0)
                if (m_MessageboxService.ShowYesNo("Edited citation will be overwritten. Continue?", "Citation not empty") == DialogResult.No)
                    return;

            m_CitationManipulationService.BeginEdit();
        }

        /// Emphasis
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (richTextBox2.SelectionLength > 0)
                m_CitationManipulationService.AddEmphasis(richTextBox2.SelectionStart, richTextBox2.SelectionStart + richTextBox2.SelectionLength - 1);

            richTextBox2.Focus();
        }


        // --------------------------

        // ContextMenuStrip  Rtb 1 ==============================================
        private void excludeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton8_Click(sender, e);
        }

        private void doneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton7_Click(sender, e);
        }

        private void showOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_CitationManipulationService.ResetCitation2();
        }


        private void CopyToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        // Citation2 - Remove line endings in selected text 
        private void RemoveLineEndingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Exclude
            if (richTextBox1.SelectionLength > 0)
                m_CitationManipulationService.RemoveLinebreakInCitation2(richTextBox1.SelectionStart, richTextBox1.SelectionStart + richTextBox1.SelectionLength - 1);
            else
                m_CitationManipulationService.RemoveLinebreakInCitation2(0, richTextBox1.Text.Length - 1);
        }



        // End contextMenuStrip Rtb 1 ==============================================


        // ContextMenuStrip Rtb 2 ==============================================

        private void EmphasisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripButton8_Click(sender, e);
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Cut();
        }

        private void CopyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox2.Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Paste();
        }

        // End ContextMenuStrip Rtb 2  ==============================================


        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //if (MessageBox.Show($"This will reset the citation in the first box with the original citation text.{Environment.NewLine}Continue and loose exclusions?", "Reset citation to original?", MessageBoxButtons.YesNo) == DialogResult.No)
            //    return;

            m_CitationManipulationService.ResetCitation2();
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
            // Exclude
            if (richTextBox1.SelectionLength > 0)
                m_CitationManipulationService.AdjustSpacesInCitation2(richTextBox1.SelectionStart, richTextBox1.SelectionStart + richTextBox1.SelectionLength - 1);
            else
                m_CitationManipulationService.AdjustSpacesInCitation2(0, richTextBox1.Text.Length - 1);
        }

        /// <summary>
        /// Toolstrip menu item BOLD emphasis
        /// </summary>
        private void BoldEmphasisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item))
                return;

            m_UserSettingsService.BoldEmphasis = item.Checked;
            LoadControls();
        }

        /// <summary>
        /// Toolstrip menu item UNDERLINE emphasis
        /// </summary>
        private void UnderlineEmphasisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item))
                return;

            m_UserSettingsService.UnderlineEmphasis = item.Checked;
            LoadControls();

        }

        private void ToolStripButton_updatePdf_Click(object sender, EventArgs e)
        {
            PdfService.RecreateTheWholeThing(VM, m_VolumeService);
        }

        private void ToolStripButton_AutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            m_UserSettingsService.AutoWritePdfOnClose = toolStripButton_AutoUpdate.Checked;
        }


        private void TextBox_CitationNotes_Leave(object sender, EventArgs e)
        {
            VM.CurrentCitation.Notes = textBox_CitationNotes.Text;
            m_VolumeService.SaveAndReloadCitation(VM.CurrentCitation);
        }

        /// <summary>
        /// Update volume data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_VolumeTitle_Leave(object sender, EventArgs e)
        {
            m_VolumeService.CurrentVolume.Title = textBox_VolumeTitle.Text;
            m_VolumeService.CurrentVolume.PublicationDate = textBox_volumePublicationDate.Text.ToSaneDateTime();
            m_VolumeService.CurrentVolume.Notes = textBox_VolumeNotes.Text;
            m_VolumeService.CurrentVolume.OffsetX = (int)numericUpDown_offsetX.Value;
            m_VolumeService.CurrentVolume.OffsetY = (int)numericUpDown_offsetY.Value;
            m_DBService.InsertOrUpdate(m_VolumeService.CurrentVolume);
            label_volumeTitle.Text = m_VolumeService.CurrentVolume.Title;
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




        #region Colors ===========================================================

        private void Label_PdfHighlightColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = m_UserSettingsService.PdfHighLightColor;
            colorDialog1.ShowDialog();
            m_UserSettingsService.PdfHighLightColor = colorDialog1.Color;
            label_PdfHighlightColor.BackColor = m_UserSettingsService.PdfHighLightColor;

            SaveColors();
        }

        private void Label_PdfUnderLineColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = m_UserSettingsService.PdfUnderlineColor;
            colorDialog1.ShowDialog();
            m_UserSettingsService.PdfUnderlineColor = colorDialog1.Color;
            label_PdfUnderLineColor.ForeColor = m_UserSettingsService.PdfUnderlineColor;

            SaveColors();
        }

        private void Label_PdfMarginColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = m_UserSettingsService.PdfMarginBoxColor;
            colorDialog1.ShowDialog();
            m_UserSettingsService.PdfMarginBoxColor = colorDialog1.Color;
            label_PdfMarginBoxColor.BackColor = m_UserSettingsService.PdfMarginBoxColor;

            SaveColors();
        }

        private void SaveColors()
        {
            VM.CurrentCitation.CitationColors =
                ColorStuff.ConvertColorsToString(label_PdfHighlightColor.BackColor, label_PdfUnderLineColor.ForeColor, label_PdfMarginBoxColor.BackColor);
            m_VolumeService.SaveAndReloadCitation(VM.CurrentCitation);
        }

        #endregion Colors =========================================================

        #region Pdf Margin Box Settings ===========================================

        private void NumericUpDown_borderThickness_Leave(object sender, EventArgs e)
        {
            if (LoadingControls)
                return;

            float val;
            try
            {
                val = (float)numericUpDown_borderThickness.Value;
            }
            catch
            {
                return;
            }

            m_UserSettingsService.PdfMarginBoxBorder = val;
            SaveMarginBoxSettingsToCitation();
        }

        private void SaveMarginBoxSettingsToCitation()
        {
            VM.CurrentCitation.MarginBoxSettings = new PdfMarginBoxSettings(
                m_UserSettingsService.PdfMarginBoxWidth,
                m_UserSettingsService.PdfMarginBoxHeight,
                m_UserSettingsService.PdfMarginBoxMargin,
                m_UserSettingsService.PdfMarginBoxBorder,
                m_UserSettingsService.PdfMarginBoxFont,
                m_UserSettingsService.PdfMarginBoxFontSize,
                m_UserSettingsService.PdfMarginBoxRightMargin,
                m_UserSettingsService.PdfMarginBoxVisualMode
                ).ToString();

            m_VolumeService.SaveAndReloadCitation(VM.CurrentCitation);
        }

        private void comboBox_PdfBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingControls)
                return;

            m_UserSettingsService.PdfMarginBoxFont = comboBox_PdfBoxFont.SelectedItem.ToString();
            SaveMarginBoxSettingsToCitation();
        }



        private void TextBox_pdfMarginBoxFontSize_Leave(object sender, EventArgs e)
        {

        }

        private void ComboBox_PdfMarginBoxDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingControls)
                return;

            m_UserSettingsService.PdfMarginBoxVisualMode = comboBox_PdfMarginBoxDisplayMode.SelectedItem.ToString();
            SaveMarginBoxSettingsToCitation();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            m_UserSettingsService.PdfMarginBoxRightMargin = checkBox_right.Checked;
            SaveMarginBoxSettingsToCitation();
        }

        private void NumericUpDown_PdfBoxMargin_Leave(object sender, EventArgs e)
        {
            if (LoadingControls)
                return;

            int val;
            try
            {
                val = (int)numericUpDown_PdfBoxMargin.Value;
            }
            catch
            {
                return;
            }

            m_UserSettingsService.PdfMarginBoxMargin = val;
            SaveMarginBoxSettingsToCitation();
        }

        private void NumericUpDown_FontSize_Leave(object sender, EventArgs e)
        {
            if (LoadingControls)
                return;

            float val;
            try
            {
                val = (float)numericUpDown_FontSize.Value;
            }
            catch
            {
                return;
            }

            m_UserSettingsService.PdfMarginBoxFontSize = val;
            SaveMarginBoxSettingsToCitation();
        }

        private void NumericUpDown_pdfMarginBoxWidth_Leave(object sender, EventArgs e)
        {
            if (LoadingControls)
                return;

            int val;
            try
            {
                val = (int)NumericUpDown_pdfMarginBoxWidth.Value;
            }
            catch
            {
                return;
            }

            m_UserSettingsService.PdfMarginBoxWidth = val;
            SaveMarginBoxSettingsToCitation();
        }


        #endregion Pdf Margin Box Settings ========================================

        private void Button1_Click_1(object sender, EventArgs e)
        {
            SaveMarginBoxSettingsToCitation();
            PdfService.RecreateTheWholeThing(VM, m_VolumeService);
        }

        private void RemoveLineEndingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveLineEndingsToolStripMenuItem1_Click(sender, e);
        }

        private void SetComboBoxSelectedIndex(ComboBox theComboBox, string text)
        {
            int idx = 0;
            foreach(string item in theComboBox.Items)
            {
                if(item.Equals(text, StringComparison.OrdinalIgnoreCase))
                {
                    theComboBox.SelectedIndex = idx;
                    return;
                }
                idx++;
            }
        }



        // Hamburger ==============================================================================
        #region Hamburger =========================================================================

        private void AuthorsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCategory fc = new FormCategory(m_CategoryService);
            fc.ShowDialog();
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Pops up Edit Raw Citation window
        private void EditRawCitationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RawCitationEditor rced = new RawCitationEditor(VM, m_UserSettingsService.CitationFont);
            rced.Show();
            LoadControls();

            MessageBox.Show("To use the edited raw citation, press 'Reset to original'.", "Raw citation edited.");
        }

        // Generate report
        private void ToolStripMenuItem_Report_Click(object sender, EventArgs e)
        {
            Form_Report fr = new Form_Report();
            fr.Show();
        }



        #endregion Hamburger ======================================================================
        // end Hamburger ==========================================================================


        // Main toolstrip =========================================================================

        // Elipsis - show Volume outline
        private void ToolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            FormVolume fm = new FormVolume();

            if (fm.ShowDialog() == DialogResult.OK || fm.SelectedCitation != VM.CurrentCitation) // Citation might be deleted
            {
                VM.CurrentCitation = fm.SelectedCitation ?? VM.CurrentCitation;
                LoadControls();
            }

        }

        private void BelGui_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_UserSettingsService.AutoWritePdfOnClose)
                PdfService.RecreateTheWholeThing(VM, m_VolumeService);
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            FormCategory fc = new FormCategory(m_CategoryService);
            fc.ShowDialog();
        }

        private void toolStripStatusLabel_CitationSelector_Click(object sender, EventArgs e)
        {
            // Please note that after this call, the number of selected citations may have changed
            // and there may even be zero citations.
            var oldCitation = VM.CurrentCitation;
            VM.CurrentCitation = SelectCitation();
            if(!m_VolumeService.Citations.Any())
            {
                m_MessageboxService.Show("No Citations", "No Citations found for current Volume.");
                Close();
                return;
            }

            if(VM.CurrentCitation == null)
            {
                // Try to reselect previous citation
                if (oldCitation != null && m_VolumeService.Citations.Any(c => c.Id == oldCitation.Id))
                    VM.CurrentCitation = oldCitation;
                else
                    VM.CurrentCitation = m_VolumeService.Citations.First();
            }

            LoadControls();
        }

        private Citation SelectCitation()
        {
            return m_CitationSelectorService.ShowSelector(VM);
        }

        private void StatusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        #region Database admin ============================================================

        /// <summary>
        /// Make backup of database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (bool cancel, string selectedPath) = m_DatabaseAdminService.SelectDatabasePathForBackup();
            if (cancel || String.IsNullOrWhiteSpace(selectedPath))
                return;

            if (!selectedPath.EndsWith(".sqlite"))
                selectedPath += ".sqlite";

            (bool success, string error) = m_DatabaseAdminService.CopyDatabaseFile(m_UserSettingsService.DBPath, selectedPath);
            if(!success)
            {
                MessageBox.Show($"{error}", "Backup database error");
                return;
            }

            var result = MessageBox.Show($"Database sucessfully backed up to {selectedPath}{Environment.NewLine}Show file in folder?", "Success", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,{selectedPath}");
            }

        }

        private void restoreDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (bool cancel, string selectedPath) = m_DatabaseAdminService.SelectDatabasePathForRestore();
            if (cancel)
                return;

            if (!selectedPath.EndsWith(".sqlite"))
                selectedPath += ".sqlite";

            (bool success, string error) = m_DatabaseAdminService.RestoreDatabaseFile(selectedPath);
            if (!success)
            {
                MessageBox.Show($"{error}", "Restore database error");
                return;
            }

            var result = MessageBox.Show($"Database restored from {selectedPath}{Environment.NewLine}Show file in folder?", "Success", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,{selectedPath}");
            }
        }

        private void ShowRestoreWorkaround(string selectedFile)
        {
            var x = Environment.NewLine;
            MessageBox.Show(
                $"Please note!{x}It is currently not possible to restore a database when Bel is running. This is a common known problem with SQlite.{x}" +
                $"The workaround is to close BelSumatra and copy the file back by hand.{x}" +
                $"Open a command prompt and type:{x}" +
                $"",
                "Not working yet");
        }

        #endregion Database admin ==========================================================

        private void BelGui_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveMarginBoxSettingsToCitation();
        }
    }
}
