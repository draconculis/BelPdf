using Dek.Cls;
using System;
using Dek.DB;

namespace Dek.Bel.Core.Models
{
    public class Volume : IModelWithId
    {
        public Id Id { get; set; }
        public Id SeriesId { get; set; }
        public string Title { get; set; }
        public string Editor { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Notes { get; set; }

        // Draw all pdf boxes with this offset for this volume
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;

        public override string ToString()
        {
            return Title;
        }
    }
}
