using Dek.Cls;
using Dek.DB;

namespace Dek.Bel.Core.Models
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
