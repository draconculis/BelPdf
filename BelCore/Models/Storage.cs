﻿using Dek.Bel.Cls;
using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class Storage
    {
        public Id Id { get; set; }
        public string Hash { get; set; }
        public Id VolumeId { get; set; }
        public string FileName { get; set; } // Original file
        public string FilePath { get; set; } // Original path
        public string StorageName { get; set; } // Name of file in storage
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }
}
