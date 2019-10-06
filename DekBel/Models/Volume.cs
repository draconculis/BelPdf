﻿using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class Volume : IModelWithId
    {
        public Id Id { get; set; }
        public Id SeriesId { get; set; }
        public string Title { get; set; }
        public string Editor { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Notes { get; set; }

        // Draw all pdf boxes with this offset for this volume
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;
    }
}
