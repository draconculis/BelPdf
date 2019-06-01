using Dek.Bel.DB;
using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    [Export]
    public class ReportDataCache
    {
        [Import] public IDBService m_DBService { get; set; }

        public List<ReportModel> Report { get; private set; }

        bool Loaded = false;
        public List<Category> Categories { get; private set; }
        public List<CitationCategory> CitationCategories { get; private set; }

        // The references
        public List<Page> Pages { get; private set; }
        public List<Volume> Volumes { get; private set; }
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

            Categories = m_DBService.Select<Category>();
            CitationCategories = m_DBService.Select<CitationCategory>();

            Pages = m_DBService.Select<Page>();
            Volumes = m_DBService.Select<Volume>();
            Books = m_DBService.Select<Book>();
            Chapters = m_DBService.Select<Chapter>();
            SubChapters = m_DBService.Select<SubChapter>();
            Paragraphs = m_DBService.Select<Paragraph>();

            Loaded = true;
        }

}
}
