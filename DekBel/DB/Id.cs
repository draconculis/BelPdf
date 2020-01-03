using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Cls;

namespace Dek.Bel.DB
{
    public struct Id : IEquatable<Id>, IComparable
    {
        public static Id Empty { get; } = new Id(Guid.Empty);
        public static Id Null => Empty;

        public Guid Value { get; set; } // Defaults to null guid

        // Ctor
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

        public string ToStringShort()
        {
            return ToStringShort(4);
        }

        public string ToStringShort(int digits)
        {
            return Value.ToString().Replace("-","").Substring(0, 5);
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

        public int CompareTo(object obj)
        {
            return Value.ToString().CompareTo(((Id)obj).ToString());
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
        public bool IsNotNull => !IsNull;
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
