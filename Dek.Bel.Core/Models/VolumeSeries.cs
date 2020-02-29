using Dek.Cls;
using Dek.DB;

namespace Dek.Bel.Core.Models
{
    public class VolumeSeries
    {
        [Key]
        public Id SeriesId { get; set; }
        [Key]
        public Id VolumeId { get; set; }
    }
}
