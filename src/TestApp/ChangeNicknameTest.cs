using FluentAssertions;
using NLog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using TestApp.Services;
using TestApp.Settings;
using TestApp.Utils;

namespace TestApp
{
    [TestFixture, Category("All")]
    public class ChangeNicknameTest
    {
        private string driverPath;

        private IWebDriver driver;

        private Logger Logger;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driverPath = PathUtil.GetPathToDriver();

            driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;
            driver.Manage().Window.Maximize();

            Logger = LoggerManager.GetLogger();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                ((ChromeDriver)driver).MakeScreenshot();
                Logger.Error(TestContext.CurrentContext.Result.Message);
            }
            else
            {
                driver.ChangeGmailAccountNickName(TestSettings.OldNickName);
            }
            
            driver.Close();
        }

        [Test]
        public void ChangeNickname_WhenAllDataAreValid_ShouldSetNewNickname()
        {
            // Arrange
            var expectedWelcome = "Добро пожаловать, " + TestSettings.NewPseudonym + " TestSurname!";

            var user = CreateUserService.CreateUserWithCredentials(TestSettings.FirstUserEmail, TestSettings.FirstUserPassword);

            driver.GmailLogin(user);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);

            string welcome = string.Empty;

            // Act
            try
            {
                var newNickName = driver.ReadGmailMessage();

                driver.ChangeGmailAccountNickName(newNickName);

                Thread.Sleep(1000);
                driver.Navigate().GoToUrl(TestSettings.AccountUrl);
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//h1[@class=\"x7WrMb\"]")));

                welcome = driver.FindElement(By.XPath("//h1[@class=\"x7WrMb\"]")).GetAttribute("innerHTML");
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }

            // Assert
            welcome.Should().BeEquivalentTo(expectedWelcome);
            Logger.Info("ChangeNickname_WhenAllDataAreValid_ShouldSetNewNickname executed successfully.");
        }
    }
}
