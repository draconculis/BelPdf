using BelManagedLib;
using Dek.Bel.Categories;
using Dek.Bel.DB;
using Dek.Bel.Models;
using Dek.Bel.UserSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel
{
    [Export]
    public class ModelsForViewing
    {

        // Current data
        public EventData Message { get; set; }
        public Citation CurrentCitation { get; set; }
        public Storage CurrentStorage { get; set; }
        public Volume CurrentVolume { get; set; }


    }
}
