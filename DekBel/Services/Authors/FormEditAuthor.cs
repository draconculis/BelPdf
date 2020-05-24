using Dek.Bel.Core.Cls;
using Dek.Bel.Core.Models;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Services.Authors
{
    public partial class FormEditAuthor : Form
    {
        public Author Author { get; set; }
        public DateTime Born { get; set; }
        public DateTime Dead { get; set; }

        private string DateFormat = "yyyy-MM-dd";

        public FormEditAuthor()
        {
            InitializeComponent();
        }

        public FormEditAuthor(Author author) : this()
        {
            Author = author;
        }

        private void FormEditAuthor_Load(object sender, EventArgs e)
        {
            if (Author == null)
                Author = new Author();

            if (Author.Id.IsNotNull)
            {
                textBox_name.Text = Author.Name;
                textBox_born.Text = Author.Born;
                textBox_dead.Text = Author.Dead;
                textBox_notes.Text = Author.Notes;
                Text = "Edit Author";
            }
            else
            {
                Text = "New Author";
            }
        }

        private void button_done_Click(object sender, EventArgs e)
        {
            Author.Name = textBox_name.Text;
            Author.Born = textBox_born.Text;
            Author.Dead = textBox_dead.Text;
            Author.Notes = textBox_notes.Text;

            if (Author.Id.IsNull)
                Author.Id = Id.NewId();

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void textBox_name_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_name.Text))
                textBox_name.BackColor = Color.LightPink;
            else
                textBox_name.BackColor = Color.White;
        }

        private void textBox_born_TextChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox tb))
                return;

            if (tb.Text.IsValidSaneDate())
            {
                tb.BackColor = Color.White;
                Born = textBox_dead.Text.ToSaneDateTime();
            }
            else
                tb.BackColor = Color.LightPink;
        }


        private void textBox_dead_TextChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox tb))
                return;

            if (tb.Text.IsValidSaneDate())
            {
                tb.BackColor = tb.BackColor = Color.White;
                Born = textBox_born.Text.ToSaneDateTime();
            }
            else
                tb.BackColor = Color.LightPink;

        }


        private void textBox_notes_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
