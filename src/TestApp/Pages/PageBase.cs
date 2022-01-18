using NLog;
using OpenQA.Selenium;
using TestApp.Services;

namespace TestApp.Pages
{
    public abstract class PageBase
    {
        protected readonly IWebDriver Driver;

        protected string BaseUrl;

        protected readonly Logger logger;

        public abstract PageBase OpenPage();

        protected PageBase(IWebDriver driver)
        {
            Driver = driver;
            logger = LoggerManager.GetLogger();
        }

        public string GetCurrentUrl()
        {
            return Driver.Url;
        }
    }
}
