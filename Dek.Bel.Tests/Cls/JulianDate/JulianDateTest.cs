using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Bel.Core.Cls;

namespace Dek.Bel.Cls.JulianDate
{
    public class JulianDateTest
    {
        /// <summary>
        /// https://ssd.jpl.nasa.gov/tc.cgi#top
        /// </summary>
        [TestCase(2020, 01, 01, 0, 0, 0, ExpectedResult = 2458849.5)]
        [TestCase(2020, 01, 01, 12, 0, 0, ExpectedResult = 2458850.0)]
        [TestCase(-4713, 01, 01, 12, 0, 0, ExpectedResult = 0)]
        [TestCase(-4713, 01, 01, 0, 0, 0, ExpectedResult = -.5)]
        [TestCase(1, 01, 01, 12, 0, 0, ExpectedResult = 1721424.0)]
        [TestCase(-1, 01, 01, 12, 0, 0, ExpectedResult = 1721058.0)]
        [TestCase(-10, 01, 01, 12, 0, 0, ExpectedResult = 1717771.0)]
        [TestCase(3200, 01, 01, 12, 0, 1, ExpectedResult = 2889836.000011574)]
        [TestCase(4040, 01, 01, 12, 0, 0, ExpectedResult = 3196640.0)]
        public double CreateJulianDate(int year, int month, int day, int hour, int minute, int second)
        {
            return DekJulianDate.ToJulianDate(year, month, day, hour, minute, second);
        }

        [TestCase(2458849.5)]
        [TestCase(2458850.0)]
        [TestCase(0)]
        [TestCase(-0.5)]
        [TestCase(0.5)]
        [TestCase(1721424.0)]
        [TestCase(1721058.0)]
        [TestCase(1717771.0)]
        [TestCase(2889836.000011574)]
        [TestCase(3196640.0)]
        public void ConvertAndConvertBack(double julianDate)
        {
            (int year, int month, int day, int hour, int minute, int second, int ms) = DekJulianDate.ToTuple(julianDate);
            double sut = DekJulianDate.ToJulianDate(year, month, day, hour, minute, second, ms);

            Assert.AreEqual(julianDate, sut);
        }

    }
}
