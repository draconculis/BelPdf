using BelManagedLib;
using Dek.Bel.Categories;
using Dek.Bel.DB;
using Dek.Bel.UserSettings;
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

        //public CitationModel

        [Import] public ICategoryService m_CategoryService { get; set; }
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public CategoryRepo m_CategoryRepo { get; set; }



        public BelGui(EventData messsage) : this()
        {
            richTextBox1.Text = messsage.Text;
            toolStripStatusLabel_GUID.Text = Guid.NewGuid().ToString();
            label1_MD5.Text = Guid.NewGuid().ToString();

            textBox_FileName.Text = Path.GetFileName(messsage.FilePath);
            //textBox_FileName = Path.GetFileName(messsage.FilePath);
            label_Page.Text = $"{messsage.StartPage} - {messsage.StopPage}";
        }

        public BelGui()
        {
            InitializeComponent();
            Meffify();

            toolStripTextBox1.Text = (string)Properties.Settings.Default["DeselectionMarker"];
        }

        private void Meffify()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(BelGui).Assembly));

            var container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                MessageBox.Show($"{Environment.NewLine}{compositionException}", "Composition error");
            }
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
            Font font = (Font)Properties.Settings.Default["Font"];

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
                Properties.Settings.Default["Font"] = fontDialog1.Font;
            }
        }

        private const float FontScaleFactor = 0.15f;
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Font f = richTextBox1.Font;
            Font newFont = new Font(f.FontFamily, f.Size * (1 + FontScaleFactor), f.Style, f.Unit);
            richTextBox1.Font = newFont;
            richTextBox2.Font = newFont;
            Properties.Settings.Default["Font"] = newFont;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Font f = richTextBox1.Font;
            float size = (f.Size * (1 - FontScaleFactor)) <= 4.0f ? 4.0f : f.Size * (1 - FontScaleFactor);
            Font newFont = new Font(f.FontFamily, size, f.Style, f.Unit);
            richTextBox1.Font = newFont;
            richTextBox2.Font = newFont;
            Properties.Settings.Default["Font"] = newFont;
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

        private void AddCategoryLabel(CategoryModel cat)
        {
            // TODO: ADD TO DB
            Label l = m_CategoryService.CreateCategoryLabelControl(cat.ToString(), false, contextMenuStrip_Category);
            
            if (flowLayoutPanel_Categories.Controls.Count < 1)
                m_CategoryService.SetMainStyleOnLabel(l);
            flowLayoutPanel_Categories.Controls.Add(l);
            comboBox2.Focus();
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
                m_CategoryService.ClearMainStyleFromLabels(flowLayoutPanel_Categories.Controls.OfType<Label>());
                m_CategoryService.SetMainStyleOnLabel(label);
            }
        }

        private void textBox1_CategorySearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_CategorySearch_KeyDown(object sender, KeyEventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(textBox1_CategorySearch.Text))
            //    return;

            //if (e.KeyCode == Keys.Return)
            //{
            //    e.Handled = true;
            //    e.SuppressKeyPress = true;
            //    AddCategoryLabel();
            //}
        }

        private void textBox1_CategorySearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCategory fc = new FormCategory(m_CategoryService);
            fc.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

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

        // Category combo
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
            if (!(sender is ComboBox combo))
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
            var cats = m_CategoryRepo.SearchCategoriesByNameOrCode(combo.Text);
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
                listBox1.Top = combo.Top + combo.Height;
                listBox1.Left = combo.Left;
                listBox1.Width = combo.Width + button_category.Width;
                listBox1.Visible = true;
            }

        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (!(listBox1.SelectedItem is CategoryModel cat))
                return;

            AddCategoryLabel(cat);

            listBox1.Visible = false;
            comboBox2.Text = "";
        }

        // --------------------------
    }
}
