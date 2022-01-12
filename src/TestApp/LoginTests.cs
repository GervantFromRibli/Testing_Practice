using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using TestApp.Settings;

namespace TestApp
{
    [TestFixture]
    public class LoginTests
    {
        public string driverPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driverPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.
                GetCurrentDirectory()).ToString()).ToString()).ToString();
            driverPath = Path.Combine(driverPath, "Settings//Driver");
        }

        [Test]
        public void LoginTest_WhenAllDataAreCorrect_ShouldRedirectToPage()
        {
            // Arrange
            var driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

            // Act
            driver.Navigate().GoToUrl(TestSettings.LoginPage);
            var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
            email.SendKeys(TestSettings.FirstUserEmail);

            var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
            buttonSubmit.Click();

            var password = driver.FindElement(By.Name("password"));
            password.SendKeys(TestSettings.FirstUserPassword);
            password.SendKeys(Keys.Enter);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("myaccount"));

            // Assert
            driver.Url.Should().BeEquivalentTo(TestSettings.ExpectedAccountUrl);
            driver.Close();
        }

        [Test]
        public void LoginTest_WhenNoEmailSent_ShouldCreateWarning()
        {
            // Arrange
            var driver = new ChromeDriver(driverPath);

            // Act
            driver.Navigate().GoToUrl(TestSettings.LoginPage);
            var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
            email.SendKeys(string.Empty);

            var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
            buttonSubmit.Click();

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
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

            // Act
            driver.Navigate().GoToUrl(TestSettings.LoginPage);
            var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
            email.SendKeys(TestSettings.FirstUserEmail);

            var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
            buttonSubmit.Click();

            var password = driver.FindElement(By.Name("password"));
            password.SendKeys(string.Empty);
            password.SendKeys(Keys.Enter);

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

            // Act
            driver.Navigate().GoToUrl(TestSettings.LoginPage);
            var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
            email.SendKeys(new string('w', 10));

            var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
            buttonSubmit.Click();

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            var isWarningExist = wait.Until(e => e.FindElement(By.XPath("//h1[@class=\"ahT6S \"]/child::span")).
                GetAttribute("innerText").Contains("Не удалось войти в аккаунт"));

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

            // Act
            driver.Navigate().GoToUrl(TestSettings.LoginPage);
            var email = driver.FindElement(By.XPath("//input[@type=\"email\"]"));
            email.SendKeys(TestSettings.FirstUserEmail);

            var buttonSubmit = driver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
            buttonSubmit.Click();

            var password = driver.FindElement(By.Name("password"));
            password.SendKeys("dwawdaw");
            password.SendKeys(Keys.Enter);

            var wait = new WebDriverWait(driver, TestSettings.ImplicitWaitSpan);
            var warning = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@jsname=\"B34EJ\"]/child::span")));

            // Assert
            warning.Text.Should().BeEquivalentTo("Неверный пароль. Повторите попытку или нажмите на ссылку \"Забыли пароль?\", чтобы сбросить его.");
            driver.Close();
        }
    }
}
