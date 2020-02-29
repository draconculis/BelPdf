﻿using BelManagedLib;
using Dek.Bel.Services;
using Dek.Bel.Cls;
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

        public List<DekRange> Emphasis { get; set; } = new List<DekRange>();
        public List<DekRange> Exclusion { get; set; } = new List<DekRange>();

        internal void InitCitationData()
        {
            if (Exclusion == null)
                Exclusion = new List<DekRange>();
            else
                Exclusion.Clear();

            if (Emphasis == null)
                Emphasis = new List<DekRange>();
            else
                Emphasis.Clear();

            Exclusion.LoadFromText(CurrentCitation.Exclusion);
            Emphasis.LoadFromText(CurrentCitation.Emphasis);
        }
    }
}
