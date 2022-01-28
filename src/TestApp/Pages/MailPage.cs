using OpenQA.Selenium;
using System;
using TestApp.Models;
using TestApp.Services;
using TestApp.Waiters;

namespace TestApp.Pages
{
    public class MailPage : PageBase
    {
        private readonly By _newMessageLocator = By.XPath("//a[@data-title-shortcut=\"N\"]");

        private readonly By _receiverLocator = By.XPath("//input[@class=\"container--H9L5q size_s--3_M-_\"]");

        private readonly By _messageTextLocator = By.XPath("//div[@role=\"textbox\"]/child::div");

        private readonly By _sendMessageLocator = By.XPath("//span[@title=\"Отправить\"]");

        private readonly By _incomeMessageLocator = By.XPath("//span[@class=\"llc__snippet\"]");

        private readonly By _newMessageTimeLocator = By.XPath("(//div[@class=\"llc__item llc__item_date\"])[1]");

        public MailPage(IWebDriver driver) : base(driver)
        {
            BaseUrl = SettingsService.MailUrl;
        }

        public override MailPage OpenPage()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
            Driver.WaitForUrlToBe(BaseUrl);
            return this;
        }

        public MailPage SendMessage(Message message)
        {
            Driver.FindElement(_newMessageLocator).Click();

            Driver.FindElements(_receiverLocator)[0].SendKeys(message.ReceiverEmail);
            Driver.FindElements(_messageTextLocator)[0].SendKeys(message.MessageText);
            Driver.FindElement(_sendMessageLocator).Click();

            Driver.WaitForSomeTime();

            logger.Info("Message sent successfully.");

            return this;
        }

        public string ReadNewMessage()
        {
            var message = Driver.FindElements(_incomeMessageLocator)[0].
                GetAttribute("innerText");

            logger.Info("Message read successfully.");

            return message;
        }

        public MailPage WaitForNewMessage(int timeout)
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
