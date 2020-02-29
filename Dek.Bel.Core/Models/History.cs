using Dek.Cls;
using System;
using Dek.DB;

namespace Dek.Bel.Core.Models
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
