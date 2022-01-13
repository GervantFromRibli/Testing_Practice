using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using TestApp.Models;
using TestApp.Settings;

namespace TestApp.Services
{
    public static class LoginService
    {
        public static IWebDriver GmailLogin(this IWebDriver driver, User user)
        {
            driver.Navigate().GoToUrl(TestSettings.LoginPage);
            var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
            email.SendKeys(user.Login);

            var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
            buttonSubmit.Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@type=\"password\"]")));
                var password = driver.FindElement(By.XPath("//input[@type=\"password\"]"));
                password.SendKeys(user.Password);
                password.SendKeys(Keys.Enter);
                wait.Until(ExpectedConditions.UrlContains(TestSettings.AccountUrl));
            }
            catch (WebDriverTimeoutException)
            {
            }

            return driver;
        }

        public static IWebDriver MailLogin(this IWebDriver driver, User user)
        {
            driver.Navigate().GoToUrl(TestSettings.MailLoginUrl);

            driver.FindElement(By.XPath("//input[@name=\"login\"]")).SendKeys(user.Login);
            driver.FindElement(By.XPath("//button[@data-testid=\"enter-password\"]")).Click();
            driver.FindElement(By.XPath("//input[@name=\"password\"]")).SendKeys(user.Password);
            driver.FindElement(By.XPath("//button[@data-testid=\"login-to-mail\"]")).Click();

            return driver;
        }
    }
}
