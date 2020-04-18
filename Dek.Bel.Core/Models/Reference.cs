using Dek.Cls;
using Dek.DB;
using System;

namespace Dek.Bel.Core.Models
{
    public class Reference : IModelWithId, IComparable<Reference>
    {
        public Id Id { get; set; }
        public Id VolumeId { get; set; }
        public int PhysicalPage { get; set; }
        public int Glyph { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int CompareTo(Reference other)
        {
            if (PhysicalPage == other.PhysicalPage)
                return Glyph.CompareTo(other.Glyph);

            return PhysicalPage.CompareTo(other.PhysicalPage);
        }

        public int CompareTo(Citation other)
        {
            if (PhysicalPage == other.PhysicalPageStart)
                return Glyph.CompareTo(other.GlyphStart);

            return PhysicalPage.CompareTo(other.PhysicalPageStart);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
