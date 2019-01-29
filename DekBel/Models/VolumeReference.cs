using Dek.Bel.Cls;
using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class VolumeReference
    {
        public Id VolumeId { get; set; }
        public int PhysicalPage { get; set; }
        public int TitleGlyphStart { get; set; }
        public int TitleGlyphStop { get; set; }
        public string Title { get; set; }
    }
}
