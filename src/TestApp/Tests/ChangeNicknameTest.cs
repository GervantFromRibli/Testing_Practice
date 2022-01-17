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
using TestApp.Tests;
using TestApp.Utils;

namespace TestApp
{
    [TestFixture, Category("All")]
    public class ChangeNicknameTest : TestBase
    {
        [Test]
        public void ChangeNickname_WhenAllDataAreValid_ShouldSetNewNickname()
        {
            // Arrange
            var expectedWelcome = "Добро пожаловать, " + SettingsService.NewPseudonym + " TestSurname!";

            var user = CreateUserService.CreateUserWithCredentials(SettingsService.FirstUserEmail, SettingsService.FirstUserPassword);

            Driver.GmailLogin(user);

            var wait = new WebDriverWait(Driver, SettingsService.ImplicitWaitSpan);

            string welcome = string.Empty;

            // Act
            var newNickName = Driver.ReadGmailMessage();

            Driver.ChangeGmailAccountNickName(newNickName);

            Thread.Sleep(1000);
            Driver.Navigate().GoToUrl(SettingsService.AccountUrl);
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//h1[@class=\"x7WrMb\"]")));

            welcome = Driver.FindElement(By.XPath("//h1[@class=\"x7WrMb\"]")).GetAttribute("innerHTML");

            // Assert
            welcome.Should().BeEquivalentTo(expectedWelcome);

            // Act 2
            expectedWelcome = "Добро пожаловать, " + SettingsService.OldNickName + " TestSurname!";
            Driver.ChangeGmailAccountNickName(SettingsService.OldNickName);

            Thread.Sleep(1000);
            Driver.Navigate().GoToUrl(SettingsService.AccountUrl);
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//h1[@class=\"x7WrMb\"]")));

            welcome = Driver.FindElement(By.XPath("//h1[@class=\"x7WrMb\"]")).GetAttribute("innerHTML");

            // Assert
            welcome.Should().BeEquivalentTo(expectedWelcome);
        }
    }
}
