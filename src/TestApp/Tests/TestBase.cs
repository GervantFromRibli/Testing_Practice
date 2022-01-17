using NLog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestApp.Driver;
using TestApp.Services;

namespace TestApp.Tests
{
    public class TestBase
    {
        protected IWebDriver Driver;

        protected Logger Logger;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SettingsService.SetSettings();

            Logger = LoggerManager.GetLogger();
        }

        [SetUp]
        public void SetUp()
        {
            Driver = DriverInstance.GetDriver();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                ((ChromeDriver)Driver).MakeScreenshot();
                Logger.Error(TestContext.CurrentContext.Result.Message);
            }
            else
            {
                Logger.Info($"{TestContext.CurrentContext.Test.Name} executed successfully.");
            }

            DriverInstance.CloseDriver();
        }
    }
}
