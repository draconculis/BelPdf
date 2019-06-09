using Dek.Bel.DB;
using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    [Export]
    public class ReportService
    {
        [Import] public ReportDataCache Cache { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public VolumeService m_VolumeService { get; set; }

        public List<ReportModel> Report { get; private set; } = new List<ReportModel>();
        public IEnumerable<ReportModel> Filtered => Report.Where(x => Filter(x));

        public Predicate<ReportModel> Filter { get; set; } = x => true;


        /*
         
             BindingList<Person> filtered = new BindingList<Person>(personas.Where(
                                 p => p.Apellido.Contains("App1")).ToList());
grid.DataSource = filtered;*/
        [ImportingConstructor]
        ReportService()
        {

        }

        public long GenerateReport(bool forceReload = false)
        {
            /*TIME*/ Stopwatch t = new Stopwatch();
            /*TIME*/ t.Start();
            Cache.LoadCache(forceReload);
            /*TIME*/ long t1 = t.ElapsedMilliseconds;

            IEnumerable<Citation> orderedCitations = new List<Citation>();
            IEnumerable<Citation> citations = m_DBService.Select<Citation>();
            orderedCitations = citations
                .OrderBy(x => x.VolumeId)
                .ThenBy(y => y.PhysicalPageStart)
                .ThenBy(y => y.GlyphStart);

            /*TIME*/ long t2 = t.ElapsedMilliseconds;

            int counter = 1;
            var nullCategory = Cache.Categories.Single(c => c.Id == Id.Null);
            foreach (Citation c in orderedCitations)
            {
                Volume volume = Cache.Volumes.SingleOrDefault(x => x.Id == c.VolumeId);
                var mainCitCat = Cache.CitationCategories.Where(x => x.CitationId == c.Id)?.SingleOrDefault(x => x.IsMain);
                var mainCategory = (mainCitCat == null)
                    ? nullCategory
                    : Cache.Categories.Single(x => x.Id == mainCitCat.CategoryId);

                ReportModel m = new ReportModel
                {
                    Idx = counter++,
                    VolumeId = c.VolumeId,
                    VolumeTitle = volume?.Title,
                    PublicationDate = volume?.PublicationDate ?? DateTime.MinValue,
                    CitationId = c.Id,
                    OriginalCitation = c.Citation1,
                    Citation = c.Citation3,
                    Page = VolumeService.GetPageNumberForVolume(volume.Id, Cache.Pages, c.PhysicalPageStart),
                    PhysicalPage = c.PhysicalPageStart,
                    Book = VolumeService.GetReferenceForVolume(volume.Id, Cache.Books, c.PhysicalPageStart, c.GlyphStart)?.Title ?? "",
                    Chapter = VolumeService.GetReferenceForVolume(volume.Id, Cache.Chapters, c.PhysicalPageStart, c.GlyphStart)?.Title ?? "",
                    SubChapter = VolumeService.GetReferenceForVolume(volume.Id, Cache.SubChapters, c.PhysicalPageStart, c.GlyphStart)?.Title ?? "",
                    Paragraph = VolumeService.GetReferenceForVolume(volume.Id, Cache.Paragraphs, c.PhysicalPageStart, c.GlyphStart)?.Title ?? "",
                    MainCategory = mainCategory.ToString(),
                    Weight = mainCitCat.Weight,

                    // Hidden
                    Emphasis = c.Emphasis,
                };

                Report.Add(m);
            }
            t.Stop();
            /*TIME*/ long tFinal = t.ElapsedMilliseconds;

            return tFinal;
        }

        /// <summary>
        /// Preload datasets
        /// </summary>
        private void PrepareReport()
        {

        }

    }
}
