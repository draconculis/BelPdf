using BelManagedLib;
using Dek.Bel.Cls;
using Dek.Bel.Models;
using Dek.Bel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
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
                Rectangles = ArrayStuff.ConvertPageAndArrayToString(message.StartPage, rects),
            };

            DBService.InsertOrUpdate(raw);

            return raw;
        }

        private string ComposeCitation(List<RawCitation> rawCitations, string citation)
        {
            string text = "";
            string adjoiner = UserSettingsService.DeselectionMarker;
            foreach (var raw in rawCitations)
            {
                text += $"{(text.Length > 1 ? adjoiner : "")}" + raw.Fragment;
            }

            text += $"{(text.Length > 1 ? adjoiner : "")}" + citation;
            return text;
        }

        private List<(int page, int[] rects)>  ComposeRectangles(List<RawCitation> rawCitations, (int page, int[] rects) mainPageRect)
        {
            var res = new List<(int page, int[] rects)>();
            res.Add(mainPageRect);
            foreach (var raw in rawCitations)
            {
                res.AddRange(ArrayStuff.ConvertStringToPagesAndArrays(raw.Rectangles));
            }

            return res;
        }

        internal Citation CreateNewCitation(List<RawCitation> rawCitations, EventData message, Id volumeId)
        {
            (int page, int[] rects) citationPageRects = (message.StartPage, ArrayStuff.ExtractArrayFromIntPtr(message.SelectionRects, message.Len * 4));
            string citationText = ComposeCitation(rawCitations, message.Text);
            List<(int page, int[] rects)> pageRects = ComposeRectangles(rawCitations, citationPageRects);

            (int pageStart, int pageStop, int glyphStart, int glyphStop) boundaries = GetBoundaries(rawCitations, message);

            var citation = new Citation
            {
                Id = Id.NewId(),
                VolumeId = volumeId,
                Citation1 = citationText, // Original, never changed (unless it contains stupid greek encoding from hell)
                Citation2 = citationText, // More of the same, textb 1
                Citation3 = "", // More of the same, textb 2
                CreatedDate = DateTime.Now,
                EditedDate = DateTime.Now,
                GlyphStart = boundaries.glyphStart,
                GlyphStop = boundaries.glyphStop,
                PhysicalPageStart = boundaries.pageStart,
                PhysicalPageStop = boundaries.pageStop,
                SelectionRects = ArrayStuff.ConvertPageAndArrayToString(pageRects),
                CitationColors = ColorStuff.ConvertColorsToString(UserSettingsService.PdfHighLightColor, UserSettingsService.PdfUnderlineColor),
            };

            DBService.InsertOrUpdate(citation);

            return citation;
        }

        /// <summary>
        /// Get minimum / maximum for page and glyph
        /// </summary>
        /// <param name="rawCitations"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private (int pageStart, int pageStop, int glyphStart, int glyphStop) GetBoundaries(List<RawCitation> rawCitations, EventData message)
        {
            int startPage = message.StartPage;
            int stopPage = message.StopPage;
            int startGlyph = message.StartGlyph;
            int stopGlyph = message.StopGlyph;
            foreach (var rawCitation in rawCitations)
            {
                if (rawCitation.PageStart < startPage)
                    startPage = rawCitation.PageStart;

                if (rawCitation.PageStop > stopPage)
                    stopPage = rawCitation.PageStop;

                if (rawCitation.GlyphStart < startGlyph)
                    startGlyph = rawCitation.GlyphStart;

                if (rawCitation.GlyphStop > stopGlyph)
                    stopGlyph = rawCitation.GlyphStop;
            }

            return (startPage, stopPage, startGlyph, stopGlyph);
        }
    }
}
