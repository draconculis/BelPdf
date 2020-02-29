using Dek.Bel.Cls;
using Dek.Bel.DB;

namespace Dek.Bel.Models
{
    public class VolumeSeries
    {
        [Key]
        public Id SeriesId { get; set; }
        [Key]
        public Id VolumeId { get; set; }
    }
}
