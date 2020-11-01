using Dek.Cls;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dek.Bel.Core.Services
{
    public class ReportModel
    {
        public int Idx { get; set; }
        public Id VolumeId { get; set; }
        public string VolumeTitle { get; set; }
        public string VolumePublicationDate { get; set; }

        public Id CitationId { get; set; }
        public int Page { get; set; }
        public int PhysicalPage { get; set; }

        public string OriginalCitation { get; set; } // Citation1
        public string Citation { get; set; } // Citation3
        public string CitationAndSource => Citation + Environment.NewLine + 
            $" - <author>, {Book}, Chapter: {Chapter}";

        public string Book { get; set; }
        public string BookAuthor { get; set; }
        public string Chapter { get; set; }
        public string SubChapter { get; set; }
        public string Paragraph { get; set; }

        public string MainCategory { get; set; }
        public int MainCategoryWeight { get; set; }

        // Hidden, but needed in grid to handle printout of citations
        //[Display(AutoGenerateField = false, Description = "Emphasis field is not generated in UI")]
        public string Emphasis { get; set; }
    }
}
