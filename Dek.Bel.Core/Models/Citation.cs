﻿using Dek.Cls;
using System;

namespace Dek.Bel.Core.Models
{
    public class Citation : IComparable<Citation>
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

        public string CitationColors { get; set; } // Highlight + underline color arrays (incl alpha)
        public string MarginBoxSettings { get; set; } // Size, font, left/right
        public string ExtraSettings { get; set; } // For the future

        public DateTime CreatedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public string Notes { get; set; }

        private const int maxlen = 50;

        public override string ToString()
        {
            return $"[{Id.ToStringShort()}]" + " " + Citation1.RemoveLineBreaks().Left(maxlen, true);
        }

        public string ToStringLong()
        {
            return $"[{Id.ToStringShort()}]" + " " + Citation1.RemoveLineBreaks();
        }

        public string ToStringShort()
        {
            return $"[{Id.ToStringShort()}]";
        }

        public int CompareTo(Citation other)
        {
            if (PhysicalPageStart == other.PhysicalPageStart)
                return GlyphStart.CompareTo(other.GlyphStart);

            return PhysicalPageStart.CompareTo(other.PhysicalPageStart);
        }

        public int CompareTo(Reference other)
        {
            if (PhysicalPageStart == other.PhysicalPage)
                return GlyphStart.CompareTo(other.Glyph);

            return PhysicalPageStart.CompareTo(other.PhysicalPage);
        }
    }
}
