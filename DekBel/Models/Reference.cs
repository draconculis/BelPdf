using Dek.Bel.Cls;
using Dek.Bel.DB;
using System;

namespace Dek.Bel.Models
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

        public override string ToString()
        {
            return Title;
        }
    }
}
