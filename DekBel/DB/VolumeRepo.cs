using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.DB
{
    [Export]
    public class VolumeRepo
    {
        [Import] IDBService DBService { get; set; }

        public List<Book> Books { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<PageRef> Pages { get; set; }

    }
}
