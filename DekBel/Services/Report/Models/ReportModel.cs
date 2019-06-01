﻿using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    public class ReportModel
    {
        public int Idx { get; set; }
        public Id VolumeId { get; set; }
        public string VolumeTitle { get; set; }
        public DateTime PublicationDate { get; set; }

        public Id CitationId { get; set; }
        public int Page { get; set; }
        public int PhysicalPage { get; set; }

        public string OriginalCitation { get; set; } // Citation1
        public string Citation { get; set; } // Citation3

        public string Book { get; set; }
        public string Chapter { get; set; }
        public string SubChapter { get; set; }
        public string Paragraph { get; set; }

        public string Category { get; set; }
    }
}
