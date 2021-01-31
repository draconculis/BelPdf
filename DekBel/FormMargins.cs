using System;
using System.Windows.Forms;

namespace Dek.Bel
{
    public partial class FormMargins : Form
    {
        public int Left => (int)numericUpDown_left.Value;
        public int Top => (int)numericUpDown_top.Value;
        public int Right => (int)numericUpDown_right.Value;
        public int Bottom => (int)numericUpDown_bottom.Value;

        private FormMargins()
        {
            InitializeComponent();
        }

        public FormMargins (int left, int top, int right, int bottom) : this()
        {
            numericUpDown_left.Value = left >= 0 ? left : 0;
            numericUpDown_top.Value = top >= 0 ? top : 0;
            numericUpDown_right.Value = right >= 0 ? right : 0;
            numericUpDown_bottom.Value = bottom >= 0 ? bottom : 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
