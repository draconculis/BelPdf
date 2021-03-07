using BelManagedLib;
using Dek.Bel.Services;
using Dek.Cls;
using Dek.DB;
using Dek.Bel.Core.Models;
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
using Dek.Bel.DB;
using Dek.Bel.Core.GUI;
using Dek.Bel.Core.Services;
using Dek.Bel.Core.DB;
using Dek.Bel.Core.Helpers;
using Dek.Bel.Core.ViewModels;
using Dek.Bel.Core.Cls;
using System.Diagnostics;

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
        [Import] public CategoryLabelService m_CategoryLabelService { get; set; }
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public HistoryRepo m_HistoryRepo { get; set; }
        [Import] public CitationService m_CitationService { get; set; }
        [Import] public RichTextService RtfService { get; set; }
        [Import] public IPdfService PdfService { get; set; }
        [Import] public CitationManipulationService m_CitationManipulationService { get; set; }
        [Import] public CitationSelectorService m_CitationSelectorService { get; set; }
        [Import] public DatabaseAdminService m_DatabaseAdminService { get; set; }
        [Import] public DatabaseAdminServiceSaveOpen m_DatabaseAdminServiceGUI { get; set; }
        [Import] public CitationPersisterService m_CitationPersisterService { get; set; }
        [Import] public SeriesService m_SeriesService { get; set; }
        [Import] public AuthorService m_AuthorService { get; set; }

        #region ctor ============================================================

        /// <summary>
        /// Coming in here means from outer space: We are called from Sumatra.
        /// </summary>
        /// <param name="message"></param>
        public BelGui(EventData message) : this()
        {
            LoadingControls = true;

            if (m_DBService == null)
                Mef.Initialize(this, new List<Type> { GetType(), typeof(ModelsForViewing) });

            VM.CurrentCitationChanged += VM_CurrentCitationChanged;
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
                    Citation newCitation  = SelectCitation();
                    if (newCitation == null)
                    {
                        VM.CurrentCitation = m_VolumeService.Citations.First();
                    }
                    else
                    {
                        VM.CurrentCitation = newCitation;
                    }
                }
                else if (m_VolumeService.Citations.Count < 1)
                {
                    MessageBox.Show(null, "There are no citations for the current volume.", "No citations found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                    return;
                }
                else // Exactly one citation found
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
                catch //(Exception ex)
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
            m_CitationPersisterService.TimeToSave += CitationPersisterService_TimeToSave;
            m_CitationPersisterService.Rtb = richTextBox2;
        }

        // Test
        public BelGui()
        {
            InitializeComponent();
            this.Text = $"Bel {AssemblyStuff.AssemblyVersion}";
            toolStripTextBox1.Text = (string)Properties.Settings.Default["DeselectionMarker"];
        }

        #endregion ctor =========================================================


        #region BelGui Form Events ==============================================

        private void BelGui_Load(object sender, EventArgs e)
        {
            LoadingControls = true;

            label_citationNotes.Font = new Font(Font, FontStyle.Bold);
            label_citationVolume.Font = new Font(Font, FontStyle.Bold);

            Font font = m_UserSettingsService.CitationFont;

            richTextBox1.Font = font;
            richTextBox2.Font = font;
            richTextBox1.SetInnerMargins(12, 10, 28, 10);
            richTextBox2.SetInnerMargins(12, 10, 28, 10);

            // Init margin box dropdowns
            GuiHelper.LoadAComboBoxWithPdfFonts(comboBox_PdfBoxFont);
            BelGuiHelper.SetComboBoxSelectedIndex(comboBox_PdfBoxFont, Constants.PdfFont.TIMES_ROMAN);
            GuiHelper.LoadAComboBoxWithDisplayModes(comboBox_PdfMarginBoxDisplayMode);
            BelGuiHelper.SetComboBoxSelectedIndex(comboBox_PdfBoxFont, Constants.MarginBoxVisualMode.Normal);

            LoadControls();
            LoadingControls = true;

            splitContainer2.Focus();
            splitContainer2.Panel2.Focus();

            ActiveControl = categoryUserControl1;

            OldLeft = Left;
            OldTop = Top;

            LoadingControls = false;
        }

        /// <summary>
        /// Event fired from VM when current citation changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VM_CurrentCitationChanged(object sender, EventArgs e)
        {
            OnCitationChanged(sender, e);
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

        private void BelGui_FormClosing(object sender, FormClosingEventArgs e)
        {
            richTextBox2.Focus();

            SaveMarginBoxSettingsToCitation();

            if (m_UserSettingsService.AutoWritePdfOnClose)
                PdfService.RecreateTheWholeThing(VM, m_VolumeService);
        }

        private void BelGui_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        #endregion BelGui Form Events ===========================================


        #region Control Loading =========================================

        private bool LoadingControls = false;

        /// <summary>
        /// Load data into controls
        /// </summary>
        private void LoadControls()
        {
            LoadingControls = true;

            // Category user control needs to know what current citation is
            categoryUserControl1.CurrentCitationId = VM.CurrentCitation.Id;

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
            textBox_volumePublicationDate.Text = m_VolumeService.CurrentVolume.PublicationDate;//.ToSaneStringDateOnly();
            numericUpDown_offsetX.Value = (decimal)m_VolumeService.CurrentVolume.OffsetX;
            numericUpDown_offsetY.Value = (decimal)m_VolumeService.CurrentVolume.OffsetY;

            // Series
            LoadSeries();
            LoadingControls = true;

            //Authors
            LoadAuthors();
            LoadingControls = true;

            // Load data from storage
            label_fileName.Text = Path.GetFileName(VM.CurrentStorage.FileName);
            label_storageName.Text = Path.GetFileName(VM.CurrentStorage.StorageName);
            label1_MD5.Text = VM.CurrentStorage.Hash;

            categoryUserControl1.Update();
            LoadReferences();
            LoadPdfMarginBoxControls();
            LoadingControls = true;

            toolStripButton_AutoUpdate.Checked = m_UserSettingsService.AutoWritePdfOnClose;

            LoadingControls = false;
        }

        //void LoadCategoryControl()
        //{
        //    LoadingControls = true;
        //    flowLayoutPanel_Categories.Controls.Clear();
        //    var cgs = m_CategoryService.CitationCategoriesByCitation(VM.CurrentCitation.Id);
        //    var categories = m_CategoryService.Categories;

        //    foreach (var cg in cgs)
        //    {
        //        if (cg.CategoryId.IsNull)
        //            continue;

        //        Category cat = categories.SingleOrDefault(x => x.Id == cg.CategoryId);
        //        if(cat != null)
        //            AddCategoryLabel(cg, cat);
        //    }

        //    LoadingControls = false;
        //}

        void LoadPdfMarginBoxControls()
        {
            LoadingControls = true;

            string settingsStr = VM.CurrentCitation.MarginBoxSettings;
            PdfMarginBoxSettings settings = new PdfMarginBoxSettings(settingsStr);

            BelGuiHelper.SetComboBoxSelectedIndex(comboBox_PdfBoxFont, settings.Font);
            BelGuiHelper.SetComboBoxSelectedIndex(comboBox_PdfMarginBoxDisplayMode, settings.DisplayMode);

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

            LoadingControls = false;
        }

        void LoadReferences()
        {
            LoadingControls = true;

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

            LoadingControls = false;
        }

        List<Volume> m_VolumesInSeries = null;
        void LoadSeries()
        {
            LoadingControls = true;

            Series series = m_SeriesService.GetSeriesForVolume(VM.CurrentCitation.VolumeId);
            if(series == null)
            {
                label_SeriesName.Text = "This Volume does not belong to a Series.";
                textBox_SeriesNote.Text = "";
                listBox_VolumesInSeries.DataSource = null;
                return;
            }

            label_SeriesName.Text = series.Name;
            textBox_SeriesNote.Text = series.Notes;
            m_VolumesInSeries = m_SeriesService.GetOtherVolumesInSeriesByVolumeId(VM.CurrentCitation.VolumeId).ToList();
            listBox_VolumesInSeries.DataSource = m_VolumesInSeries;

            LoadingControls = false;
        }

        List<AuthorsGridViewModel> m_AuthorGridViewModels;
        void LoadAuthors()
        {
            LoadingControls = true;
            // Set up grid

            // Load
            LoadingControls = false;
        }

        #endregion Load Controls ===========================================


        #region Toolstrip 1 (top menu row) ==============================================

        private void ToolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            FormVolume fm = new FormVolume();

            if (fm.ShowDialog() == DialogResult.OK || fm.SelectedCitation != VM.CurrentCitation) // Citation might be deleted
            {
                VM.CurrentCitation = fm.SelectedCitation ?? VM.CurrentCitation;
                LoadControls();
            }

        }

        private void flipOrientationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Orientation = (splitContainer1.Orientation == Orientation.Horizontal) ? Orientation.Vertical : Orientation.Horizontal;

            // Adjustr splitter
            if (splitContainer1.Orientation == Orientation.Horizontal)
                splitContainer1.SplitterDistance = (splitContainer1.Height) / 2;
            else
                splitContainer1.SplitterDistance = (splitContainer1.Width) / 2;
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

        //private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (((sender as ToolStripMenuItem)?.Owner as ContextMenuStrip)?.SourceControl is Label label)
        //    {
        //        var cc = (CitationCategory)label.Tag;
        //        m_DBService.Delete<CitationCategory>($"`{nameof(CitationCategory.CitationId)}`='{cc.CitationId}' AND `{nameof(CitationCategory.CategoryId)}`='{cc.CategoryId}'");
        //        flowLayoutPanel_Categories.Controls.Remove(label);
        //    }
        //}

        #endregion Toolstrip 1 (top menu row) ===========================================


        #region Toolstrip 1 =====================================================

        private void ToolStripButton_updatePdf_Click(object sender, EventArgs e)
        {
            PdfService.RecreateTheWholeThing(VM, m_VolumeService);
        }

        private void ToolStripButton_AutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            m_UserSettingsService.AutoWritePdfOnClose = toolStripButton_AutoUpdate.Checked;
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
                if (m_MessageboxService.ShowYesNo("Edited citation will be overwritten. Continue?", "Citation not empty") == DekDialogResult.No)
                    return;

            splitContainer1.SplitterDistance = 0;

            m_CitationManipulationService.BeginEdit();
        }

        /// Emphasis
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            var currentCursorPos = richTextBox2.SelectionStart;

            if (richTextBox2.SelectionLength > 0)
                m_CitationManipulationService.AddEmphasis(richTextBox2.SelectionStart, richTextBox2.SelectionStart + richTextBox2.SelectionLength - 1);

            richTextBox2.Focus();
            richTextBox2.SelectionStart = currentCursorPos;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //if (MessageBox.Show($"This will reset the citation in the first box with the original citation text.{Environment.NewLine}Continue and loose exclusions?", "Reset citation to original?", MessageBoxButtons.YesNo) == DialogResult.No)
            //    return;

            ResetCitation2();
        }

        /// <summary>
        /// Citation2 is shown in first rtf box. This makes citation2 = citation1 (the original citation)
        /// </summary>
        public void ResetCitation2()
        {
            var resetform = new Form_ResetCitation(VM.CurrentCitation.Citation1, m_UserSettingsService.CitationFont);

            if (resetform.ShowDialog() == DialogResult.Yes)
            {
                VM.CurrentCitation.Citation2 = VM.CurrentCitation.Citation1;
                VM.Exclusion.Clear();
                VM.CurrentCitation.Exclusion = null;
                m_DBService.InsertOrUpdate(VM.CurrentCitation);

                m_CitationManipulationService.FireCitationChanged();
            }
        }

        private void RemoveLineEndingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveLineEndingsToolStripMenuItem1_Click(sender, e);
        }

        private void resetToOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetCitation2();
        }

        private void toolStripStatusLabel_CitationSelector_Click(object sender, EventArgs e)
        {
            var oldCitation = VM.CurrentCitation;
            // Please note that after this call, the number of selected citations may have changed
            // and there may even be zero citations.
            Citation newCitation = SelectCitation();
            
            if (!m_VolumeService.Citations.Any())
            {
                m_MessageboxService.Show(
                    "No Citations", 
                    $"No Citations found for current Volume. {Environment.NewLine}Bel must now close!");
                Close();
                return;
            }

            if (newCitation != null)
            {
                VM.CurrentCitation = newCitation;
            }
            else
            {
                // Try to reselect previous citation
                if (oldCitation != null && m_VolumeService.Citations.Any(c => c.Id == oldCitation.Id))
                    VM.CurrentCitation = oldCitation;
                else
                    VM.CurrentCitation = m_VolumeService.Citations.First();
            }

            LoadControls();
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

        #endregion Toolstrip 1 ==================================================


        #region Rtb1 (Citation 2) ===============================================

        private void richTextBox1_SizeChanged(object sender, EventArgs e)
        {
            if (!(sender is RichTextBox rtb))
                return;

            rtb.Invalidate();
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


        #endregion Rtb1 (Citation 2) ============================================


        #region Rtb2 (Citation 3) ================================================

        private void richTextBox2_Enter(object sender, EventArgs e)
        {
            toolStripButton_Emphasis.Enabled = true;
        }

        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            VM.CurrentCitation.Citation3 = richTextBox2.Text;
            m_VolumeService.SaveAndReloadCitation(VM.CurrentCitation);

            if (toolStripButton_Emphasis.Selected)
                return;

            toolStripButton_Emphasis.Enabled = false;
        }

        private void CitationPersisterService_TimeToSave(object sender, CitationPersisterService.TimeToSaveEventArgs e)
        {
            if (e.TheControl == richTextBox2)
            {
                VM.CurrentCitation.Citation3 = richTextBox2.Text;
                m_DBService.InsertOrUpdate(VM.CurrentCitation);
                //richTextBox2.BackColor = Color.White;
            }
        }

        #endregion Rtb2 (Citation 3) =============================================


        #region RichTextBox margins =============================================

        private void marginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var margins = richTextBox1.GetInnerMargins();
            FormMargins f = new FormMargins(margins.left, margins.top, margins.right, margins.bottom);
            if (f.ShowDialog(this) == DialogResult.Cancel)
                return;

            richTextBox1.SetInnerMargins(f.LeftMargin, f.TopMargin, f.RightMargin, f.BottomMargin);
            richTextBox2.SetInnerMargins(f.LeftMargin, f.TopMargin, f.RightMargin, f.BottomMargin);
        }

        #endregion RichTextBox margins ==========================================


        #region ContextMenuStrip Rtb 1 ==========================================

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
            ResetCitation2();
        }

        // Remove line endings in selected text 
        private void RemoveLineEndingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Exclude
            if (richTextBox1.SelectionLength > 0)
                m_CitationManipulationService.RemoveLinebreakInCitation2(richTextBox1.SelectionStart, richTextBox1.SelectionStart + richTextBox1.SelectionLength - 1);
            else
                m_CitationManipulationService.RemoveLinebreakInCitation2(0, richTextBox1.Text.Length - 1);
        }

        private void CopyToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        #endregion ==============================================================


        #region ContextMenuStrip Rtb 2 ==========================================

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
            Clipboard.SetText(richTextBox2.Text);
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Paste();
        }

        #endregion ContextMenuStrip Rtb 2 =======================================

        // **** TABS ************************************************************

        #region TAB CITATION ====================================================

        private void TextBox_CitationNotes_Leave(object sender, EventArgs e)
        {
            VM.CurrentCitation.Notes = textBox_CitationNotes.Text;
            m_VolumeService.SaveAndReloadCitation(VM.CurrentCitation);
        }

        #endregion ==============================================================


        #region TAB SERIES ======================================================

        private void button_EditSeries_Click(object sender, EventArgs e)
        {

        }

        private void button_Series_Click(object sender, EventArgs e)
        {
            VolumeSeries volumeSeries = m_SeriesService.GetVolumeSeriesByVolumeId(VM.CurrentCitation.VolumeId);

            FormSeries f = new FormSeries(VM.CurrentCitation.VolumeId);
            f.ShowDialog();

            if (f.SelectedSeries == null && volumeSeries == null)
            {
                return; // Safe to return, nothing happened
            }
            if (f.SelectedSeries == null && volumeSeries != null)
            {
                m_SeriesService.DetachVolumeFromSeries(VM.CurrentCitation.VolumeId);
                LoadSeries();
                return;
            }

            if (volumeSeries == null)
            {
                // Selected somthing new
                m_SeriesService.AttachVolumeToSeries(VM.CurrentCitation.VolumeId, f.SelectedSeries.Id);
            }
            else
            {
                // Selected somthing new
                m_SeriesService.DetachVolumeFromSeries(VM.CurrentCitation.VolumeId);
                m_SeriesService.AttachVolumeToSeries(VM.CurrentCitation.VolumeId, f.SelectedSeries.Id);
            }

            LoadSeries();
        }


        #endregion ==============================================================


        #region TAB VOLUME ======================================================

        /// <summary>
        /// Update volume data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_VolumeTitle_Leave(object sender, EventArgs e)
        {
            m_VolumeService.CurrentVolume.Title = textBox_VolumeTitle.Text;
            m_VolumeService.CurrentVolume.PublicationDate = textBox_volumePublicationDate.Text;//.ToSaneDateTime();
            m_VolumeService.CurrentVolume.ISBN = textBox_ISBN.Text;
            m_VolumeService.CurrentVolume.Notes = textBox_VolumeNotes.Text;
            m_VolumeService.CurrentVolume.OffsetX = (int)numericUpDown_offsetX.Value;
            m_VolumeService.CurrentVolume.OffsetY = (int)numericUpDown_offsetY.Value;
            m_DBService.InsertOrUpdate(m_VolumeService.CurrentVolume);
            label_volumeTitle.Text = m_VolumeService.CurrentVolume.Title;
        }


        #endregion ==============================================================


        #region TAB BOOKS =======================================================


        #endregion ==============================================================


        #region TAB AUTHORS =====================================================

        private void button_authorEdit_Click(object sender, EventArgs e)
        {
            FormAuthors fa = new FormAuthors();
            fa.ShowDialog();
        }

        private void button_authorAdd_Click(object sender, EventArgs e)
        {

        }

        private void button_authorRemove_Click(object sender, EventArgs e)
        {

        }

        #endregion ==============================================================


        #region TAB FILE ========================================================
        #endregion ==============================================================

        //**** TABS *************************************************************


        #region Category UserControl Logic ======================================

        private void categoryUserControl1_CategoryChanged(object sender, EventArgs e)
        {

        }

        #endregion Category UserControl Logic ===================================


        #region Colors ==========================================================

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

        private void button_CopyCategoryColor_Click(object sender, EventArgs e)
        {
            Category cat = m_CategoryService.GetMainCategory(VM.CurrentCitation.Id);
            if (cat == null)
                return;
            if (string.IsNullOrEmpty(cat.CategoryColor) || cat.CategoryColor.Length < 3)
                return;

            Color[] colors = ColorStuff.ConvertStringToColors(cat.CategoryColor);
            if (!colors.Any())
                return;

            m_UserSettingsService.PdfMarginBoxColor = colors[0];
            label_PdfMarginBoxColor.BackColor = m_UserSettingsService.PdfMarginBoxColor;
        }

        private void SaveColors()
        {
            VM.CurrentCitation.CitationColors =
                ColorStuff.ConvertColorsToString(label_PdfHighlightColor.BackColor, label_PdfUnderLineColor.ForeColor, label_PdfMarginBoxColor.BackColor);
            m_VolumeService.SaveAndReloadCitation(VM.CurrentCitation);
        }

        #endregion Colors =======================================================


        #region Pdf Margin Box Settings =========================================

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
            if (LoadingControls)
                return;
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
            if (LoadingControls)
                return;

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


        #endregion Pdf Margin Box Settings ======================================


        #region Hamburger =======================================================

        private void AuthorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAuthors fa = new FormAuthors();
            fa.ShowDialog();
        }

        FormCategory m_FormCategory = null;
        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_FormCategory == null)
            {
                m_FormCategory = new FormCategory(m_CategoryService);
                m_FormCategory.Show(this);
                m_FormCategory.CategoryChanged += OnCategoryChagedInFormCategory;
                m_FormCategory.FormClosed += (_, __) =>
                {
                    m_FormCategory.CategoryChanged -= OnCategoryChagedInFormCategory;
                    m_FormCategory = null;
                };
            }
            else
                m_FormCategory.Visible = !m_FormCategory.Visible;
        }

        private void OnCategoryChagedInFormCategory(object sender, CategoryEventArgs e)
        {
            categoryUserControl1.Update();
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Pops up Edit Raw Citation window
        private void EditRawCitationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RawCitationEditor rced = new RawCitationEditor(m_DBService, VM, m_UserSettingsService.CitationFont);
            rced.ShowDialog(this);
            LoadControls();

            MessageBox.Show("To use the edited raw citation, press 'Reset to original'.", "Raw citation edited.");
        }

        // Generate report
        private void ToolStripMenuItem_Report_Click(object sender, EventArgs e)
        {
            Form_Report fr = new Form_Report(m_VolumeService.CurrentVolume.Id);
            fr.ShowDialog(this);

            if (fr.SelectedCitationId.IsNull)
                return;

            // We got a live one
            var oldCitation = VM.CurrentCitation;
            Citation newCitation = m_CitationService.GetCitation(m_VolumeService.CurrentVolume.Id, fr.SelectedCitationId);
            if (newCitation is null)
            {
                MessageBox.Show($"Could not select citation with id {newCitation.Id}.");
                return;
            }

            if (oldCitation.VolumeId != newCitation.VolumeId)
            {
                MessageBox.Show($"Cannot select a citation from a different Volume!{Environment.NewLine}Current volume is {m_VolumeService.CurrentVolume.Title}.", "Different Volume");
                return;
            }

            VM.CurrentCitation = newCitation;
            LoadControls();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout f = new FormAbout();

            f.ShowDialog(this);
        }

        private void showVolumeOutlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click_2(sender, e);
        }

        #endregion Hamburger ====================================================


        #region Database admin ==================================================

        /// <summary>
        /// Make backup of database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (bool cancel, string selectedPath) = m_DatabaseAdminServiceGUI.SelectDatabasePathForBackup();
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
            (bool cancel, string selectedPath) = m_DatabaseAdminServiceGUI.SelectDatabasePathForRestore();
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

        #endregion Database admin ===============================================


        #region Outline =========================================================

        bool FormOutlineOpen = false;
        Form_Outline FormOutline;

        /// <summary>
        /// Show Outline
        /// </summary>
        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {
            if (FormOutlineOpen)
            {
                FormOutline.Close();
                FormOutline = null;
                return;
            }

            FormOutline = new Form_Outline(m_VolumeService, VM.CurrentCitation.Id, Left - 250, Top, 250, Height);
            FormOutline.FormClosing += Form_Outline_FormClosing;
            FormOutline.CitationSelected += FormOutline_CitationSelected;
            FormOutline.Owner = this;
            FormOutlineOpen = true;
            FormOutline.Show();
        }

        private void FormOutline_CitationSelected(object sender, EventArgs e)
        {
            VM.CurrentCitation = FormOutline.SelectedCitation ?? VM.CurrentCitation;
            LoadControls();
        }

        private void Form_Outline_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormOutlineOpen = false;
            FormOutline.FormClosing -= Form_Outline_FormClosing;
            FormOutline.CitationSelected -= FormOutline_CitationSelected;
        }


        int OldLeft;
        int OldTop;
        private void BelGui_Move(object sender, EventArgs e)
        {
            if (FormOutline == null)
                return;

            FormOutline.Top += (Top - OldTop);
            FormOutline.Left += (Left - OldLeft);
            //FormOutline.Top = Top;
            //FormOutline.Left = Left - FormOutline.Width;

            OldLeft = Left;
            OldTop = Top;
        }

        #endregion Outline ======================================================


        private Citation SelectCitation()
        {
            return m_CitationSelectorService.ShowSelector(VM);
        }

    }
}
