using BelManagedLib;
using Dek.Bel.Cls;
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
    public class CitationRepo
    {
        [Import] IDBService dBService { get; set; }
        string SqlSelectFields = $"SELECT `Id`, `Fragment`, `PageStart`, `PageStop`, `GlyphStart`, `GlyphStop`, `Rectangles`, `Date`";

        public CitationRepo()
        {
            Mef.Initialize(this);
        }

        public RawCitation AddRawCitations(EventData messsage)
        {
            RawCitation raw = new RawCitation
            {
                Id = Id.NewId(),
                Fragment = messsage.Text,
                GlyphStart = messsage.StartGlyph,
                GlyphStop = messsage.StopGlyph,
                PageStop = messsage.StopPage,
                PageStart = messsage.StartPage,
                Rectangles = messsage.SelectionRectsString,
                Date = DateTime.Now,
            };

            dBService.InsertOrUpdate(raw);

            return raw;
        }


    }
}
