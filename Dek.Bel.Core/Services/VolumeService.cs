using Dek.Bel.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using Dek.DB;
using Dek.Cls;

namespace Dek.Bel.Core.Services
{
    /// <summary>
    /// Represents current loaded Volume, including reference types.
    /// NOTE This service contains a state.
    /// </summary>
    [Export]
    public class VolumeService
    {
        [Import] IDBService m_DBService { get; set; }

        public Volume CurrentVolume { get; private set; }
        public List<Book> Books { get; private set; }
        public List<Chapter> Chapters { get; private set; }
        public List<SubChapter> SubChapters { get; private set; }
        public List<Paragraph> Paragraphs { get; private set; }
        public List<Page> Pages { get; private set; }
        public List<Citation> Citations { get; private set; }

        /// <summary>
        /// Loads a full volume, including all references types
        /// </summary>
        /// <param name="id"></param>
        public void LoadVolume(string id)
        {
            LoadVolume(Id.NewId(id));
        }

        public void LoadVolume(Id id)
        {
            CurrentVolume = m_DBService.SelectById<Volume>(id);
            //Books = m_DBService.Select<Book>($"`VolumeId`='{id}'").OrderBy(x => x.PhysicalPage).ThenBy(y => y.Glyph).ToList();
            Books = m_DBService.Select<Book>($"`VolumeId`='{id}'").OrderBy(x => x).ToList();
            Chapters = m_DBService.Select<Chapter>($"`VolumeId`='{id}'").OrderBy(x => x).ToList();
            SubChapters = m_DBService.Select<SubChapter>($"`VolumeId`='{id}'").OrderBy(x => x).ToList();
            Paragraphs = m_DBService.Select<Paragraph>($"`VolumeId`='{id}'").OrderBy(x => x).ToList();
            Pages = m_DBService.Select<Page>($"`VolumeId`='{id}'").OrderBy(x => x.PhysicalPage).ToList();
        }

        public void LoadCitations(Id volumeId)
        {
            var citations = m_DBService.Select<Citation>($"`VolumeId`='{volumeId}'");
            Citations = citations.AsParallel().OrderBy(x => x.PhysicalPageStart).ThenBy(y => y.GlyphStart).ToList();
        }

        public void LoadCitations()
        {
            LoadCitations(CurrentVolume.Id);
        }

        /// <summary>
        /// Save citation to db and updates loaded citations
        /// </summary>
        public void SaveAndReloadCitation(Citation citation)
        {
            m_DBService.InsertOrUpdate(citation);

            ReloadCitation(citation);
        }

        public void ReloadCitation(Citation citation)
        {
            if (citation is null) // not good
                return;

            if (Citations == null)
                Citations = new List<Citation>();

            var existingCitation = Citations.SingleOrDefault(x => x.Id == citation.Id);
            if (existingCitation != null)
            {
                Citations.Remove(existingCitation);
            }

            Citations.Add(citation);
        }

        public void DeleteCitation(string id)
        {
            DeleteCitation(new Id(id));
        }

        public void DeleteCitation(Id id)
        {
            Citation cit = Citations.SingleOrDefault(x => x.Id == id);
            if (cit == null)
                return;
            m_DBService.Delete(cit);
            LoadCitations(CurrentVolume.Id);
        }

        #region References =========================================================================

        public Book GetBook(int physicalPage, int glyph = 0)
        {
            return GetReference(Books, physicalPage, glyph);
        }

        public Chapter GetChapter(int physicalPage, int glyph = 0)
        {
            return GetReference(Chapters, physicalPage, glyph);
        }

        public SubChapter GetSubChapter(int physicalPage, int glyph = 0)
        {
            return GetReference(SubChapters, physicalPage, glyph);
        }

        public Paragraph GetParagraph(int physicalPage, int glyph = 0)
        {
            return GetReference(Paragraphs, physicalPage, glyph);
        }

        public Page GetPage(int physicalPage, int glyph = 0)
        {
            return GetReference(Pages, physicalPage, glyph);
        }

        public int GetPageNumber(int physicalPage)
        {
            return GetPageNumber(Pages, physicalPage);
        }

        public int GetPageNumber(List<Page> pages, int physicalPage)
        {
            Page page = GetReference(pages, physicalPage, 1);
            if (page == null)
                return physicalPage;

            if (physicalPage == page.PhysicalPage)
                return page.PaginationStart;// + (glyph > page.Glyph ? 1 : 0);

            int offset = physicalPage - page.PhysicalPage;
            return (offset > 0) ? page.PaginationStart + offset : physicalPage; // Only return for values after first pagination ref
        }

        public static int GetPageNumberForVolume(Id volumeId, List<Page> pages, int physicalPage)
        {
            Page page = GetReferenceForVolume(volumeId, pages, physicalPage, 1);
            if (page == null)
                return physicalPage;

            if (physicalPage == page.PhysicalPage)
                return page.PaginationStart;// + (glyph > page.Glyph ? 1 : 0);

            int offset = physicalPage - page.PhysicalPage;
            return (offset > 0) ? page.PaginationStart + offset : physicalPage; // Only return for values after first pagination ref
        }


        /// <summary>
        /// Returns reference page / glyph belongs to.
        /// Returns null if 
        /// 1) no reference exists, 
        /// 2) if page / glyph points before any reference.
        /// </summary>
        /// <returns>Null or reference</returns>
        public T GetReference<T>(List<T> references, int physicalPage, int glyph) where T : Reference
        {
            if (references == null || references.Count == 0)
                return null;

            List<T> orderedReferences = references
                .OfType<T>()
                .OrderBy(x => x)
                .ToList();

            // If pointing before first reference, return null
            if (physicalPage < orderedReferences.First().PhysicalPage 
                || (orderedReferences.First().PhysicalPage == physicalPage && glyph < orderedReferences.First().Glyph))
                return null;


            var lastref = orderedReferences.First();
            int idx = 1;
            while (idx < orderedReferences.Count)
            {
                var cur = orderedReferences[idx++];
                if (cur.PhysicalPage > physicalPage || (cur.PhysicalPage == physicalPage && cur.Glyph > glyph))
                    break;

                lastref = cur;
            }

            return lastref;
        }

        /// <summary>
        /// Gets an ordered list of references for currently loaded volume. Does not include page numbers.
        /// </summary>
        /// <returns></returns>
        public List<Reference> GetAllReferences()
        {
            if (CurrentVolume.Id == Id.Empty)
                return new List<Reference>();

            List<Reference> references = new List<Reference>();

            references.AddRange(Books);
            references.AddRange(Chapters);
            references.AddRange(SubChapters);
            references.AddRange(Paragraphs);

            return references
                .OrderBy(x => x)
                .ToList();
        }

        public static T GetReferenceForVolume<T>(Id volumeId, List<T> references, int physicalPage, int glyph) where T : Reference
        {
            if (references == null || references.Count == 0)
                return null;

            List<T> orderedReferences = references
                .OfType<T>()
                .Where(v => v.VolumeId == volumeId)
                .OrderBy(x => x).ToList();

            if (orderedReferences == null || orderedReferences.Count == 0)
                return null;

            var lastref = orderedReferences.First();
            int idx = 1;
            while (idx < orderedReferences.Count)
            {
                var cur = orderedReferences[idx++];
                if (cur.PhysicalPage > physicalPage || (cur.PhysicalPage == physicalPage && cur.Glyph > glyph))
                    break;

                lastref = cur;
            }

            return lastref;
        }

        /// <summary>
        /// Expects ordered data!
        /// </summary>
        public static T GetReferenceForVolumeOrdered<T>(Id volumeId, List<T> orderedReferences, int physicalPage, int glyph) where T : Reference
        {
            if (orderedReferences == null || orderedReferences.Count == 0)
                return null;

            if (orderedReferences == null || orderedReferences.Count == 0)
                return null;

            var lastref = orderedReferences.First();
            int idx = 1;
            while (idx < orderedReferences.Count)
            {
                var cur = orderedReferences[idx++];
                if (cur.PhysicalPage > physicalPage || (cur.PhysicalPage == physicalPage && cur.Glyph > glyph))
                    break;

                lastref = cur;
            }

            return lastref;
        }

        #endregion References ======================================================================

        /// <summary>
        /// Return a list with all citations in volume, decorated with references.
        /// </summary>
        /// <returns></returns>
        public List<CitationWithReferences> GetCitationWithReferences()
        {
            LoadCitations(CurrentVolume.Id);
            var rcit = new List<CitationWithReferences>();
            foreach (var cit in Citations.Select(x => new CitationWithReferences(x)))
            {
                cit.Book = GetBook(cit.PhysicalPageStart, cit.GlyphStart);
                cit.Chapter = GetChapter(cit.PhysicalPageStart, cit.GlyphStart);
                cit.SubChapter = GetSubChapter(cit.PhysicalPageStart, cit.GlyphStart);
                cit.Paragraph = GetParagraph(cit.PhysicalPageStart, cit.GlyphStart);
                rcit.Add(cit);
            }

            return rcit;
        }
    }

    public class CitationWithReferences : Citation
    {
        public Book Book { get; set; }
        public Chapter Chapter { get; set; }
        public SubChapter SubChapter { get; set; }
        public Paragraph Paragraph { get; set; }

        public CitationWithReferences(Citation citation)
        {
            Type resultType = typeof(CitationWithReferences);
            PropertyInfo[] properties = typeof(Citation).GetProperties();

            foreach (var property in properties)
            {
                var propToSet = resultType.GetProperty(property.Name);
                if (propToSet.SetMethod != null)
                {
                    propToSet.SetValue(this, property.GetValue(citation));
                }
            }
        }

    }
}
