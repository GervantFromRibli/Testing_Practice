using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using System.Threading;
using TestApp.Models;
using TestApp.Services;
using TestApp.Settings;
using TestApp.Utils;

namespace TestApp
{
    [TestFixture]
    public class SendMessageTests
    {
        public string driverPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driverPath = TestStringUtils.GetPathToDriver();
        }

        [Test]
        public void SendMessage_WhenAllDataAreValid_ShouldSendAndReceiveMessages()
        {
            // Arrange
            var firstUserDriver = new ChromeDriver(driverPath);
            var secondUserDriver = new ChromeDriver(driverPath);
            firstUserDriver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;
            secondUserDriver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

            var firstUser = CreateUserService.CreateUserWithCredentials(TestSettings.FirstUserEmail, TestSettings.FirstUserPassword);
            var secondUser = CreateUserService.CreateUserWithCredentials(TestSettings.SecondUserEmail, TestSettings.SecondUserPassword);
            var firstMessage = new Message
            {
                MessageText = TestSettings.MessageToSend,
                ReceiverEmail = TestSettings.SecondUserEmail
            };
            var secondMessage = new Message
            {
                MessageText = TestSettings.NewPseudonym,
                ReceiverEmail = TestSettings.FirstUserEmail
            };

            firstUserDriver.GmailLogin(firstUser);
            secondUserDriver.MailLogin(secondUser);

            // Act 1
            firstUserDriver.GmailSendMessage(firstMessage);
            Thread.Sleep(15000);
            var message = secondUserDriver.ReadMailMessage();

            // Assert 1
            message.Should().BeEquivalentTo(TestSettings.MessageToSend);

            // Act 2
            secondUserDriver.MailSendMessage(secondMessage);

            secondUserDriver.Close();
            Thread.Sleep(70000);

            message = firstUserDriver.ReadGmailMessage();
            // Assert 2
            message.Should().BeEquivalentTo(TestSettings.NewPseudonym);
            firstUserDriver.Close();
        }
    }
}
