using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dek.Bel
{
    public partial class Form_ResetCitation : Form
    {
        private Form_ResetCitation()
        {
            InitializeComponent();
        }

        public Form_ResetCitation(string citationText, Font citationFont) : this()
        {
            richTextBox1.Font = citationFont;
            richTextBox1.Text = citationText;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            Close();
        }
    }
}
