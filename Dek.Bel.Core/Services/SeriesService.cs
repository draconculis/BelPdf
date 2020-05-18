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
    public class SeriesService
    {
        [Import] IDBService m_DBService { get; set; }

        SeriesService()
        {

        }

        public IEnumerable<Volume> GetVolumesInSeries(Id seriesId)
        {
            IEnumerable<VolumeSeries> volumeSeries = GetVolumeSeriesBySeriesId(seriesId);
            if (!volumeSeries.Any())
                return new List<Volume>();

            string where = String.Empty;
            foreach (var vs in volumeSeries)
            {
                if (where.Length > 0)
                    where += " OR ";
                where += $"`Id`='{vs.VolumeId}'";
            }

            IEnumerable<Volume> allVolumesInSeries = m_DBService.Select<Volume>(where);
            return allVolumesInSeries;
        }

        public IEnumerable<Volume> GetAllVolumesInSeriesByVolumeId(Id volumeId)
        {
            Series series = GetSeriesForVolume(volumeId);
            if (series == null)
                return new List<Volume>();

            return GetVolumesInSeries(series.Id);
        }

        public IEnumerable<Volume> GetOtherVolumesInSeriesByVolumeId(Id volumeId)
        {
            return GetAllVolumesInSeriesByVolumeId(volumeId).Where(v => v.Id != volumeId);
        }

        public Series GetSeriesForVolume(Id volumeId)
        {
            Id seriesId = GetVolumeSeriesByVolumeId(volumeId)?.SeriesId ?? Id.Empty;
            if (seriesId.IsNull)
                return null;

            return m_DBService.SelectById<Series>(seriesId);
        }

        public VolumeSeries GetVolumeSeriesByVolumeId(Id volumeId)
        {
            return m_DBService.Select<VolumeSeries>().SingleOrDefault(r => r.VolumeId == volumeId);
        }

        public IEnumerable<VolumeSeries> GetVolumeSeriesBySeriesId(Id seriesId)
        {
            return m_DBService.Select<VolumeSeries>().Where(r => r.SeriesId == seriesId);
        }

        public void InsertOrUpdateVolumeSeries(VolumeSeries volumeSeries)
        {
            m_DBService.InsertOrUpdate(volumeSeries);
        }

        public void InsertOrUpdateSeries(Series series)
        {
            m_DBService.InsertOrUpdate(series);
        }

        public void CreateSeries(Series volumeSeries)
        {
            m_DBService.InsertOrUpdate(volumeSeries);
        }

        public Series CreateSeries(string name, string notes)
        {
            Series series = new Series
            {
                Id = Id.NewId(),
                Name = name,
                Notes = notes
            };

            m_DBService.InsertOrUpdate(series);

            return series;
        }

        public void DetachVolumeFromSeries(Id volumeId)
        {
            var volSeries = GetVolumeSeriesByVolumeId(volumeId);
            m_DBService.Delete(volSeries);
        }

        public void AttachVolumeToSeries(Id volumeId, Id seriesId)
        {
            VolumeSeries volumeSeries = new VolumeSeries
            {
                SeriesId = seriesId,
                VolumeId = volumeId,
            };

            m_DBService.InsertOrUpdate(volumeSeries);
        }

        public IEnumerable<Series> GetAllSeries()
        {
            return m_DBService.Select<Series>();
        }

        /// <summary>
        /// Deletes Series and all relations to Volumes.
        /// </summary>
        /// <param name="series"></param>
        public void DeleteSeries(Series series)
        {
            m_DBService.Delete(series);
            m_DBService.Delete(new VolumeSeries(), $"`SeriesId`='{series.Id}'");
        }
    }
}
