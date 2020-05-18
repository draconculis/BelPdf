using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Core.Cls
{
    public static class EnumExtension
    {
        public static string GetEnumStringValue(this Enum enumVal)
        {
            var enumType = enumVal.GetType();

            var memInfo = enumType.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(EnumStringValueAttribute), false);
            return (attributes.Length > 0) ? ((EnumStringValueAttribute)attributes[0]).EnumString : string.Empty;
        }
    }

    //[AttributeUsage(AttributeTargets.Enum)]
    public class EnumStringValueAttribute : Attribute
    {
        public string EnumString { get; set; }

        public EnumStringValueAttribute(string enumString)
        {
            EnumString = enumString;
        }
    }
}
