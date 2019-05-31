﻿using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.DB
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
            Books = m_DBService.Select<Book>($"`VolumeId`='{id}'").OrderBy(x => x.PhysicalPage).ThenBy(y => y.Glyph).ToList();
            Chapters = m_DBService.Select<Chapter>($"`VolumeId`='{id}'").OrderBy(x => x.PhysicalPage).ThenBy(y => y.Glyph).ToList();
            SubChapters = m_DBService.Select<SubChapter>($"`VolumeId`='{id}'").OrderBy(x => x.PhysicalPage).ThenBy(y => y.Glyph).ToList();
            Paragraphs = m_DBService.Select<Paragraph>($"`VolumeId`='{id}'").OrderBy(x => x.PhysicalPage).ThenBy(y => y.Glyph).ToList();
            Pages = m_DBService.Select<Page>($"`VolumeId`='{id}'").OrderBy(x => x.PhysicalPage).ThenBy(y => y.Glyph).ToList();
        }

        public void LoadCitations(Id id)
        {
            Citations = m_DBService.Select<Citation>($"`VolumeId`='{id}'").AsParallel().OrderBy(x => x.PhysicalPageStart).ThenBy(y => y.GlyphStart).ToList();
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

        public int GetPageNumber(int physicalPage, int glyph = 0)
        {
            Page page = GetReference(Pages, physicalPage, glyph);
            if (page == null)
                return physicalPage;

            if(physicalPage == page.PhysicalPage)
                return page.PaginationStart + (glyph > page.Glyph ? 1 : 0);

            int offset = physicalPage - page.PhysicalPage;
            return page.PaginationStart + offset;
        }

        private T GetReference<T>(List<T> references, int physicalPage, int glyph) where T : Reference
        {
            if (references == null || references.Count == 0)
                return null;

            var lastref = references.First();
            int idx = 1;
            while (idx < references.Count)
            {
                var cur = references[idx++];
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
