using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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

            // Act
            Driver.GmailLogin(user);

            // Assert
            Driver.Url.Should().BeEquivalentTo(SettingsService.ExpectedAccountUrl);
        }

        [Test]
        public void LoginTest_WhenNoEmailSent_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoLoginAndPassword();

            IWebElement warning = null;

            // Act
            Driver.GmailLoginWithoutPassword(user);

            var wait = new WebDriverWait(Driver, SettingsService.ImplicitWaitSpan);
            warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class=\"o6cuMc\"]")));

            // Assert
            warning.Text.Should().BeEquivalentTo("Введите адрес электронной почты или номер телефона.");
        }

        [Test]
        public void LoginTest_WhenNoPasswordSent_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoPassword(SettingsService.FirstUserEmail);

            IWebElement warning = null;

            // Act
            Driver.GmailLoginWithoutRedirection(user);

            var wait = new WebDriverWait(Driver, SettingsService.ImplicitWaitSpan);
            warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@jsname=\"B34EJ\"]/child::span")));

            // Assert
            warning.Text.Should().BeEquivalentTo("Введите пароль");
        }

        [Test]
        public void LoginTest_WhenEmailIsWrong_ShouldCreateWarningPage()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithNoPassword(TestStringUtil.GenerateString());

            bool isWarningExist = false;

            // Act
            Driver.GmailLoginWithoutPassword(user);

            var wait = new WebDriverWait(Driver, SettingsService.ImplicitWaitSpan);
            isWarningExist = wait.Until(e => e.FindElement(By.XPath("//div[@class=\"o6cuMc\"]")).
                GetAttribute("innerText").Contains("Не удалось найти аккаунт Google."));

            // Assert
            isWarningExist.Should().BeTrue();
        }

        [Test]
        public void LoginTest_WhenPasswordIsWrong_ShouldCreateWarning()
        {
            // Arrange
            var user = CreateUserService.CreateUserWithCredentials(SettingsService.FirstUserEmail, TestStringUtil.GenerateString());

            IWebElement warning = null;

            // Act
            Driver.GmailLoginWithoutRedirection(user);

            var wait = new WebDriverWait(Driver, SettingsService.ImplicitWaitSpan);
            warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@jsname=\"B34EJ\"]/child::span")));

            // Assert
            warning.Text.Should().BeEquivalentTo("Неверный пароль. Повторите попытку или нажмите на ссылку \"Забыли пароль?\", чтобы сбросить его.");
        }
    }
}
