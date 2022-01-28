using OpenQA.Selenium;
using System;
using TestApp.Models;
using TestApp.Services;
using TestApp.Waiters;

namespace TestApp.Pages
{
    public class GmailPage : PageBase
    {
        private readonly By _newMessageLocator = By.XPath("//div[@class=\"T-I T-I-KE L3\"]");

        private readonly By _receiverLocator = By.XPath("//textarea[@name=\"to\"]");

        private readonly By _messageTextLocator = By.XPath("//div[@class=\"Am Al editable LW-avf tS-tW\"]");

        private readonly By _sendMessageLocator = By.XPath("//div[@class=\"T-I J-J5-Ji aoO v7 T-I-atl L3\"]");

        private readonly By _incomeMessageLocator = By.XPath("//tbody/child::tr[1]");

        private readonly By _incomeMessageTextLocator = By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]");

        private readonly By _newMessageTimeLocator = By.XPath("(//td[@class=\"xW xY \"])[1]");

        public GmailPage(IWebDriver driver) : base(driver)
        {
            BaseUrl = SettingsService.GmailUrl;
        }

        public override GmailPage OpenPage()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
            Driver.WaitForUrlToBe(BaseUrl);
            return this;
        }

        public GmailPage SendMessage(Message message)
        {
            Driver.FindElement(_newMessageLocator).Click();

            Driver.FindElement(_receiverLocator).SendKeys(message.ReceiverEmail);
            Driver.FindElement(_messageTextLocator).SendKeys(message.MessageText);
            Driver.FindElement(_sendMessageLocator).Click();

            Driver.WaitForSomeTime();

            logger.Info("Message sent successfully.");

            return this;
        }

        public string ReadNewMessage()
        {
            Driver.WaitForAllElementsToExist(_incomeMessageLocator, 6).Click();

            string message = Driver.WaitForElementToBeVisible(_incomeMessageTextLocator).Text;

            logger.Info("Message read successfully.");

            return message;
        }

        public GmailPage WaitForNewMessage(int timeout)
        {
            var time = DateTime.Now;
            var firstTimeToCheck = time.AddMinutes(-1).ToString("HH:mm");
            var secondTimeToCheck = time.ToString("HH:mm");
            var thirdTimeToCheck = time.AddMinutes(1).ToString("HH:mm");

            Driver.WaitForOneOfThreeTextesToAppear(_newMessageTimeLocator, firstTimeToCheck, secondTimeToCheck, thirdTimeToCheck, timeout);

            return this;
        }
    }
}
