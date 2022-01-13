using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Settings;

namespace TestApp.Services
{
    public static class MessageService
    {
        public static IWebDriver GmailSendMessage(this IWebDriver driver, Message message)
        {
            driver.Navigate().GoToUrl(TestSettings.GmailUrl);

            driver.FindElement(By.XPath("//div[@class=\"T-I T-I-KE L3\"]")).Click();

            driver.FindElement(By.XPath("//textarea[@name=\"to\"]")).SendKeys(message.ReceiverEmail);
            driver.FindElement(By.XPath("//div[@class=\"Am Al editable LW-avf tS-tW\"]")).SendKeys(message.MessageText);
            driver.FindElement(By.XPath("//div[@class=\"T-I J-J5-Ji aoO v7 T-I-atl L3\"]")).Click();

            return driver;
        }

        public static IWebDriver MailSendMessage(this IWebDriver driver, Message message)
        {
            driver.Navigate().GoToUrl(TestSettings.MailUrl);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[@data-title-shortcut=\"N\"]")));

            driver.FindElement(By.XPath("//a[@data-title-shortcut=\"N\"]")).Click();
            driver.FindElements(By.XPath("//input[@class=\"container--H9L5q size_s--3_M-_\"]"))[0].SendKeys(message.ReceiverEmail);
            driver.FindElements(By.XPath("//div[@role=\"textbox\"]/child::div"))[0].SendKeys(message.MessageText);
            driver.FindElement(By.XPath("//span[@title=\"Отправить\"]")).Click();

            return driver;
        }

        public static string ReadMailMessage(this IWebDriver driver)
        {
            driver.Navigate().GoToUrl(TestSettings.MailUrl);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            wait.Until(ExpectedConditions.UrlContains(TestSettings.MailUrl));
            
            var message = driver.FindElements(By.XPath("//span[@class=\"llc__snippet\"]"))[0].GetAttribute("innerText");

            return message;
        }

        public static string ReadGmailMessage(this IWebDriver driver)
        {
            driver.Navigate().GoToUrl(TestSettings.GmailUrl);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tbody/child::tr[1]")));
            driver.FindElements(By.XPath("//tbody/child::tr[1]"))[5].Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]")));
            var message = driver.FindElement(By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]")).GetAttribute("innerText");

            return message;
        }
    }
}
