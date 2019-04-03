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
        private bool mouseIsDown = false;
        private Point firstPoint;

        public Form_AddReference(string header, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            InitializeComponent();

            Size s = TextRenderer.MeasureText(value, textBox1.Font);
            if(s.Width > 800)
            {
                Width = 900;
                textBox1.ScrollBars = ScrollBars.Vertical;
                textBox1.Multiline = true;
                Height *= 2;
            }
            else if (s.Width > Width)
                Width = s.Width + 100;

            label1.Text = header;
            if (!string.IsNullOrWhiteSpace(value))
                textBox1.Text = value;
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

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();

        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void Form_AddReference_MouseDown(object sender, MouseEventArgs e)
        {
            firstPoint = e.Location;
            mouseIsDown = true;
        }

        private void Form_AddReference_MouseUp(object sender, MouseEventArgs e)
        {
            mouseIsDown = false;
        }

        private void Form_AddReference_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseIsDown)
            {
                // Get the difference between the two points
                int xDiff = firstPoint.X - e.Location.X;
                int yDiff = firstPoint.Y - e.Location.Y;

                // Set the new point
                int x = this.Location.X - xDiff;
                int y = this.Location.Y - yDiff;
                this.Location = new Point(x, y);
            }
        }
    }
}
