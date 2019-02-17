using Dek.Bel.Cls;
using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    /*
 Volym (titel, författare(pl.), trans, förlag, år)
 books (titel, författare(pl.), trans)
 kapitel (start - slutsida, var på sida kapitel börjar)
 subkapitel (start slutsida)
 stycke



 Bibeln: Kapitel + Stycke


kapiel (start + slutsida)

 */

    /* ===========================================================
     rapport över citat
     Per kategori(er)

    selektering spara rader från rapport


     */

    public class Book : VolumeReference, IModelWithId
    {
        public Id Id { get; set; }

        public string ISBN { get; set; }
        public string Edition { get; set; }
        public DateTime PublishDate { get; set; }
        public string Notes { get; set; }
    }
}
