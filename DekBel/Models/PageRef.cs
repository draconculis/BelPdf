using Dek.Bel.DB;

namespace Dek.Bel.Models
{
    public class PageRef
    {
        public Id Id { get; set; }
        public Id VolumeId { get; set; }
        public int PhysicalPage { get; set; }
        public int GlyphStart { get; set; } = 1;
        public int Page { get; set; }
        public int Description { get; set; }
    }
}
