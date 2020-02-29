using BelManagedLib;
using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Bel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Dek.Bel.Cls
{
    [Export]
    public class CitationService
    {
        [Import] IDBService m_DBService { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }
        [Import] public HistoryRepo m_HistoryRepo { get; set; }

        private History LastHistory; // Used to get current Volume

        public CitationService()
        {
            if (m_DBService == null)
                Mef.Initialize(this);

            LastHistory = m_HistoryRepo.GetLastOpened(); // Our currently open file in Sumatra
        }

        public RawCitation AddRawCitations(EventData message)
        {
            LastHistory = m_HistoryRepo.GetLastOpened(); // Our currently open file in Sumatra

            int[] rects = ArrayStuff.ExtractArrayFromIntPtr(message.SelectionRects, message.Len * 4);
            var volume = m_DBService.SelectById<Volume>(LastHistory.VolumeId);

            RawCitation raw = new RawCitation
            {
                Id = Id.NewId(),
                VolumeId = volume.Id,
                Fragment = message.Text,
                GlyphStart = message.StartGlyph,
                GlyphStop = message.StopGlyph,
                PageStop = message.StopPage,
                PageStart = message.StartPage,
                Date = DateTime.Now,
                Rectangles = ArrayStuff.ConvertPageAndArrayToString(message.StartPage, rects),
            };

            m_DBService.InsertOrUpdate(raw);

            return raw;
        }

        public IEnumerable<RawCitation> GetRawCitations(Id volumeId)
        {
            //LastHistory = m_HistoryRepo.GetLastOpened(); // Our currently open file in Sumatra
            //Id volumeId = LastHistory.VolumeId;
            //return m_DBService.Select<RawCitation>().Where(r => r.VolumeId == volumeId);
            return m_DBService.Select<RawCitation>().Where(r => r.VolumeId == volumeId);
        }

        public IEnumerable<Citation> GetCitations(Id volumeId)
        {
            return m_DBService.Select<Citation>().Where(c => c.VolumeId == volumeId);
        }

        public Citation GetCitation(Id volumeId, Id id)
        {
            return GetCitations(volumeId).SingleOrDefault(c => c.Id == id);
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

        public Citation CreateNewCitation(EventData message, Id volumeId)
        {
            List<RawCitation> rawCitations = m_DBService.Select<RawCitation>().Where(x => x.VolumeId == volumeId).ToList();
            return CreateNewCitation(rawCitations, message, volumeId);
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
                CitationColors = ColorStuff.ConvertColorsToString(UserSettingsService.PdfHighLightColor, UserSettingsService.PdfUnderlineColor, UserSettingsService.PdfMarginBoxColor),
            };

            m_DBService.InsertOrUpdate(citation);

            return citation;
        }

        public Citation DeleteCitationById(Id volumeId, Id id)
        {
            Citation cit = GetCitations(volumeId).SingleOrDefault(c => c.Id == id);
            if (cit == null)
                return null;

            DeleteCitation(cit);

            return cit;
        }

        public void DeleteCitation(Citation citation)
        {
            m_DBService.Delete(citation);
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
