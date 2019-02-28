using System;
using System.Threading;
using NUnit.Framework;
using ScrewNutUI;
using ScrewNutUI.Parameters;

namespace ScrewNut.Tests.ParametersTest
{
    [TestFixture]
    public class RectangleParameterTests
    {
        private KompasApplication _kompasApplication;

        [SetUp]
        public void InitializeKompasInstance()
        {
            Thread.Sleep(1500);
            _kompasApplication = TestHelper.GetKompasInstance();
        }

        [OneTimeTearDown]
        public void DestructKompasInstance()
        {
            _kompasApplication.DestructApp();
        }

        [TestCase(3, 4, false, TestName = "Вызов конструктора с корректными параметрами width = 3, height = 4")]
        [TestCase(5, 12, false, TestName = "Вызов конструктора с корректными параметрами width = 5, height = 12")]
        [TestCase(0, 12, true, TestName = "Вызов конструктора с некорректными параметрами width = 0, height = 12")]
        [TestCase(double.MinValue, double.MaxValue, true,
            TestName = "Вызов конструктора с некорректными параметрами width = MinValue, height = MaxValue")]
        [TestCase(double.MaxValue, double.NegativeInfinity, true,
            TestName = "Вызов конструктора с некорректными параметрами width = MaxValue, height = NegativeInfinity")]
        public void RectangleConstructorTests(double width, double height, bool isError)
        {
            RectangleParameter rectangle;

            if (isError)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    rectangle = new RectangleParameter(_kompasApplication, width,
                        height, new Point2D(0, 0));
                });
            }
            else
            {
                rectangle = new RectangleParameter(_kompasApplication, width,
                    height, new Point2D(0, 0));

                Assert.NotNull(rectangle);
                Assert.AreEqual(rectangle.FigureParam.width, width);
                Assert.AreEqual(rectangle.FigureParam.height, height);
            }
        }

        [TestCase(TestName = "Вызов конструктора с передачей нулевой ссылки на экземпляр приложения")]
        public void PolygonConstructorNegativeTestsWithNullApplication()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var rectangle = new RectangleParameter(null, 1,
                    1, new Point2D(0, 0));
            });
        }
    }
}