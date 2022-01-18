using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TestApp.Pages;
using TestApp.Services;
using TestApp.Tests;
using TestApp.Utils;

namespace TestApp
{
    [TestFixture, Category("All"), Category("SmokeTests")]
    public class LoginTests : TestBase
    {
        [Test]
        public void LoginTest_WhenAllDataAreCorrect_ShouldRedirectToPage()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithCredentials(SettingsService.FirstUserEmail, SettingsService.FirstUserPassword);
            var loginPage = new GmailLoginPage(Driver);

            // Act
            var url = loginPage.
                OpenPage().
                Login(user).
                GetCurrentUrl();

            // Assert
            url.Should().BeEquivalentTo(SettingsService.ExpectedAccountUrl);
        }

        [Test]
        public void LoginTest_WhenNoEmailSent_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoLoginAndPassword();
            var loginPage = new GmailLoginPage(Driver);

            // Act
            var warning = loginPage.
                OpenPage().
                LoginWithoutPassword(user).
                GetEmailWarning();

            // Assert
            warning.Text.Should().BeEquivalentTo("Введите адрес электронной почты или номер телефона.");
        }

        [Test]
        public void LoginTest_WhenNoPasswordSent_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoPassword(SettingsService.FirstUserEmail);
            var loginPage = new GmailLoginPage(Driver);

            // Act
            var warning = loginPage.
                OpenPage().
                LoginWithoutRedirection(user).
                GetPasswordWarning();

            // Assert
            warning.Text.Should().BeEquivalentTo("Введите пароль");
        }

        [Test]
        public void LoginTest_WhenEmailIsWrong_ShouldCreateWarningPage()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoPassword(TestStringUtil.GenerateString());
            var loginPage = new GmailLoginPage(Driver);

            // Act
            var warning = loginPage.
                OpenPage().
                LoginWithoutPassword(user).
                GetEmailWarning();

            // Assert
            warning.Text.Should().BeEquivalentTo("Не удалось найти аккаунт Google.");
        }

        [Test]
        public void LoginTest_WhenPasswordIsWrong_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithCredentials(SettingsService.FirstUserEmail, TestStringUtil.GenerateString());
            var loginPage = new GmailLoginPage(Driver);

            // Act
            var warning = loginPage.
                OpenPage().
                LoginWithoutRedirection(user).
                GetPasswordWarning();

            // Assert
            warning.Text.Should().BeEquivalentTo("Неверный пароль. Повторите попытку или нажмите на ссылку \"Забыли пароль?\", чтобы сбросить его.");
        }
    }
}
