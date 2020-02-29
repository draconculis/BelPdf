using Dek.Cls;
using Dek.DB;
using System;

namespace Dek.Bel.Core.Models
{
    public class RawCitation : IModelWithId
    {
        public Id Id { get; set; }
        public Id VolumeId { get; set; }
        public string Fragment { get; set; }
        public int PageStart { get; set; }
        public int PageStop { get; set; }
        public int GlyphStart { get; set; }
        public int GlyphStop { get; set; }
        public string Rectangles { get; set; }
        public DateTime Date { get; set; }
    }
}
