using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TestApp.Models;
using TestApp.Services;

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

        public GmailPage(IWebDriver driver) : base(driver)
        {
            BaseUrl = SettingsService.GmailUrl;
        }

        public override GmailPage OpenPage()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
            return this;
        }

        public GmailPage SendMessage(Message message)
        {
            Driver.FindElement(_newMessageLocator).Click();

            Driver.FindElement(_receiverLocator).SendKeys(message.ReceiverEmail);
            Driver.FindElement(_messageTextLocator).SendKeys(message.MessageText);
            Driver.FindElement(_sendMessageLocator).Click();

            logger.Info("Message sent successfully.");

            return this;
        }

        public string ReadNewMessage()
        {
            var wait = new WebDriverWait(Driver, SettingsService.ImplicitWaitSpan);

            Driver.FindElements(_incomeMessageLocator)[5].Click();

            string message = wait.Until(ExpectedConditions.ElementIsVisible(_incomeMessageTextLocator)).Text;

            logger.Info("Message read successfully.");

            return message;
        }
    }
}
