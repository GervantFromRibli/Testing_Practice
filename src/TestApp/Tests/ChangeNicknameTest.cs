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
using TestApp.Pages;
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
            var accountPage = new GmailLoginPage(Driver).
                OpenPage().
                Login(user);

            // Act
            var newNickName = new GmailPage(Driver).OpenPage().ReadNewMessage();

            accountPage.
                OpenPage().
                ChangeNickname(newNickName);

            var welcome = accountPage.OpenPage().ReadWelcomeMessage(expectedWelcome);

            // Assert
            welcome.Should().BeEquivalentTo(expectedWelcome);

            // Act 2
            expectedWelcome = "Добро пожаловать, " + SettingsService.OldNickName + " TestSurname!";

            accountPage.
                OpenPage().
                ChangeNickname(SettingsService.OldNickName);

            welcome = accountPage.OpenPage().ReadWelcomeMessage(expectedWelcome);

            // Assert
            welcome.Should().BeEquivalentTo(expectedWelcome);
        }
    }
}
