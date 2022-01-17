using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using TestApp.Models;

namespace TestApp.Services
{
    public static class MessageService
    {
        private static Logger logger = LoggerManager.GetLogger();

        public static IWebDriver GmailSendMessage(this IWebDriver driver, Message message)
        {
            try
            {
                driver.Navigate().GoToUrl(SettingsService.GmailUrl);

                driver.FindElement(By.XPath("//div[@class=\"T-I T-I-KE L3\"]")).Click();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//textarea[@name=\"to\"]")));

                driver.FindElement(By.XPath("//textarea[@name=\"to\"]")).SendKeys(message.ReceiverEmail);
                driver.FindElement(By.XPath("//div[@class=\"Am Al editable LW-avf tS-tW\"]")).SendKeys(message.MessageText);
                driver.FindElement(By.XPath("//div[@class=\"T-I J-J5-Ji aoO v7 T-I-atl L3\"]")).Click();
                logger.Info("Message sent successfully.");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }

            return driver;
        }

        public static IWebDriver MailSendMessage(this IWebDriver driver, Message message)
        {
            try
            {
                driver.Navigate().GoToUrl(SettingsService.MailUrl);

                var wait = new WebDriverWait(driver, SettingsService.ImplicitWaitSpan);
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[@data-title-shortcut=\"N\"]")));

                driver.FindElement(By.XPath("//a[@data-title-shortcut=\"N\"]")).Click();
                driver.FindElements(By.XPath("//input[@class=\"container--H9L5q size_s--3_M-_\"]"))[0].SendKeys(message.ReceiverEmail);
                driver.FindElements(By.XPath("//div[@role=\"textbox\"]/child::div"))[0].SendKeys(message.MessageText);
                driver.FindElement(By.XPath("//span[@title=\"Отправить\"]")).Click();
                logger.Info("Message sent successfully.");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }

            return driver;
        }

        public static string ReadMailMessage(this IWebDriver driver)
        {
            string message = string.Empty;
            try
            {
                driver.Navigate().GoToUrl(SettingsService.MailUrl);

                var wait = new WebDriverWait(driver, SettingsService.ImplicitWaitSpan);
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//span[@class=\"llc__snippet\"]")));

                message = driver.FindElements(By.XPath("//span[@class=\"llc__snippet\"]"))[0].GetAttribute("innerText");
                logger.Info("Message read successfully.");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }

            return message;
        }

        public static string ReadGmailMessage(this IWebDriver driver)
        {
            string message = string.Empty;
            try
            {
                driver.Navigate().GoToUrl(SettingsService.GmailUrl);

                var wait = new WebDriverWait(driver, SettingsService.ImplicitWaitSpan);
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tbody/child::tr[1]")));
                driver.FindElements(By.XPath("//tbody/child::tr[1]"))[5].Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]")));
                message = driver.FindElement(By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]")).GetAttribute("innerText");
                logger.Info("Message read successfully.");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }

            return message;
        }
    }
}
