using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TestApp.Services;

namespace TestApp.Pages
{
    public class AccountPage : PageBase
    {
        private readonly By _privateDataLocator = By.XPath("//a[@data-rid=\"10003\" and @class=\"VZLjze Wvetm zCVEd EhlvJf\"]");

        private readonly By _namePanelLocator = By.XPath("//a[@href=\"name\"]");

        private readonly By _nameFieldLocator = By.XPath("//input[@class=\"VfPpkd-fmcmS-wGMbrd CtvUB\"]");

        private readonly By _welcomeMessageLocator = By.XPath("//h1[@class=\"x7WrMb\"]");

        public AccountPage(IWebDriver driver) : base(driver)
        {
            BaseUrl = SettingsService.AccountUrl;
        }

        public override AccountPage OpenPage()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
            return this;
        }

        public AccountPage ChangeNickname(string nickname)
        {
            Driver.FindElement(_privateDataLocator).Click();
            Driver.FindElement(_namePanelLocator).Click();

            var nameField = Driver.FindElements(_nameFieldLocator)[0];
            nameField.Clear();
            nameField.SendKeys(nickname);
            nameField.SendKeys(Keys.Enter);

            logger.Info("Account nickname changed successfully.");

            return this;
        }

        public string ReadWelcomeMessage()
        {
            var wait = new WebDriverWait(Driver, SettingsService.ImplicitWaitSpan);

            var welcome = wait.Until(ExpectedConditions.ElementExists(_welcomeMessageLocator)).Text;

            return welcome;
        }

        public GmailPage GoToMailPage()
        {
            return new GmailPage(Driver).OpenPage();
        }
    }
}
