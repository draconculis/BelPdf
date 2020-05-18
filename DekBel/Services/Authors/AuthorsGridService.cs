using Dek.Bel.Core.Models;
using Dek.Bel.Core.Services;
using Dek.Bel.Core.ViewModels;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services.Authors
{
    /// <summary>
    /// Helper class for the authors tab
    /// </summary>
    [Export]
    public class AuthorsGridService
    {
        [Import] AuthorService m_AuthorService { get; set; }
        [Import] SeriesService m_SeriesService { get; set; }

        public IEnumerable<AuthorsGridViewModel> GetAllAuthors(Id volumeId)
        {
            VolumeSeries seriesForVolume = m_SeriesService.GetVolumeSeriesByVolumeId(volumeId);

            IEnumerable<AuthorWithType> seriesAuthors = (seriesForVolume != null)
                ? m_AuthorService.GetSeriesAuthors(seriesForVolume.SeriesId)
                : new List<AuthorWithType>();

            IEnumerable<AuthorWithType> volumeAuthors = m_AuthorService.GetVolumeAuthors(volumeId);

            IEnumerable<AuthorWithType> bookAuthors = m_AuthorService.GetBookAuthors(volumeId);


            List<AuthorsGridViewModel> authorVms = new List<AuthorsGridViewModel>();
            foreach(AuthorWithType author in seriesAuthors.Union(volumeAuthors).Union(bookAuthors))
            {
                authorVms.Add(
                    new AuthorsGridViewModel
                    {
                        Id = author.Id,
                        Born = author.Born,
                        Dead = author.Dead,
                        Name = author.Name,
                        Notes = author.Notes,
                        AuthorType = author.AuthorType,
                        ItemId = author.ItemId,


                    }
                    
                    );
            }

            return authorVms;
        }
    }
}
