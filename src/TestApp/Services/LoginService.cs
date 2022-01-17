using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using TestApp.Models;

namespace TestApp.Services
{
    public static class LoginService
    {
        private static readonly Logger logger = LoggerManager.GetLogger();

        public static IWebDriver GmailLogin(this IWebDriver driver, User user)
        {
            try
            {
                driver.Navigate().GoToUrl(SettingsService.LoginPage);
                var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
                email.SendKeys(user.Login);

                var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
                buttonSubmit.Click();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@type=\"password\"]")));
                var password = driver.FindElement(By.XPath("//input[@type=\"password\"]"));
                password.SendKeys(user.Password);
                password.SendKeys(Keys.Enter);
                wait.Until(ExpectedConditions.UrlContains(SettingsService.AccountUrl));
                logger.Info($"User {user.Login} successfully logged in.");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }

            return driver;
        }

        public static IWebDriver GmailLoginWithoutPassword(this IWebDriver driver, User user)
        {
            try
            {
                driver.Navigate().GoToUrl(SettingsService.LoginPage);
                var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
                email.SendKeys(user.Login);

                var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
                buttonSubmit.Click();
                logger.Info($"User {user.Login} successfully put his email.");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return driver;
        }

        public static IWebDriver GmailLoginWithoutRedirection(this IWebDriver driver, User user)
        {
            try
            {
                driver.Navigate().GoToUrl(SettingsService.LoginPage);
                var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
                email.SendKeys(user.Login);

                var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
                buttonSubmit.Click();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@type=\"password\"]")));
                var password = driver.FindElement(By.XPath("//input[@type=\"password\"]"));
                password.SendKeys(user.Password);
                password.SendKeys(Keys.Enter);
                logger.Info($"User {user.Login} successfully wrote password.");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return driver;
        }

        public static IWebDriver MailLogin(this IWebDriver driver, User user)
        {
            try
            {
                driver.Navigate().GoToUrl(SettingsService.MailLoginUrl);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@name=\"username\"]")));
                driver.FindElement(By.XPath("//input[@name=\"username\"]")).SendKeys(user.Login);
                driver.FindElement(By.XPath("//button[@data-test-id=\"next-button\"]")).Click();
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@name=\"password\"]")));
                driver.FindElement(By.XPath("//input[@name=\"password\"]")).SendKeys(user.Password);
                driver.FindElement(By.XPath("//button[@data-test-id=\"submit-button\"]")).Click();

                logger.Info($"User {user.Login} successfully logged in.");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }

            return driver;
        }
    }
}
