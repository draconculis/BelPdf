using Dek.Bel.Core.Cls;
using Dek.Bel.Core.Models;
using Dek.Cls;
using Dek.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Core.Services
{
    [Export]
    public class AuthorService
    {
        [Import] IDBService m_DBService { get; set; }


        public IEnumerable<AuthorWithType> GetSeriesAuthors(Id seriesId)
        {
            var rels = GetSeriesAuthorRelations(seriesId);
            List<AuthorWithType> authors = new List<AuthorWithType>();
            foreach(var rel in rels)
            {
                Author auth = m_DBService.SelectById<Author>(rel.AuthorId);
                authors.Add(GetAuthorWithType(auth, rel.SeriesId, rel.AuthorType));
            }

            return authors;
        }

        public IEnumerable<AuthorWithType> GetBookAuthors(Id bookId)
        {
            var rels = GetBookAuthorRelations(bookId);
            List<AuthorWithType> authors = new List<AuthorWithType>();
            foreach (var rel in rels)
            {
                Author auth = m_DBService.SelectById<Author>(rel.AuthorId);
                authors.Add(GetAuthorWithType(auth, rel.BookId, rel.AuthorType));
            }

            return authors;
        }

        public IEnumerable<AuthorWithType> GetVolumeAuthors(Id volumeId)
        {
            var rels = GetVolumeAuthorRelations(volumeId);
            List<AuthorWithType> authors = new List<AuthorWithType>();
            foreach (var rel in rels)
            {
                Author auth = m_DBService.SelectById<Author>(rel.AuthorId);
                authors.Add(GetAuthorWithType(auth, rel.VolumeId, rel.AuthorType));
            }

            return authors;
        }

        public void AddAuthorToSeries(Id authorId, Id seriesId, AuthorType authorType)
        {
            var rel = new SeriesAuthor
            {
                AuthorId = authorId,
                SeriesId = seriesId,
                AuthorType = authorType,
            };

            m_DBService.InsertOrUpdate(rel);
        }

        public void AddAuthorToVolume(Id authorId, Id volumeId, AuthorType authorType)
        {
            var rel = new VolumeAuthor
            {
                AuthorId = authorId,
                VolumeId = volumeId,
                AuthorType = authorType,
            };

            m_DBService.InsertOrUpdate(rel);
        }

        public void AddAuthorToBook(Id authorId, Id bookId, AuthorType authorType)
        {
            var rel = new BookAuthor
            {
                AuthorId = authorId,
                BookId = bookId,
                AuthorType = authorType,
            };

            m_DBService.InsertOrUpdate(rel);
        }

        public void RemoveAuthorFromSeries(Id authorId, Id seriesId)
        {
            m_DBService.Delete(typeof(SeriesAuthor), $"`AuthorId`='{authorId}' AND `{nameof(SeriesAuthor.SeriesId)}`= '{seriesId}'");
        }

        public void RemoveAuthorFromVolume(Id authorId, Id volumeId)
        {
            m_DBService.Delete(typeof(VolumeAuthor), $"`AuthorId`='{authorId}' AND `{nameof(VolumeAuthor.VolumeId)}`= '{volumeId}'");
        }

        public void RemoveAuthorFromBook(Id authorId, Id bookId)
        {
            m_DBService.Delete(typeof(BookAuthor), $"`AuthorId`='{authorId}' AND `{nameof(BookAuthor.BookId)}`= '{bookId}'");
        }

        /// <summary>
        /// Completely removes author, also from all items
        /// </summary>
        /// <param name="authorId"></param>
        public void RemoveAuthor(Id authorId)
        {
            m_DBService.Delete(typeof(Author), $"`Id`='{authorId}'");
        }

        /// <summary>
        /// Detaches author from all items
        /// </summary>
        /// <param name="authorId"></param>
        public void DetachAuthor(Id authorId)
        {
            m_DBService.Delete(typeof(BookAuthor), $"`AuthorId`='{authorId}'");
            m_DBService.Delete(typeof(VolumeAuthor), $"`AuthorId`='{authorId}'");
            m_DBService.Delete(typeof(SeriesAuthor), $"`AuthorId`='{authorId}'");
        }




        private AuthorWithType GetAuthorWithType(Author author, Id itemId, AuthorType authorType)
        {
            return new AuthorWithType
            {
                Id = author.Id,
                Born = author.Born,
                Dead = author.Dead,
                Name = author.Name,
                Notes = author.Notes,
                AuthorType = authorType,
                ItemId = itemId,
            };
        }

        private IEnumerable<SeriesAuthor> GetSeriesAuthorRelations(Id id)
        {
            return GetAuthorRelations<SeriesAuthor>(id);
        }

        private IEnumerable<BookAuthor> GetBookAuthorRelations(Id id)
        {
            return GetAuthorRelations<BookAuthor>(id);
        }
        private IEnumerable<VolumeAuthor> GetVolumeAuthorRelations(Id id)
        {
            return GetAuthorRelations<VolumeAuthor>(id);
        }

        private IEnumerable<T> GetAuthorRelations<T>(Id id) where T : new()
        {
            return m_DBService.Select<T>($"`VolumeId`='{id}'");
        }



    }
}
