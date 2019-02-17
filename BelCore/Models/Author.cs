using Dek.Bel.Cls;
using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class Author : IModelWithId
    {
        public Id Id { get; set; }
        public string Name { get; set; }
        public DateTime? Born { get; set; }
        public DateTime? Dead { get; set; }
        public string Notes { get; set; }
    }
}
