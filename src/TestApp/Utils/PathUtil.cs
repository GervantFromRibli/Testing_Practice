using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
