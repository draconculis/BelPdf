using Dek.Bel.DB;
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
        public string Title { get; set; }
        public string Editor { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
