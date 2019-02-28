using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using ScrewNutUI;
using ScrewNutUI.Builders;

namespace ScrewNut.Tests
{
    [TestFixture]
    public class KompasApplicationTests
    {
        [TestCase(TestName = "Проверка запуска приложения")]
        public void LoadApplicationTest()
        {
            Thread.Sleep(1500);

            var application = new KompasApplication();

            var processIsExist = KompasProcessIsExist();
            Assert.IsTrue(processIsExist);

            application.DestructApp();
        }

        [TestCase(TestName = "Проверка закрытия приложения")]
        public void DestructApplicationTest()
        {
            Thread.Sleep(1500);

            var application = new KompasApplication();

            application.DestructApp();
            Thread.Sleep(1500);
            var processIsExist = KompasProcessIsExist();

            Assert.IsFalse(processIsExist);
        }

        [TestCase(TestName = "Проверка создания документа в запущенном приложении")]
        public void CreateDocumentTest()
        {
            Thread.Sleep(1500);

            var application = new KompasApplication();
            application.CreateDocument3D();

            Assert.NotNull(application.Document3D);
            Assert.NotNull(application.ScrewPart);

            application.DestructApp();
        }

        [TestCase(-50, 21, 100, 50, 1,
            TestName = "Передача параметров с некорректным отрицательным значением = -50")]
        [TestCase(85, double.NegativeInfinity, double.PositiveInfinity, 375, 10,
            TestName = "Передача параметров с некорректным значением = Infinity")]
        [TestCase(150, 25, 1000, 500, double.NaN,
            TestName = "Передача параметров с некорректным значением = NaN")]
        public void SetNegativeParametersTests(params double[] inputParameters)
        {
            Thread.Sleep(1500);

            var application = new KompasApplication();

            Assert.Throws<InvalidOperationException>(() =>
            {
                application.Parameters = inputParameters.ToList();
            });

            application.DestructApp();
        }

        [TestCase(150, 25, 1000, 500, 15,
            TestName = "Передача списка параметров")]
        public void SetPositiveParametersTests(params double[] inputParameters)
        {
            Thread.Sleep(1500);

            var application = new KompasApplication {Parameters = inputParameters.ToList()};

            CollectionAssert.AreEqual(inputParameters.ToList(), application.Parameters);
            application.DestructApp();
        }

        [TestCase(150, 25, 1000, 500, 15, 1,
            TestName = "Передача списка с некорректным числом параметров > 5")]
        public void SetMoreCountParametersTests(params double[] inputParameters)
        {
            Thread.Sleep(1500);

            var application = TestHelper.GetKompasInstance();

            Assert.Throws<InvalidOperationException>(() =>
            {
                application.Parameters = inputParameters.ToList();
            });

            application.DestructApp();
        }

        [TestCase(150, 25, 1000,
            TestName = "Передача списка с некорректным числом параметров < 5")]
        public void SetLessCountParametersTests(params double[] inputParameters)
        {
            Thread.Sleep(1500);

            var application = TestHelper.GetKompasInstance();

            Assert.Throws<InvalidOperationException>(() =>
            {
                application.Parameters = inputParameters.ToList();
            });

            application.DestructApp();
        }

        /// <summary>
        /// Exist or not Kompas process
        /// </summary>
        /// <returns></returns>
        private bool KompasProcessIsExist()
        {
            return Process.GetProcessesByName("KOMPAS").Any();
        }
    }
}