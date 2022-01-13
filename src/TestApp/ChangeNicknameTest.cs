using FluentAssertions;
using NUnit.Framework;
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
    [TestFixture]
    public class ChangeNicknameTest
    {
        public string driverPath;

        public IWebDriver driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driverPath = TestStringUtils.GetPathToDriver();

            driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;
            driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.ChangeGmailAccountNickName(TestSettings.OldNickName);
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

            // Act
            var newNickName = driver.ReadGmailMessage();

            driver.ChangeGmailAccountNickName(newNickName);

            Thread.Sleep(1000);
            driver.Navigate().GoToUrl(TestSettings.AccountUrl);
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//h1[@class=\"x7WrMb\"]")));

            var welcome = driver.FindElement(By.XPath("//h1[@class=\"x7WrMb\"]")).GetAttribute("innerHTML");

            // Assert
            welcome.Should().BeEquivalentTo(expectedWelcome);
        }
    }
}
