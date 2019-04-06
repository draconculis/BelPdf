using BelManagedLib;
using Dek.Bel.Cls;
using Dek.Bel.Models;
using Dek.Bel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.DB
{
    [Export]
    public class CitationService
    {
        [Import] IDBService DBService { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }
        string SqlSelectFields = $"SELECT `Id`, `Fragment`, `PageStart`, `PageStop`, `GlyphStart`, `GlyphStop`, `Rectangles`, `Date`";

        public CitationService()
        {
            Mef.Initialize(this);
        }

        public RawCitation AddRawCitations(EventData message)
        {
            int[] rects = ArrayStuff.ExtractArrayFromIntPtr(message.SelectionRects, message.Len * 4);

            RawCitation raw = new RawCitation
            {
                Id = Id.NewId(),
                Fragment = message.Text,
                GlyphStart = message.StartGlyph,
                GlyphStop = message.StopGlyph,
                PageStop = message.StopPage,
                PageStart = message.StartPage,
                Date = DateTime.Now,
            };

            DBService.InsertOrUpdate(raw);

            return raw;
        }

        private string ComposeCitation(List<RawCitation> rawCitations, string citation)
        {
            // TODO: The rectangles, jada jada

            string text = "";
            string adjoiner = UserSettingsService.DeselectionMarker;
            foreach (var raw in rawCitations)
            {
                text += $"{(text.Length > 1 ?  adjoiner : "")}" + raw.Fragment;
            }

            text += $"{(text.Length > 1 ? adjoiner : "")}" + citation;
            return text;
        }

        internal Citation CreateNewCitation(List<RawCitation> rawCitations, EventData message, Id volumeId)
        {
            int[] rects = ArrayStuff.ExtractArrayFromIntPtr(message.SelectionRects, message.Len * 4);
            string citationText = ComposeCitation(rawCitations, message.Text);
            RichTextBox rtb = new RichTextBox();
            rtb.Text = citationText;
            var citation = new Citation
            {
                Id = Id.NewId(),
                VolumeId = volumeId,
                Citation1 = citationText, // Original, untouched, never changed
                Citation2 = rtb.Text, // More of the same, textb 1
                Citation3 = "", // More of the same, textb 2
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
                GlyphStart = message.StartGlyph,
                GlyphStop = message.StopGlyph,
                PhysicalPageStart = message.StartPage,
                PhysicalPageStop = message.StopPage,
                SelectionRects = ArrayStuff.ConvertArrayToString(rects),
            };

            DBService.InsertOrUpdate(citation);

            return citation;
        }

    }
}
