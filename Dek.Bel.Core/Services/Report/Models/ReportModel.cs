using Dek.Cls;
using System;

namespace Dek.Bel.Core.Services
{
    public class ReportModel
    {
        public int Idx { get; set; }
        public Id VolumeId { get; set; }
        public string VolumeTitle { get; set; }
        public string PublicationDate { get; set; }

        public Id CitationId { get; set; }
        public int Page { get; set; }
        public int PhysicalPage { get; set; }

        public string OriginalCitation { get; set; } // Citation1
        public string Citation { get; set; } // Citation3
        public string CitationAndSource => Citation + Environment.NewLine + 
            $" - <author>, {Book}, Chapter: {Chapter}";

        public string Book { get; set; }
        public string Chapter { get; set; }
        public string SubChapter { get; set; }
        public string Paragraph { get; set; }

        public string MainCategory { get; set; }
        public int Weight { get; set; }

        // Hidden
        public string Emphasis { get; set; }
    }
}
