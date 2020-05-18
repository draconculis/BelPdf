using Dek.Cls;
using Dek.DB;
using System;

namespace Dek.Bel.Core.Models
{
    public class AuthorWithType : Author
    {
        public AuthorType AuthorType { get; set; }
        public Id ItemId { get; set; }
    }
}
