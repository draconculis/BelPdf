using Dek.Bel.Cls;
using Dek.Bel.DB;
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

namespace Dek.Bel.Services
{
    public partial class RawCitationEditor : Form
    {
        [Import] public IDBService m_DBService { get; set; }
        StupidEncodingFixer fixer = new StupidEncodingFixer();
        ModelsForViewing VM;
        bool _hasUnsavedChanges;
        bool HasUnsavedChanges
        {
            get => _hasUnsavedChanges;
            set 
            {
                button_Reset.Enabled = value;
                _hasUnsavedChanges = value;
            }
        }


        public RawCitationEditor(ModelsForViewing vm, Font citationFont)
        {
            VM = vm;

            if (m_DBService == null)
                Mef.Initialize(this);

            InitializeComponent();

            textBox1.Font = citationFont;
            textBox2.Font = citationFont;

            HasUnsavedChanges = false;

            LoadControls();
        }

        void LoadControls()
        {
            textBox1.Text = VM.CurrentCitation.Citation1;
            textBox2.Text = VM.CurrentCitation.Citation1;
        }

        private void FixGreekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int start = textBox2.SelectionStart;
            int len = textBox2.SelectionLength;

            string original = textBox2.Text;
            string selectedText = textBox2.SelectedText;
            if (string.IsNullOrWhiteSpace(textBox2.SelectedText))
                selectedText = textBox2.Text;

            string fixedText = fixer.FixGreekText(selectedText);

            string s1 = start > 0 ? original.Substring(0, start - 1) : "";
            string s3 = (start + len - 1) < original.Length -1 ? original.Substring(start + len) : "";

            textBox2.Text = s1 + fixedText + s3;
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"This cannot be undone.{Environment.NewLine}Continue and save raw citation?", "Save raw citation?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            VM.CurrentCitation.Citation1 = textBox2.Text;
            m_DBService.InsertOrUpdate(VM.CurrentCitation);
            LoadControls();
            HasUnsavedChanges = false;
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            if (HasUnsavedChanges)
                if (MessageBox.Show($"Your changes will not be saved to the database.{Environment.NewLine}Continue and discard changes?", "Unsaved changes", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

            Close();
        }

        private void Button_Reset_Click(object sender, EventArgs e)
        {
            if (!HasUnsavedChanges)
                return;

            if (MessageBox.Show($"Reset your changes?{Environment.NewLine}N.B: This will not write anything to the database.", "Reset changes?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            textBox2.Text = textBox1.Text;
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.Undo();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            HasUnsavedChanges = true;
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.Copy();
        }
    }
}
