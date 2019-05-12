using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class DataSetModel
    {
        // Volume data
        Id VolumeId { get; set; }
        Id SeriesId { get; set; }
        Id SeriesName { get; set; }


        Id CitationId { get; set; }
    }
}
