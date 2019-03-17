using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class CitationCategory : IModel
    {
        [Key]
        public Id CitationId { get; set; }

        [Key]
        public Id CategoryId { get; set; }

        public int Weight { get; set; }

        public bool IsMain { get; set; }
    }
}
