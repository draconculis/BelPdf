using System;

namespace Dek.DB
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
