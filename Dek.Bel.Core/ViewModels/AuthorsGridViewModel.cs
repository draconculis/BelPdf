using Dek.Bel.Core.Models;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Core.ViewModels
{
    /// <summary>
    /// Used in the authors tab
    /// </summary>
    public class AuthorsGridViewModel : Author
    {
        public AuthorType AuthorType { get; set; }

        public Id ItemId { get; set; }

        public string ItemType { get; set; }

        public string ItemTitle { get; set; }
    }

    public static class AuthorsGridViewModelExtensions
    {

        public static void LoadAuthor(this AuthorsGridViewModel me, Author author, AuthorType authorType, string ItemType, string itmeTitle)
        {
            me.Load(author);

        }
    }

}
