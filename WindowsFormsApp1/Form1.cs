using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BelManagedLib;
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
                    Text = "␇ Recently, analyses of two large stellar surveys revealed the presence of a well populated elemental abundance sequence, two distinct sequences in the colour–magnitude diagram and a prominent, slightly retrograde kinematic structure in the halo near the Sun, which may trace an important accretion event experienced by the Galaxy.",
                    StartPage = 1,
                    StopPage = 1,
                    StartGlyph = 5,
                    StopGlyph = 8,
                    SelectionRectsString = "1,2,3,4;",
                    Len = 1,
                    Code = 1,
                    FilePath = @"C:\\28",
                });

            bel.ShowDialog();

            label_resulttext.Text = bel.Result.Message;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RequestFileStorageData data = new RequestFileStorageData
            {
                FilePath = @"s:\Users\stimo\Desktop\1662753704.pdf",
            };

            BelManagedClass bel = new BelManagedClass();

            ResultFileStorageData res = bel.RequestFileStoragePath(data);

            MessageBox.Show(res.StorageFilePath);

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

    }
}
