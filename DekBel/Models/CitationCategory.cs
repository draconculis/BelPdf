using Dek.Bel.Cls;
using Dek.Bel.DB;

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
