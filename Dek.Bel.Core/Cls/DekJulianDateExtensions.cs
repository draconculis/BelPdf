using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Core.Cls
{
    public static class DekJulianDateExtensions
    {
        public static bool IsValidSaneDate(this string me)
        {
            return DekJulianDate.IsValidSaneDate(me);
        }

    }
}
