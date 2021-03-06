﻿using System;

namespace Dek.Bel.Core.Models
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

    public class Book : Reference
    {
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public string PublicationDate { get; set; }
        public string Notes { get; set; }
    }
}
