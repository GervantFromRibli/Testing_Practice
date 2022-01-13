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
using TestApp.Settings;
using TestApp.Utils;

namespace TestApp
{
    [TestFixture]
    public class SendMessageTests
    {
        private string driverPath;

        private Logger Logger;

        private IWebDriver firstUserDriver;

        private IWebDriver secondUserDriver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driverPath = PathUtil.GetPathToDriver();

            firstUserDriver = new ChromeDriver(driverPath);
            secondUserDriver = new ChromeDriver(driverPath);
            firstUserDriver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;
            secondUserDriver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

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
            message.Should().BeEquivalentTo(TestSettings.MessageToSend);

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
            message.Should().BeEquivalentTo(TestSettings.NewPseudonym);
            
        }
    }
}
