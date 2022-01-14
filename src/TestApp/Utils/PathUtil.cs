using System.IO;

namespace TestApp.Utils
{
    public static class PathUtil
    {
        public static string GetPathToDriver()
        {
            var driverPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.
                GetCurrentDirectory()).ToString()).ToString()).ToString();
            driverPath = Path.Combine(driverPath, "Settings//Driver");

            return driverPath;
        }

        public static string GetPathToScreenshots()
        {
            var screenshotPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.
                GetCurrentDirectory()).ToString()).ToString()).ToString();
            screenshotPath = Path.Combine(screenshotPath, "Settings//Screenshots");

            return screenshotPath;
        }

        public static string GetPathLog()
        {
            var logPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.
                GetCurrentDirectory()).ToString()).ToString()).ToString();
            logPath = Path.Combine(logPath, "Settings//Log//log.txt");

            return logPath;
        }

        public static string GetPathToData(string environment)
        {
            var dataPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.
                GetCurrentDirectory()).ToString()).ToString()).ToString();
            var fileShortPath = $"Settings//Test_Data//{environment}.json";
            dataPath = Path.Combine(dataPath, fileShortPath);

            return dataPath;
        }
    }
}
