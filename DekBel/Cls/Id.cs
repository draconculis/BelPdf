using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Cls;

namespace Dek.Bel.Cls
{
    public class Id
    {
        public static Id Empty { get; } = new Id(Guid.Empty);
        Guid Value { get; set; }

        public static Id NewId() => new Id();
        public static Id NewId(string id) => new Id(id);
        public static Id NewId(Guid guid) => new Id(guid);

        public Id() : this(Guid.NewGuid())
        {
        }

        public Id(string value) : this(value.ToGuid())
        {
        }

        public Id(Guid value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool IsNull => Guid.Empty == Value;
    }
}
