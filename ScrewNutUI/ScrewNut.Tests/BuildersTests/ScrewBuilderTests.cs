using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ScrewNutUI;
using ScrewNutUI.Builders;

namespace ScrewNut.Tests.BuildersTests
{
    [TestFixture]
    public class ScrewBuilderTests
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

        [TestCase(50, 21, 100, 50, 1, TestName = "Построение детали с минимально возможными параметрами")]
        [TestCase(85, 23, 500, 375, 10, TestName = "Построение детали с обычными параметрами")]
        [TestCase(150, 25, 1000, 500, 30, TestName = "Построение детали с максимально возможными параметрами")]
        public void CreateDetailCorrectTests(params double[] inputParameters)
        {
            var parameters = new List<double>()
            {
                inputParameters[0], inputParameters[1], inputParameters[2], inputParameters[3], inputParameters[4]
            };
            _kompasApplication.Parameters = parameters;
            var screwBuilder = new ScrewBuilder(_kompasApplication);
            bool buildResult = screwBuilder.CreateDetail();
            Assert.IsTrue(buildResult);
        }
    }
}