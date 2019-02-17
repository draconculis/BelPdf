using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.DB
{
    public class RefKeyAttribute : Attribute
    {
        public string RefKey { get; }

        RefKeyAttribute(string refKey)
        {
            RefKey = refKey;
        }
    }
}
