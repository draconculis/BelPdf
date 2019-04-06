using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class Reference : IModelWithId
    {
        public Id Id { get; set; }
        public Id VolumeId { get; set; }
        public int PhysicalPage { get; set; }
        public int Glyph { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
