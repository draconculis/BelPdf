using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.ReferenceGui
{
    public partial class Form_AddReference : Form
    {
        public string Value { get; set; }

        public Form_AddReference(string header, string value = null)
        {
            Label l = new Label();
            l.Text = value;
            int w = l.Width;

            if (w > Width)
                Width = Math.Min(w * 2, 1000);

            label1.Text = header;

            InitializeComponent();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form_AddReference_Load(object sender, EventArgs e)
        {

        }

        private void Button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Button_ok_Click(object sender, EventArgs e)
        {
            Value = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
