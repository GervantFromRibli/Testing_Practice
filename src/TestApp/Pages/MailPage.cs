using OpenQA.Selenium;
using System;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Pages
{
    public class MailPage : PageBase
    {
        private readonly By _newMessageLocator = By.XPath("//a[@data-title-shortcut=\"N\"]");

        private readonly By _receiverLocator = By.XPath("//input[@class=\"container--H9L5q size_s--3_M-_\"]");

        private readonly By _messageTextLocator = By.XPath("//div[@role=\"textbox\"]/child::div");

        private readonly By _sendMessageLocator = By.XPath("//span[@title=\"Отправить\"]");

        private readonly By _incomeMessageLocator = By.XPath("//span[@class=\"llc__snippet\"]");

        public MailPage(IWebDriver driver) : base(driver)
        {
            BaseUrl = SettingsService.MailUrl;
        }

        public override MailPage OpenPage()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
            return this;
        }

        public MailPage SendMessage(Message message)
        {
            Driver.FindElement(_newMessageLocator).Click();

            Driver.FindElements(_receiverLocator)[0].SendKeys(message.ReceiverEmail);
            Driver.FindElements(_messageTextLocator)[0].SendKeys(message.MessageText);
            Driver.FindElement(_sendMessageLocator).Click();

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
    }
}
