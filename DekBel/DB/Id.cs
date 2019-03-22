using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Cls;

namespace Dek.Bel.DB
{
    public struct Id : IEquatable<Id>
    {
        public static Id Empty { get; } = new Id(Guid.Empty);
        public static Id Null => Empty;

        public Guid Value { get; set; } // Defaults to null guid

        public static Id NewId() => new Id { Value = Guid.NewGuid() };
        public static Id NewId(string id) => new Id(id);
        public static Id NewId(Guid guid) => new Id(guid);

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

        public bool Equals(Id other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Id)obj);
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<Guid>.Default.GetHashCode(Value);
        }

        public static bool operator ==(Id a, Id b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Id a, Id b)
        {
            return !a.Equals(b);
        }

        public bool IsNull => Guid.Empty == Value;
    }

    public static class IdExtensions
    {
        public static Id ToId(this string me)
        {
            if (string.IsNullOrWhiteSpace(me))
                return Id.Null;

            return new Id(me);
        }
    }
}
