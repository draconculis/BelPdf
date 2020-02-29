using Dek.Cls;
using Dek.DB;

namespace Dek.Bel.Core.Models
{
    public class SeriesAuthor
    {
        [Key]
        public Id SeriesId { get; set; }
        [Key]
        public Id AuthorId { get; set; }

        public AuthorType AuthorType { get; set; }

        public string Notes { get; set; }
    }
}
