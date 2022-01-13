using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using TestApp.Utils;

namespace TestApp.Services
{
    public static class ScreenshotService
    {
        public static ChromeDriver MakeScreenshot(this ChromeDriver driver)
        {
            var screenshot = driver.GetScreenshot();
            var path = Path.Combine(PathUtil.GetPathToScreenshots(), DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".png");
            screenshot.SaveAsFile(path);
            return driver;
        }
    }
}
