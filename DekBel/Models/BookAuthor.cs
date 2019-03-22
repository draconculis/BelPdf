using Dek.Bel.DB;

namespace Dek.Bel.Models
{
    public class BookAuthor
    {
        [Key]
        public Id BookId { get; set; }
        [Key]
        public Id AuthorId { get; set; }

        public AuthorType AuthorType { get; set; }
    }
}
