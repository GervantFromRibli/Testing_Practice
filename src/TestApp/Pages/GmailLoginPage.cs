using OpenQA.Selenium;
using TestApp.Models;
using TestApp.Services;
using TestApp.Waiters;

namespace TestApp.Pages
{
    public class GmailLoginPage : PageBase
    {
        private readonly string AccountUrl;

        private readonly By _emailLocator = By.XPath("//input[@type=\"email\"]");

        private readonly By _submitLocator = By.ClassName("VfPpkd-LgbsSe");

        private readonly By _passwordLocator = By.XPath("//input[@type=\"password\"]");

        private readonly By _emailWarningLocator = By.XPath("//div[@class=\"o6cuMc\"]");

        private readonly By _passwordWarningLocator = By.XPath("//div[@jsname=\"B34EJ\"]/child::span");

        public GmailLoginPage(IWebDriver driver) : base(driver)
        {
            BaseUrl = SettingsService.LoginPage;
            AccountUrl = SettingsService.AccountUrl;
        }

        public override GmailLoginPage OpenPage()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
            Driver.WaitForUrlToBe(BaseUrl);
            return this;
        }

        public AccountPage Login(User user)
        {
            Driver.FindElement(_emailLocator).SendKeys(user.Login);

            Driver.FindElement(_submitLocator).Click();

            var password = Driver.WaitForElementToBeClickable(_passwordLocator);
            password.SendKeys(user.Password);
            password.SendKeys(Keys.Enter);

            Driver.WaitForUrlToBe(AccountUrl);

            logger.Info($"User {user.Login} successfully logged in.");

            return new AccountPage(Driver);
        }

        public GmailLoginPage LoginWithoutRedirection(User user)
        {
            Driver.FindElement(_emailLocator).SendKeys(user.Login);

            Driver.FindElement(_submitLocator).Click();

            var password = Driver.WaitForElementToBeClickable(_passwordLocator);
            password.SendKeys(user.Password);
            password.SendKeys(Keys.Enter);

            logger.Info($"User {user.Login} successfully wrote password.");

            return this;
        }

        public GmailLoginPage LoginWithoutPassword(User user)
        {
            Driver.FindElement(_emailLocator).SendKeys(user.Login);

            Driver.FindElement(_submitLocator).Click();

            logger.Info($"User {user.Login} successfully put his email.");

            return this;
        }

        public IWebElement GetEmailWarning()
        {
            return Driver.WaitForElementToExist(_emailWarningLocator);
        }

        public IWebElement GetPasswordWarning()
        {
            return Driver.WaitForElementToExist(_passwordWarningLocator);
        }
    }
}