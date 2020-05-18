using Dek.Cls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Core.Models
{
    public class Series
    {
        public Id Id { get; set; }

        public string Name { get; set; }

        public string Notes{ get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
