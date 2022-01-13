using FluentAssertions;
using NLog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using TestApp.Services;
using TestApp.Settings;
using TestApp.Utils;

namespace TestApp
{
    [TestFixture, Category("All"), Category("SmokeTests")]
    public class LoginTests
    {
        private string driverPath;

        private Logger Logger;

        private IWebDriver driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driverPath = PathUtil.GetPathToDriver();

            Logger = LoggerManager.GetLogger();
        }

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                ((ChromeDriver)driver).MakeScreenshot();
                Logger.Error(TestContext.CurrentContext.Result.Message);
            }

            driver.Close();
        }

        [Test]
        public void LoginTest_WhenAllDataAreCorrect_ShouldRedirectToPage()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithCredentials(TestSettings.FirstUserEmail, TestSettings.FirstUserPassword);

            try
            {
                // Act
                driver.GmailLogin(user);
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }

            // Assert
            driver.Url.Should().BeEquivalentTo(TestSettings.ExpectedAccountUrl);
            Logger.Info("LoginTest_WhenAllDataAreCorrect_ShouldRedirectToPage executed successfully.");
        }

        [Test]
        public void LoginTest_WhenNoEmailSent_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoLoginAndPassword();

            IWebElement warning = null;

            // Act
            try
            {
                driver.GmailLoginWithoutPassword(user);

                var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
                warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class=\"o6cuMc\"]")));
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }

            // Assert
            warning.Text.Should().BeEquivalentTo("Введите адрес электронной почты или номер телефона.");
            Logger.Info("LoginTest_WhenNoEmailSent_ShouldCreateWarning executed successfully");
        }

        [Test]
        public void LoginTest_WhenNoPasswordSent_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoPassword(TestSettings.FirstUserEmail);

            IWebElement warning = null;

            // Act
            try
            {
                driver.GmailLoginWithoutRedirection(user);

                var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
                warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@jsname=\"B34EJ\"]/child::span")));
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }

            // Assert
            warning.Text.Should().BeEquivalentTo("Введите пароль");
            Logger.Info("LoginTest_WhenNoPasswordSent_ShouldCreateWarning executed successfully.");
        }

        [Test]
        public void LoginTest_WhenEmailIsWrong_ShouldCreateWarningPage()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoPassword(TestStringUtil.GenerateString());

            bool isWarningExist = false;

            // Act
            try
            {
                driver.GmailLoginWithoutPassword(user);

                var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
                isWarningExist = wait.Until(e => e.FindElement(By.XPath("//div[@class=\"o6cuMc\"]")).
                    GetAttribute("innerText").Contains("Не удалось найти аккаунт Google."));
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
            
            // Assert
            isWarningExist.Should().BeTrue();
            Logger.Info("LoginTest_WhenEmailIsWrong_ShouldCreateWarningPage executed successfully.");
        }

        [Test]
        public void LoginTest_WhenPasswordIsWrong_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithCredentials(TestSettings.FirstUserEmail, TestStringUtil.GenerateString());

            IWebElement warning = null;

            // Act
            try
            {
                driver.GmailLoginWithoutRedirection(user);

                var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
                warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@jsname=\"B34EJ\"]/child::span")));
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }

            // Assert
            warning.Text.Should().BeEquivalentTo("Неверный пароль. Повторите попытку или нажмите на ссылку \"Забыли пароль?\", чтобы сбросить его.");
            Logger.Info("LoginTest_WhenPasswordIsWrong_ShouldCreateWarning executed successfully.");
        }
    }
}
