using System;
using System.Threading.Tasks;
using NUnit.Framework;
using ScrewNutUI;
using ScrewNutUI.Parameters;

namespace ScrewNut.Tests.ParametersTest
{
    [TestFixture]
    public class PolygonParameterTests
    {
        private KompasApplication _kompasApplication;

        [SetUp]
        public void InitializeKompasInstance()
        {
            _kompasApplication = TestHelper.GetKompasInstance();
        }

        [OneTimeTearDown]
        public void DestructKompasInstance()
        {
            _kompasApplication.DestructApp();
        }

        [TestCase(3, 4, false, TestName = "Вызов конструктора с корректными параметрами angle = 3, circle radius = 4")]
        [TestCase(5, 12, false, TestName = "Вызов конструктора с корректными параметрами angle = 5, circle radius = 12")]
        [TestCase(2, 12, true, TestName = "Вызов конструктора с некорректными параметрами angle = 2, circle radius = 12")]
        [TestCase(int.MinValue, double.MaxValue, true, 
            TestName = "Вызов конструктора с некорректными параметрами angle = MinValue, circle radius = MaxValue")]
        [TestCase(int.MaxValue, double.NegativeInfinity, true,
            TestName = "Вызов конструктора с некорректными параметрами angle = MaxValue, circle radius = NegativeInfinity")]
        public void PolygonConstructorTests(int anglesCount, double inscribedCircleRadius, bool isError)
        {
            PolygonParameter polygon;

            if (isError)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    polygon = new PolygonParameter(_kompasApplication, anglesCount,
                        inscribedCircleRadius, new Point2D(0, 0));
                });
            }
            else
            {
                polygon = new PolygonParameter(_kompasApplication, anglesCount,
                    inscribedCircleRadius, new Point2D(0, 0));

                Assert.NotNull(polygon);
                Assert.AreEqual(polygon.FigureParam.count, anglesCount);
                Assert.AreEqual(polygon.FigureParam.radius, inscribedCircleRadius);
            }
        }

        [TestCase(TestName = "Вызов конструктора с передачей нулевой ссылки на экземпляр приложения")]
        public void PolygonConstructorNegativeTestsWithNullApplication()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var polygon = new PolygonParameter(null, 1,
                    1, new Point2D(0, 0));
            });
        }
    }
}