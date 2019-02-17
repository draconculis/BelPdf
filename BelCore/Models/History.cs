using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    /// <summary>
    /// Stores last opened files with dates and hashes
    /// </summary>
    public class History
    {
        [Key]
        public string Hash { get; set; }
        public DateTime OpenDate { get; set; }
        public Id StorageId { get; set; }
        public Id VolumeId { get; set; }
    }
}
