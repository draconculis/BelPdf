﻿using BelManagedLib;
using Dek.Bel.Services;
using Dek.Bel.DB;
using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Cls;

namespace Dek.Bel.Services
{
    [Export]
    public class ModelsForViewing
    {
        // Current data
        public EventData Message { get; set; }
        public Citation CurrentCitation { get; set; }
        public Storage CurrentStorage { get; set; }

        public List<TextRange> Emphasis { get; set; } = new List<TextRange>();
        public List<TextRange> Exclusion { get; set; } = new List<TextRange>();

        public List<CitationCategory> CurrentCitations { get; set; } = new List<CitationCategory>();
    }
}