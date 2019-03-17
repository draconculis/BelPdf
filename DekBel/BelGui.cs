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
        [Import] public ICategoryService CategoryService { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }
        [Import] public IDBService DBService { get; set; }
        [Import] public CategoryRepo CategoryRepo { get; set; }
        private StorageService StorageService { get; } = new StorageService();
        [Import] public HistoryRepo HistoryRepo { get; set; }
        [Import] public CitationRepo CitationRepo { get; set; }
        [Import] public RichTextService RtfService { get; set; }
        [Import] public PdfService PdfService { get; set; }
        [Import] public CitationManipulationService CitationService{ get; set;}


        /// <summary>
        /// Coming in here means add a new citation. We are called from Sumatra.
        /// </summary>
        /// <param name="message"></param>
        public BelGui(EventData message) : this()
        {
            if (DBService == null)
                Mef.Initialize(this);

            List<RawCitation> rawCitations = DBService.Select<RawCitation>();
            DBService.ClearTable<RawCitation>();

            // Get volume and storage
            History history = HistoryRepo.GetLastOpened(); // Our currently open file in Sumatra
            VM.CurrentVolume = DBService.SelectById<Volume>(history.VolumeId);
            VM.CurrentStorage = DBService.SelectById<Storage>(history.StorageId);
                       
            // Create new citation
            VM.CurrentCitation = CitationRepo.CreateNewCitation(rawCitations, message);

            CitationService.CitationChanged += CitationService_CitationChanged;
            LoadControls();
        }

        void LoadCitation()
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

            toolStripTextBox1.Text = (string)Properties.Settings.Default["DeselectionMarker"];
        }

        /// <summary>
        /// Load data
        /// </summary>
        private void LoadControls()
        {
            // Load data from citation
            richTextBox1.Rtf = RtfService.CreateRtfWithExlusionsAndEmphasis(VM.CurrentCitation.Citation2, VM.Exclusion, null);
            richTextBox2.Rtf = RtfService.CreateRtfWithExlusionsAndEmphasis(VM.CurrentCitation.Citation3, null, VM.Emphasis);
            int maxlen = 50;
            toolStripStatusLabel_GUID.Text = VM.CurrentCitation.Id.ToString() + " ";
            toolStripStatusLabel_citationPreview.Font = new Font(richTextBox1.Font.FontFamily, toolStripStatusLabel_GUID.Font.Size);
            toolStripStatusLabel_citationPreview.Text = VM.CurrentCitation.Citation1.Substring(0, VM.CurrentCitation.Citation1.Length > maxlen ? maxlen : VM.CurrentCitation.Citation1.Length) + $"{(VM.CurrentCitation.Citation1.Length > maxlen ? "…" : "")}";
            label1_MD5.Text = Guid.NewGuid().ToString();

            // Load data from storage
            label_fileName.Text = Path.GetFileName(VM.Message.FilePath);
            //textBox_FileName = Path.GetFileName(messsage.FilePath);
            label_Page.Text = $"{VM.Message.StartPage} - {VM.Message.StopPage}";

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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void showTextRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{new TextRange(richTextBox1.SelectionStart, richTextBox1.SelectionLength, true)}");

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

        private void AddCategoryLabel(Category cat)
        {
            // TODO: ADD TO DB
            Label l = CategoryService.CreateCategoryLabelControl(cat.ToString(), false, contextMenuStrip_Category);
            
            if (flowLayoutPanel_Categories.Controls.Count < 1)
                CategoryService.SetMainStyleOnLabel(l);

            flowLayoutPanel_Categories.Controls.Add(l);
            textBox_CategorySearch.Focus();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((sender as ToolStripMenuItem)?.Owner as ContextMenuStrip)?.SourceControl is Label label)
                flowLayoutPanel_Categories.Controls.Remove(label);
        }

        private void setAsMainCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((sender as ToolStripMenuItem)?.Owner as ContextMenuStrip)?.SourceControl is Label label)
            {
                CategoryService.ClearMainStyleFromLabels(flowLayoutPanel_Categories.Controls.OfType<Label>());
                CategoryService.SetMainStyleOnLabel(label);
            }
        }

        private void textBox1_CategorySearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_CategorySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_CategorySearch.Text))
                return;

            //if (e.KeyCode == Keys.Return)
            //{
            //    e.Handled = true;
            //    e.SuppressKeyPress = true;
            //    AddCategoryLabel();
            //}
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
            var cats = CategoryRepo.SearchCategoriesByNameOrCode(textbox.Text);
            if(cats.Count < 1)
            {
                listBox1.Visible = false;
                return;
            }

            foreach(var c in cats)
                listBox1.Items.Add(c);

            listBox1.SelectedIndex = 0;
            if (!listBox1.Visible)
            {
                listBox1.SelectedIndex = 0;
                listBox1.Top = textbox.Top + textbox.Height;
                listBox1.Left = textbox.Left;
                listBox1.Width = textbox.Width + button_category.Width;
                listBox1.Visible = true;
            }
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCategory fc = new FormCategory(CategoryService);
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
            if (toolStripDropDownButton1.Pressed)
                return;

            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
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

        #region Category logic ------------------------------------------------------
        // Category
        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
        }

        private void comboBox2_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (!(listBox1.SelectedItem is Category cat))
                return;

            AddCategoryLabel(cat);

            listBox1.Visible = false;
            textBox_CategorySearch.Text = "";
        }

        private void textBox_CategorySearch_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion Category logic ------------------------------------------------------



        private void copyGUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // toolstrip

        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            // Exclude
            if(richTextBox1.SelectionLength > 0)
                CitationService.ExcludeSelectedText(richTextBox1.SelectionStart, richTextBox1.SelectionStart + richTextBox1.SelectionLength - 1);
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
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            // Begin edit - e.g copy rtf2 to rtf3
            string text = VM.CurrentCitation.Citation2;
            StringBuilder sb = new StringBuilder();
            bool excluded = false;
            for (int i = 0; i < text.Length; i++)
            {
                if(!VM.Exclusion.ContainsInteger(i) && excluded)
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
            DBService.InsertOrUpdate(VM.CurrentCitation);
            LoadControls();
        }


        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (richTextBox2.SelectionLength > 0)
                CitationService.AddEmphasis(richTextBox2.SelectionStart, richTextBox2.SelectionStart + richTextBox2.SelectionLength - 1);

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
            CitationService.ResetCitation2();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            CitationService.ResetCitation2();
        }

        private void LeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // richTextBox1.
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
            PdfService.AddAnnotation(VM.CurrentStorage.StorageName, VM.CurrentCitation.SelectionRects);
        }


        // --------------------------
        /*************************************************
         
        ***************************************************/


    }
}
