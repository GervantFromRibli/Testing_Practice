using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System.Threading;
using TestApp.Models;
using TestApp.Pages;
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
            var gmailPage = new GmailLoginPage(Driver).
                OpenPage().
                Login(firstUser).
                GoToMailPage();

            gmailPage.SendMessage(firstMessage);

            Thread.Sleep(2000);

            var mailPage = new MailLoginPage(Driver).
                OpenPage().
                Login(secondUser).WaitForNewMessage(40);

            var message = mailPage.ReadNewMessage();

            // Assert 1
            message.Should().BeEquivalentTo(SettingsService.MessageToSend);

            // Act 2
            mailPage.SendMessage(secondMessage);

            Thread.Sleep(1000);

            message = new GmailPage(Driver).
                OpenPage().
                WaitForNewMessage(60).
                ReadNewMessage();

            // Assert 2
            message.Should().BeEquivalentTo(SettingsService.NewPseudonym);
            
        }
    }
}
