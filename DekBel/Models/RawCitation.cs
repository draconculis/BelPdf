using Dek.Bel.Cls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class RawCitation
    {
        public Id Id { get; set; }
        public string Fragment { get; set; }
        public int PageStart { get; set; }
        public int PageStop { get; set; }
        public int GlyphStart { get; set; }
        public int GlyphStop { get; set; }
        public string Rectangles { get; set; }
        public DateTime Date { get; set; }
    }
}
