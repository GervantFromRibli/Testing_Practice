using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using TestApp.Utils;

namespace TestApp.Driver
{
    public class DriverInstance
    {
        private static IWebDriver driver;

        private DriverInstance() { }

        public static IWebDriver GetDriver()
        {
            if (driver == null)
            {
                driver = new ChromeDriver(PathUtil.GetPathToDriver());
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));
                driver.Manage().Window.Maximize();
            }

            return driver;
        }

        public static void CloseDriver()
        {
            driver.Quit();
            driver = null;
        }
    }
}
