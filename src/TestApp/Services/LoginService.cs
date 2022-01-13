using NLog;
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
        private static Logger logger = LoggerManager.GetLogger();

        public static IWebDriver GmailLogin(this IWebDriver driver, User user)
        {
            try
            {
                driver.Navigate().GoToUrl(TestSettings.LoginPage);
                var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
                email.SendKeys(user.Login);

                var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
                buttonSubmit.Click();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@type=\"password\"]")));
                var password = driver.FindElement(By.XPath("//input[@type=\"password\"]"));
                password.SendKeys(user.Password);
                password.SendKeys(Keys.Enter);
                wait.Until(ExpectedConditions.UrlContains(TestSettings.AccountUrl));
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
                driver.Navigate().GoToUrl(TestSettings.LoginPage);
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
                driver.Navigate().GoToUrl(TestSettings.LoginPage);
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
                driver.Navigate().GoToUrl(TestSettings.MailLoginUrl);

                driver.FindElement(By.XPath("//input[@name=\"login\"]")).SendKeys(user.Login);
                driver.FindElement(By.XPath("//button[@data-testid=\"enter-password\"]")).Click();
                driver.FindElement(By.XPath("//input[@name=\"password\"]")).SendKeys(user.Password);
                driver.FindElement(By.XPath("//button[@data-testid=\"login-to-mail\"]")).Click();

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
