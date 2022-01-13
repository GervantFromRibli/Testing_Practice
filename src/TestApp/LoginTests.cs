using FluentAssertions;
using NUnit.Framework;
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
    [TestFixture]
    public class LoginTests
    {
        private string driverPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driverPath = TestStringUtils.GetPathToDriver();
        }

        [Test]
        public void LoginTest_WhenAllDataAreCorrect_ShouldRedirectToPage()
        {
            // Arrange
            var driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

            var user = CreateUserService.CreateUserWithCredentials(TestSettings.FirstUserEmail, TestSettings.FirstUserPassword);

            // Act
            driver.GmailLogin(user);

            // Assert
            driver.Url.Should().BeEquivalentTo(TestSettings.ExpectedAccountUrl);
            driver.Close();
        }

        [Test]
        public void LoginTest_WhenNoEmailSent_ShouldCreateWarning()
        {
            // Arrange
            var driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

            var user = CreateUserService.CreateUserWithNoLoginAndPassword();

            // Act
            driver.GmailLogin(user);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            var warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class=\"o6cuMc\"]")));

            // Assert
            warning.Text.Should().BeEquivalentTo("Введите адрес электронной почты или номер телефона.");
            driver.Close();
        }

        [Test]
        public void LoginTest_WhenNoPasswordSent_ShouldCreateWarning()
        {
            // Arrange
            var driver = new ChromeDriver(driverPath);

            var user = CreateUserService.CreateUserWithNoPassword(TestSettings.FirstUserEmail);

            // Act
            driver.GmailLogin(user);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            var warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@jsname=\"B34EJ\"]/child::span")));

            // Assert
            warning.Text.Should().BeEquivalentTo("Введите пароль");
            driver.Close();
        }

        [Test]
        public void LoginTest_WhenEmailIsWrong_ShouldCreateWarningPage()
        {
            // Arrange
            var driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

            var user = CreateUserService.CreateUserWithNoPassword(TestStringUtils.GenerateString());

            // Act
            driver.GmailLogin(user);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            var isWarningExist = wait.Until(e => e.FindElement(By.XPath("//div[@class=\"o6cuMc\"]")).
                GetAttribute("innerText").Contains("Не удалось найти аккаунт Google."));

            // Assert
            isWarningExist.Should().BeTrue();
            driver.Close();
        }

        [Test]
        public void LoginTest_WhenPasswordIsWrong_ShouldCreateWarning()
        {
            // Arrange
            var driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

            var user = CreateUserService.CreateUserWithCredentials(TestSettings.FirstUserEmail, TestStringUtils.GenerateString());

            // Act
            driver.GmailLogin(user);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            var warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@jsname=\"B34EJ\"]/child::span")));

            // Assert
            warning.Text.Should().BeEquivalentTo("Неверный пароль. Повторите попытку или нажмите на ссылку \"Забыли пароль?\", чтобы сбросить его.");
            driver.Close();
        }
    }
}
