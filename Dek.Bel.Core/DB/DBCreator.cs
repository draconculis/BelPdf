using Dek.Bel.Core.Models;
using System.ComponentModel.Composition;
using Dek.Cls;
using Dek.DB;

namespace Dek.Bel.Core.DB
{
    [Export(typeof(IDBCreator))]
    public class DBCreator : IDBCreator
    {
        public void Create(IDBService repo)
        {
            repo.CreateTable(typeof(Volume));
            repo.CreateTable(typeof(Series));
            repo.CreateTable(typeof(VolumeSeries));

            // References
            repo.CreateTable(typeof(Book));
            repo.CreateTable(typeof(Chapter));
            repo.CreateTable(typeof(SubChapter));
            repo.CreateTable(typeof(Paragraph));
            repo.CreateTable(typeof(Page));

            repo.CreateTable(typeof(Author));
            repo.CreateTable(typeof(BookAuthor));
            repo.CreateTable(typeof(SeriesAuthor));
            repo.CreateTable(typeof(VolumeAuthor));


            // Citation
            // CREATE TABLE `Citation` ( `Id` TEXT NOT NULL, `Citation1` TEXT NOT NULL, `Citation2` TEXT NOT NULL, `CreatedDate` TEXT NOT NULL, `EditedDate` TEXT NOT NULL, PRIMARY KEY(`CitationId`) )
            repo.CreateTable(typeof(Citation));
            repo.CreateTable(typeof(RawCitation));

            //if (CreateTable(TableRawCitationName,
            //    "`Id` TEXT, " +
            //    "`Fragment` TEXT NOT NULL, " +
            //    "`PageStart` INT NOT NULL, " +
            //    "`PageStop` INT NOT NULL, " +
            //    "`GlyphStart` INT NOT NULL, " +
            //    "`GlyphStop` INT NOT NULL, " +
            //    "`Rectangles` TEXT NOT NULL, " +
            //    "`Date` TEXT NOT NULL ",
            //    "`Id`"))
            //{
            //    CreateIndex(TableCitationName, "Code");
            //}



            // Categories
            // 

            if (repo.CreateTable(typeof(Category)))
            {
                // Uncategorized
                repo.InsertOrUpdate(new Category
                {
                    Id = Id.Empty,
                    Code = "None",
                    Name = "Uncategorized",
                    Description = "No category selected"
                });
                repo.InsertOrUpdate(new Category
                {
                    Id = Id.NewId(),
                    Code = "CT1",
                    Name = "Category one",
                    Description = "First category"
                });
                repo.InsertOrUpdate(new Category
                {
                    Id = Id.NewId(),
                    Code = "CT2",
                    Name = "Category two",
                    Description = "Second category"
                });
                repo.InsertOrUpdate(new Category
                {
                    Id = Id.NewId(),
                    Code = "CT3",
                    Name = "Category three",
                    Description = "Third category"
                });
            }

            repo.CreateTable(typeof(CitationCategory));


            // Storage
            //CREATE TABLE "Storage"( `Id` TEXT NOT NULL, `Hash` TEXT NOT NULL, `SourceFileName` TEXT NOT NULL, `SourceFilePath` TEXT NOT NULL, `StorageFileName` TEXT NOT NULL UNIQUE, `Author` TEXT, `Date` TEXT, `Comment` TEXT, PRIMARY KEY(`Id`))
            repo.CreateTable(typeof(Storage));

            repo.CreateTable(typeof(History));
        }



    }
}
