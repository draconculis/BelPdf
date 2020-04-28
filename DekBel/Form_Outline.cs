using Dek.Bel.Core.DB;
using Dek.Bel.Core.Services;
using Dek.DB;
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
using Dek.Cls;
using Dek.Bel.Core.Models;

namespace Dek.Bel
{
    public partial class Form_Outline : Form
    {
        [Import] public IDBService m_DBService { get; set; }
        [Import] public HistoryRepo m_HistoryRepo { get; set; }
        [Import] public VolumeService m_VolumeService { get; set; }

        public Citation SelectedCitation { get; set; }

        List<Reference> References;
        List<CitationReference> CitationReferences;

        private EventHandler onCitationSelected;
        // Define the event member using the event keyword.  
        // In this case, for efficiency, the event is defined
        // using the event property construct.  
        public event EventHandler CitationSelected
        {
            add
            {
                onCitationSelected += value;
            }
            remove
            {
                onCitationSelected -= value;
            }
        }

        public Form_Outline(VolumeService volumeService, Id currentCitation, int left, int top, int width, int height)
        {
            InitializeComponent();

            Left = left;
            Top = top;
            Width = width;
            Height = height;

            if (m_DBService == null)
                Mef.Compose(this);

            References = m_VolumeService.GetAllReferences();

            CitationReferences = CreateCitationReferenceList(References, m_VolumeService.Citations);

            listBox1.DrawItem += ListBox1_DrawItem;
            listBox1.MeasureItem += ListBox1_MeasureItem;
            listBox1.DataSource = CitationReferences;

            CitationReference sel = CitationReferences.SingleOrDefault(x => x.Citation?.Id == currentCitation);

            if (sel != null)
            {
                listBox1.SelectedItem = sel;
            }
        }

        #region Custom paint =======================================================

        private void ListBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 22;
        }

        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            //Rectangle rc = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 5, e.Bounds.Height - 3);
            //e.Graphics.FillRectangle(new SolidBrush(Color.CornflowerBlue), rc);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            sf.FormatFlags = StringFormatFlags.NoWrap;

            CitationReference citRef = CitationReferences[e.Index];
            string indent = "";

            Font theFont = Font;
            if(citRef.Reference is null)
            {
                indent = "       ";
            }
            else if (citRef.Reference is Book)
            {
                theFont = new Font(FontFamily.GenericSerif, 14, FontStyle.Bold | FontStyle.Underline);
            }
            else if (citRef.Reference is Chapter)
            {
                theFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                indent = "  ";
            }
            else if (citRef.Reference is SubChapter)
            {
                theFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold | FontStyle.Italic);
                indent = "    ";
            }
            else if (citRef.Reference is Paragraph)
            {
                theFont = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);
                indent = "      §";
            }

            e.Graphics.DrawString(indent + citRef.ToString().Replace("\n", " ").Replace("\r", " "),
                theFont,
                new SolidBrush(Color.Black),
                e.Bounds,
                sf);
        }

        #endregion Custom paint ====================================================

        private void Form_Outline_Load(object sender, EventArgs e)
        {

        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private List<CitationReference> CreateCitationReferenceList(List<Reference> references, List<Citation> citations)
        {
            List<CitationReference> res = new List<CitationReference>();
            if ((references?.Count() ?? 0) == 0 && (citations?.Count() ?? 0) == 0)
                return res;

            if ((references?.Count() ?? 0) == 0)
                return citations
                    .Select(x => new CitationReference(x))
                    .ToList();

            if ((citations?.Count() ?? 0) == 0)
                return references
                    .Select(x => new CitationReference(x))
                    .ToList();

            var curCit = 0;
            //var curReference = 0;
            //var curCitation = citations.First();
            //var curReference = references.First();

            bool endOfCit = false;
            foreach(Reference reference in references)
            {
                while (reference.CompareTo(citations[curCit]) > 0 && !endOfCit)
                {
                    res.Add(new CitationReference(citations[curCit]));
                    if(curCit + 1 == citations.Count)
                        endOfCit = true;
                    else
                        curCit++;
                }
                res.Add(new CitationReference(reference));
            }
            while (!endOfCit)
            {
                res.Add(new CitationReference(citations[curCit]));
                if (curCit + 1 == citations.Count)
                    endOfCit = true;
                else
                    curCit++;
            }

            return res;
        }

        // Select an close
        private void button1_Click(object sender, EventArgs e)
        {
            SelectCitation();
        }

        private void SelectCitation()
        {
            if (listBox1.SelectedItem == null)
                return;
            if (((CitationReference)listBox1.SelectedItem).Citation == null)
                return;

            SelectedCitation = ((CitationReference)listBox1.SelectedItem).Citation;
            onCitationSelected?.Invoke(this, new EventArgs());
        }

        private void Form_Outline_ResizeEnd(object sender, EventArgs e)
        {
            listBox1.Invalidate(true);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectCitation();
        }
    }

    public class CitationReference : Reference
    {
        public Citation Citation { get; }
        public Reference Reference { get; }

        public CitationReference(Citation citation)
        {
            Citation = citation;
        }

        public CitationReference(Reference reference)
        {
            Reference = reference;
        }

        public override string ToString()
        {
            return Citation == null
                ? Reference.ToString()
                : Citation.ToString();
        }
    }
}
