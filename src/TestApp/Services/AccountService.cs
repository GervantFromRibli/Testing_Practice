using OpenQA.Selenium;
using System.Threading;
using TestApp.Settings;

namespace TestApp.Services
{
    public static class AccountService
    {
        public static IWebDriver ChangeGmailAccountNickName(this IWebDriver driver, string nickName)
        {
            driver.Navigate().GoToUrl(TestSettings.AccountUrl);
            driver.FindElement(By.XPath("//a[@data-rid=\"10003\" and @class=\"VZLjze Wvetm zCVEd EhlvJf\"]")).Click();
            driver.FindElement(By.XPath("//a[@href=\"name\"]")).Click();
            var nameField = driver.FindElements(By.XPath("//input[@class=\"VfPpkd-fmcmS-wGMbrd CtvUB\"]"))[0];
            nameField.Clear();
            nameField.SendKeys(nickName);
            nameField.SendKeys(Keys.Enter);

            return driver;
        }
    }
}
