using Dek.Bel.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Dek.DB;
using Dek.Cls;
using System.Linq;

namespace Dek.Bel.Core.Services
{
    [Export]
    public class ReportDataCache
    {
        [Import] public IDBService m_DBService { get; set; }

        public List<ReportModel> Report { get; private set; }

        bool Loaded = false;

        // The volumes
        public Dictionary<Id, Volume> Volumes { get; private set; }

        // Categories
        public Dictionary<Id, Category> Categories { get; private set; }
        public List<CitationCategory> CitationCategories { get; private set; }

        // Series
        public List<Series> Series { get; private set; }
        public List<VolumeSeries> VolumeSeries { get; private set; }

        // Authors
        public Dictionary<Id, Author> Authors { get; private set; }
        public List<VolumeAuthor> VolumeAuthors { get; private set; }
        public List<BookAuthor> BookAuthors { get; private set; }

        // The references
        public List<Page> Pages { get; private set; }
        public List<Book> Books { get; private set; }
        public List<Chapter> Chapters { get; private set; }
        public List<SubChapter> SubChapters { get; private set; }
        public List<Paragraph> Paragraphs { get; private set; }

        /// <summary>
        /// Preload datasets
        /// </summary>
        public void LoadCache(bool forceReload = false)
        {
            if (Loaded && !forceReload)
                return;

            Volumes = new Dictionary<Id, Volume>();
            foreach (Volume vol in m_DBService.Select<Volume>())
                Volumes.Add(vol.Id, vol);

            // Categories
            Categories = new Dictionary<Id, Category>();
            foreach(Category cat in m_DBService.Select<Category>())
                Categories.Add(cat.Id, cat);
            CitationCategories = m_DBService.Select<CitationCategory>();

            // Series
            Series = m_DBService.Select<Series>();
            VolumeSeries = m_DBService.Select<VolumeSeries>();

            Authors = new Dictionary<Id, Author>();
            foreach(Author auth in m_DBService.Select<Author>())
                Authors.Add(auth.Id, auth);

            VolumeAuthors = m_DBService.Select<VolumeAuthor>();
            BookAuthors = m_DBService.Select<BookAuthor>();

            Pages = m_DBService.Select<Page>().OrderBy(x => x).ToList();
            Books = m_DBService.Select<Book>().OrderBy(x => x).ToList();
            Chapters = m_DBService.Select<Chapter>().OrderBy(x => x).ToList();
            SubChapters = m_DBService.Select<SubChapter>().OrderBy(x => x).ToList();
            Paragraphs = m_DBService.Select<Paragraph>().OrderBy(x => x).ToList();

            Loaded = true;
        }

}
}
