using Dek.Bel.Cls;
using Dek.Bel.DB;

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
        public string CategoryMarginBoxSettings { get; set; } // For the future

        public override string ToString() => FullName;
    }
}
