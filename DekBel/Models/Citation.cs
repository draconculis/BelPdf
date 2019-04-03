using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class Citation
    {
        public Id Id { get; set; }
        public Id VolumeId { get; set; }

        public int PhysicalPageStart { get; set; }
        public int PhysicalPageStop { get; set; }
        public int GlyphStart { get; set; }
        public int GlyphStop { get; set; }
        public string SelectionRects { get; set; }

        public string Citation1 { get; set; } // Original text (concatenated with raw citations)
        public string Citation2 { get; set; } // Deletions replaced with '…'. Emphasis med ¤ Fet ¤, speciell hilite i citat.
        public string Citation3 { get; set; } // Final edited text.

        public string Exclusion { get; set; } // for Citation2
        public string Emphasis { get; set; } // for Citation3

        public DateTime CreatedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public string Notes { get; set; }

        private const int maxlen = 50;

        public override string ToString()
        {
            return Id.ToString().Substring(24) + " - " + Citation1.Substring(0, Citation1.Length > maxlen ? maxlen : Citation1.Length) + $"{(Citation1.Length > maxlen ? "…" : "")}";
        }
    }
}
