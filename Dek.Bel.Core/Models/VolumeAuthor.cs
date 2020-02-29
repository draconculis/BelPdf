using Dek.Cls;
using Dek.DB;

namespace Dek.Bel.Core.Models
{
    public class VolumeAuthor
    {
        [Key]
        public Id VolumeId { get; set; }
        [Key]
        public Id AuthorId { get; set; }

        public AuthorType AuthorType { get; set; }

        public string Notes { get; set; }
    }
}
