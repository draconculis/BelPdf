using System;

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
