using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestApp.Settings;

namespace TestApp
{
    [TestFixture]
    public class ChangeNicknameTest
    {
        public string driverPath;

        private ChromeDriver driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driverPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.
                GetCurrentDirectory()).ToString()).ToString()).ToString();
            driverPath = Path.Combine(driverPath, "Settings//Driver");

            driver = new ChromeDriver(driverPath);
            driver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;
            driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.FindElement(By.XPath("//a[@data-rid=\"10003\" and @class=\"VZLjze Wvetm zCVEd EhlvJf\"]")).Click();
            driver.FindElement(By.XPath("//a[@href=\"name\"]")).Click();
            var nameField = driver.FindElements(By.XPath("//input[@class=\"VfPpkd-fmcmS-wGMbrd CtvUB\"]"))[0];
            nameField.Clear();
            nameField.SendKeys(TestSettings.OldNickName);
            nameField.SendKeys(Keys.Enter);
            driver.Close();
        }

        [Test]
        public void ChangeNickname_WhenAllDataAreValid_ShouldSetNewNickname()
        {
            // Arrange
            var expectedWelcome = "Добро пожаловать, " + TestSettings.NewPseudonym + " TestSurname!";

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

            // Act
            driver.Navigate().GoToUrl(TestSettings.GmailUrl);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tbody/child::tr[1]")));
            driver.FindElements(By.XPath("//tbody/child::tr[1]"))[5].Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]")));
            var newNickName = driver.FindElement(By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]")).GetAttribute("innerText");

            driver.Navigate().GoToUrl(TestSettings.AccountUrl);
            driver.FindElement(By.XPath("//a[@data-rid=\"10003\" and @class=\"VZLjze Wvetm zCVEd EhlvJf\"]")).Click();
            driver.FindElement(By.XPath("//a[@href=\"name\"]")).Click();
            var nameField = driver.FindElements(By.XPath("//input[@class=\"VfPpkd-fmcmS-wGMbrd CtvUB\"]"))[0];
            nameField.Clear();
            nameField.SendKeys(newNickName);
            nameField.SendKeys(Keys.Enter);

            Thread.Sleep(1000);
            driver.Navigate().GoToUrl(TestSettings.AccountUrl);
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//h1[@class=\"x7WrMb\"]")));

            // Assert
            var welcome = driver.FindElement(By.XPath("//h1[@class=\"x7WrMb\"]")).GetAttribute("innerHTML");
            welcome.Should().BeEquivalentTo(expectedWelcome);
        }
    }
}
