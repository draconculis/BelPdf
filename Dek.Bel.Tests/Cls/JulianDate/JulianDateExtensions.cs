using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Bel.Core.Cls;

namespace Dek.Bel.Cls.JulianDate
{
    public class JulianDateExtensionsTests
    {
        // Early leap years -45, -42, -39, -36, -33, -30, -27, -24, -21, -18, -15, -12, -9, 8, 12 
        // Note priestly error, 3 years instead of 4
        [TestCase(-45, ExpectedResult = true)]
        [TestCase(-42, ExpectedResult = true)]
        [TestCase(-39, ExpectedResult = true)]
        [TestCase(-36, ExpectedResult = true)]
        [TestCase(-33, ExpectedResult = true)]
        [TestCase(-30, ExpectedResult = true)]
        [TestCase(-27, ExpectedResult = true)]
        [TestCase(-24, ExpectedResult = true)]
        [TestCase(-21, ExpectedResult = true)]
        [TestCase(-18, ExpectedResult = true)]
        [TestCase(-15, ExpectedResult = true)]
        [TestCase(-12, ExpectedResult = true)]
        [TestCase(-9, ExpectedResult = true)]

        // Early non leap years
        [TestCase(-48, ExpectedResult = false)]
        [TestCase(-44, ExpectedResult = false)]
        [TestCase(-16, ExpectedResult = false)]
        [TestCase(-13, ExpectedResult = false)]
        [TestCase(-11, ExpectedResult = false)]
        [TestCase(-10, ExpectedResult = false)]
        [TestCase(-8, ExpectedResult = false)]
        [TestCase(-7, ExpectedResult = false)]
        [TestCase(-6, ExpectedResult = false)]
        [TestCase(-5, ExpectedResult = false)]
        [TestCase(-4, ExpectedResult = false)]
        [TestCase(-3, ExpectedResult = false)]
        [TestCase(-2, ExpectedResult = false)]
        [TestCase(-1, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = false)] // does not exist, but should be false anyway
        [TestCase(1, ExpectedResult = false)]
        [TestCase(2, ExpectedResult = false)]
        [TestCase(3, ExpectedResult = false)]
        [TestCase(4, ExpectedResult = false)]
        [TestCase(5, ExpectedResult = false)]
        [TestCase(6, ExpectedResult = false)]
        [TestCase(7, ExpectedResult = false)]
        [TestCase(9, ExpectedResult = false)]
        [TestCase(10, ExpectedResult = false)]
        [TestCase(11, ExpectedResult = false)]

        // First proper leap years
        [TestCase(8, ExpectedResult = true)]
        [TestCase(12, ExpectedResult = true)]
        [TestCase(16, ExpectedResult = true)]
        [TestCase(20, ExpectedResult = true)]
        [TestCase(24, ExpectedResult = true)]

        // Centuries after 1582 are not leap years, if year is not divisable by 400
        [TestCase(1400, ExpectedResult = true)]
        [TestCase(1500, ExpectedResult = true)]
        [TestCase(1700, ExpectedResult = false)]
        [TestCase(1800, ExpectedResult = false)]
        [TestCase(1900, ExpectedResult = false)]
        [TestCase(2100, ExpectedResult = false)]
        [TestCase(2200, ExpectedResult = false)]
        [TestCase(2300, ExpectedResult = false)]
        [TestCase(2500, ExpectedResult = false)]

        // Leap years every 4:th century after 1582
        [TestCase(1600, ExpectedResult = true)]
        [TestCase(2000, ExpectedResult = true)]
        [TestCase(2400, ExpectedResult = true)]
        [TestCase(2800, ExpectedResult = true)]

        // Some random leap years
        [TestCase(1592, ExpectedResult = true)]
        [TestCase(1604, ExpectedResult = true)]
        [TestCase(1780, ExpectedResult = true)]
        [TestCase(1860, ExpectedResult = true)]
        [TestCase(1996, ExpectedResult = true)]
        [TestCase(2004, ExpectedResult = true)]
        [TestCase(2016, ExpectedResult = true)]
        [TestCase(2284, ExpectedResult = true)]

        // Some random non leap years
        [TestCase(1603, ExpectedResult = false)]
        [TestCase(1642, ExpectedResult = false)]
        [TestCase(1710, ExpectedResult = false)]
        [TestCase(1766, ExpectedResult = false)]
        [TestCase(2001, ExpectedResult = false)]
        [TestCase(2010, ExpectedResult = false)]
        [TestCase(2011, ExpectedResult = false)]
        [TestCase(2022, ExpectedResult = false)]

        public bool IsLeapYear(int year)
        {
            return DekJulianDate.IsLeapYear(year);
        }

    }
}
