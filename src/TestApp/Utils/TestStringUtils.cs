using System.IO;

namespace TestApp.Utils
{
    public static class TestStringUtils
    {
        public static string GenerateString()
        {
            return new string('a', 10);
        }

        public static string GetPathToDriver()
        {
            var driverPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.
                GetCurrentDirectory()).ToString()).ToString()).ToString();
            driverPath = Path.Combine(driverPath, "Settings//Driver");

            return driverPath;
        }
    }
}
