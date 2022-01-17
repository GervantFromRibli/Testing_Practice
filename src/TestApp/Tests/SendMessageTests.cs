using FluentAssertions;
using NUnit.Framework;
using System.Threading;
using TestApp.Models;
using TestApp.Services;
using TestApp.Tests;

namespace TestApp
{
    [TestFixture, Category("All")]
    public class SendMessageTests : TestBase
    {
        [Test]
        public void SendMessage_WhenAllDataAreValid_ShouldSendAndReceiveMessages()
        {
            // Arrange
            var firstUser = CreateUserService.CreateUserWithCredentials(SettingsService.FirstUserEmail, SettingsService.FirstUserPassword);
            var secondUser = CreateUserService.CreateUserWithCredentials(SettingsService.SecondUserEmail, SettingsService.SecondUserPassword);
            var firstMessage = new Message
            {
                MessageText = SettingsService.MessageToSend,
                ReceiverEmail = SettingsService.SecondUserEmail
            };
            var secondMessage = new Message
            {
                MessageText = SettingsService.NewPseudonym,
                ReceiverEmail = SettingsService.FirstUserEmail
            };

            // Act 1
            Driver.GmailLogin(firstUser);

            string message;

            Driver.GmailSendMessage(firstMessage);

            Thread.Sleep(15000);
            Driver.MailLogin(secondUser);

            message = Driver.ReadMailMessage();

            // Assert 1
            message.Should().BeEquivalentTo(SettingsService.MessageToSend);

            // Act 2
            Thread.Sleep(2000);
            Driver.MailSendMessage(secondMessage);

            Thread.Sleep(70000);

            message = Driver.ReadGmailMessage();

            // Assert 2
            message.Should().BeEquivalentTo(SettingsService.NewPseudonym);
            
        }
    }
}
