using System;
using NUnit.Framework;
using ScrewNutUI.Parameters;

namespace ScrewNut.Tests.ParametersTest
{
    [TestFixture]
    public class Point2DTests
    {
        [TestCase(10.5, 10.5, TestName = "Вызов конструктора с параметрами [10.5, 10.5]")]
        [TestCase(1, 0.1, TestName = "Вызов конструктора с параметрами [1, 0.1]")]
        public void PointPositiveConstructorTests(double xc, double yc)
        {
            var point = new Point2D(xc, yc);

            Assert.AreEqual(point.X, xc);
            Assert.AreEqual(point.Y, yc);
        }

        [TestCase(double.NaN, 10.5, TestName = "Вызов конструктора с параметрами [NaN, 10.5]")]
        [TestCase(1, double.NegativeInfinity, TestName = "Вызов конструктора с параметрами [1, Infinity]")]
        [TestCase(double.PositiveInfinity, 0.1, TestName = "Вызов конструктора с параметрами [1, Infinity]")]
        public void PointNegativeConstructorTests(double xc, double yc)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var point = new Point2D(xc, yc);
            });
        }
    }
}