using FluentAssertions;
using NLog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using TestApp.Models;
using TestApp.Services;
using TestApp.Utils;

namespace TestApp
{
    [TestFixture, Category("All")]
    public class SendMessageTests
    {
        private string driverPath;

        private Logger Logger;

        private IWebDriver firstUserDriver;

        private IWebDriver secondUserDriver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SettingsService.SetSettings();

            driverPath = PathUtil.GetPathToDriver();

            firstUserDriver = new ChromeDriver(driverPath);
            secondUserDriver = new ChromeDriver(driverPath);
            firstUserDriver.Manage().Timeouts().ImplicitWait = SettingsService.ImplicitWaitSpan;
            secondUserDriver.Manage().Timeouts().ImplicitWait = SettingsService.ImplicitWaitSpan;

            Logger = LoggerManager.GetLogger();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                ((ChromeDriver)firstUserDriver).MakeScreenshot();
                ((ChromeDriver)secondUserDriver).MakeScreenshot();
                Logger.Error(TestContext.CurrentContext.Result.Message);
            }

            secondUserDriver.Close();
            firstUserDriver.Close();
        }

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

            firstUserDriver.GmailLogin(firstUser);
            secondUserDriver.MailLogin(secondUser);

            string message;

            // Act 1
            try
            {
                firstUserDriver.GmailSendMessage(firstMessage);
                Thread.Sleep(15000);
                message = secondUserDriver.ReadMailMessage();
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }

            // Assert 1
            message.Should().BeEquivalentTo(SettingsService.MessageToSend);

            // Act 2
            try
            {
                secondUserDriver.MailSendMessage(secondMessage);

                Thread.Sleep(70000);

                message = firstUserDriver.ReadGmailMessage();
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
            
            // Assert 2
            message.Should().BeEquivalentTo(SettingsService.NewPseudonym);
            
        }
    }
}
