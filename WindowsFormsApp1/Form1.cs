using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dek.Bel;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            BelGui bel = new BelGui(
                new BelManagedLib.EventData {
                    Text = "␇ Recently, analyses of two large stellar surveys revealed the presence of a well populated elemental abundance sequence, two distinct sequences in the colour–magnitude diagram and a prominent, slightly retrograde kinematic structure in the halo near the Sun, which may trace an important accretion event experienced by the Galaxy."
                    });

            bel.ShowDialog();

            label_resulttext.Text = bel.Result.Message;
        }
    }
}
