using ScrewNutUI;

namespace ScrewNut.Tests
{
    public static class TestHelper
    {
        private static KompasApplication _kompasApplication;

        public static KompasApplication GetKompasInstance()
        {
            _kompasApplication = new KompasApplication(false);
            _kompasApplication.CreateDocument3D();
            return _kompasApplication;
        }
    }
}