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
using Dek.Bel.DB;
using Dek.Bel.InterOp;

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
                    //SelectionRects = ,
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

        private void button5_Click(object sender, EventArgs e)
        {
            var citationRepo = new CitationService();
            citationRepo.AddRawCitations(
                         new BelManagedLib.EventData
                         {
                             Text = "First text.",
                             StartPage = 1,
                             StopPage = 1,
                             StartGlyph = 100,
                             StopGlyph = 110,
                             //SelectionRectsString = "1,2,3,4;",
                             Len = 1,
                             Code = (int)CodesEnum.DEKBELCODE_ADDCITATION,
                             FilePath = @"C:\\28",
                         }
                );

            citationRepo.AddRawCitations(
                         new BelManagedLib.EventData
                         {
                             Text = "Second text.",
                             StartPage = 2,
                             StopPage = 2,
                             StartGlyph = 19,
                             StopGlyph = 25,
                             //SelectionRectsString = "1,2,3,4;",
                             Len = 1,
                             Code = (int)CodesEnum.DEKBELCODE_ADDCITATION,
                             FilePath = @"C:\\28",
                         }
                );


            BelGui bel = new BelGui(
            new BelManagedLib.EventData
            {
                Text = "Third text: Recently, analyses of two large stellar surveys revealed the presence.",
                StartPage = 1,
                StopPage = 1,
                StartGlyph = 5,
                StopGlyph = 8,
                //SelectionRectsString = "1,2,3,4;",
                Len = 1,
                Code = (int)CodesEnum.DEKBELCODE_ADDANDSHOWCITATION,
                FilePath = @"C:\\28",
            });

            bel.ShowDialog();

            label_resulttext.Text = bel.Result.Message;
        }
    }
}
