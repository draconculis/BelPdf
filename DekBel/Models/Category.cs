using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class Category : IModelWithId
    {
        public Id Id { get; set;  }
        public string Code { get; set; }
        public string Name { get; set;  }
        public string Description { get; set; }

        private string FullName => $"{Code} - {Name}";

        public string CategoryColor { get; set; } // Highlight + underline color arrays (incl alpha)

        public override string ToString() => FullName;
    }
}
