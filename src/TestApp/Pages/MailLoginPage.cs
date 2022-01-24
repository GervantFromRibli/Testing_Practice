using OpenQA.Selenium;
using TestApp.Models;
using TestApp.Services;
using TestApp.Waiters;

namespace TestApp.Pages
{
    public class MailLoginPage : PageBase
    {
        private readonly By _usernameLocator = By.XPath("//input[@name=\"username\"]");

        private readonly By _nextButtonLocator = By.XPath("//button[@data-test-id=\"next-button\"]");

        private readonly By _passwordLocator = By.XPath("//input[@name=\"password\"]");

        private readonly By _submitLocator = By.XPath("//button[@data-test-id=\"submit-button\"]");

        public MailLoginPage(IWebDriver driver) : base(driver)
        {
            BaseUrl = SettingsService.MailLoginUrl;
        }

        public override MailLoginPage OpenPage()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
            Driver.WaitForUrlToBe(BaseUrl);
            return this;
        }

        public MailPage Login(User user)
        {
            Driver.FindElement(_usernameLocator).SendKeys(user.Login);
            Driver.FindElement(_nextButtonLocator).Click();

            Driver.FindElement(_passwordLocator).SendKeys(user.Password);
            Driver.FindElement(_submitLocator).Click();

            logger.Info($"User {user.Login} successfully logged in.");

            return new MailPage(Driver);
        }
    }
}
