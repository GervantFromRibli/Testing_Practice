using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Threading;
using TestApp.Settings;

namespace TestApp
{
    [TestFixture]
    public class SendMessageTests
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
        public void SendMessage_WhenAllDataAreValid_ShouldSendAndReceiveMessages()
        {
            // Arrange
            var firstUserDriver = new ChromeDriver(driverPath);
            var secondUserDriver = new ChromeDriver(driverPath);
            firstUserDriver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;
            secondUserDriver.Manage().Timeouts().ImplicitWait = TestSettings.ImplicitWaitSpan;

            firstUserDriver.Navigate().GoToUrl(TestSettings.LoginPage);
            var email = firstUserDriver.FindElement(By.XPath("//input[@type=\"email\"]"));
            email.SendKeys(TestSettings.FirstUserEmail);

            var buttonSubmit = firstUserDriver.FindElement(By.ClassName("VfPpkd-LgbsSe"));
            buttonSubmit.Click();

            var password = firstUserDriver.FindElement(By.Name("password"));
            password.SendKeys(TestSettings.FirstUserPassword);
            password.SendKeys(Keys.Enter);

            var wait = new WebDriverWait(firstUserDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("myaccount"));

            firstUserDriver.Navigate().GoToUrl(TestSettings.GmailUrl);

            secondUserDriver.Navigate().GoToUrl(TestSettings.MailLoginUrl);

            secondUserDriver.FindElement(By.XPath("//input[@name=\"login\"]")).SendKeys(TestSettings.SecondUserEmail);
            secondUserDriver.FindElement(By.XPath("//button[@data-testid=\"enter-password\"]")).Click();
            secondUserDriver.FindElement(By.XPath("//input[@name=\"password\"]")).SendKeys(TestSettings.SecondUserPassword);
            secondUserDriver.FindElement(By.XPath("//button[@data-testid=\"login-to-mail\"]")).Click();

            secondUserDriver.Navigate().GoToUrl(TestSettings.MailUrl);

            // Act 1
            firstUserDriver.FindElement(By.XPath("//div[@class=\"T-I T-I-KE L3\"]")).Click();

            firstUserDriver.FindElement(By.XPath("//textarea[@name=\"to\"]")).SendKeys(TestSettings.SecondUserEmail);
            firstUserDriver.FindElement(By.XPath("//div[@class=\"Am Al editable LW-avf tS-tW\"]")).SendKeys(TestSettings.MessageToSend);
            firstUserDriver.FindElement(By.XPath("//div[@class=\"T-I J-J5-Ji aoO v7 T-I-atl L3\"]")).Click();

            wait = new WebDriverWait(secondUserDriver, TestSettings.ImplicitWaitSpan);
            wait.Until(ExpectedConditions.UrlContains(TestSettings.MailUrl));
            Thread.Sleep(15000);
            var message = secondUserDriver.FindElements(By.XPath("//span[@class=\"llc__snippet\"]"))[0].GetAttribute("innerText");

            // Assert 1
            message.Should().BeEquivalentTo(TestSettings.MessageToSend);

            // Act 2
            secondUserDriver.FindElement(By.XPath("//a[@data-title-shortcut=\"N\"]")).Click();
            secondUserDriver.FindElements(By.XPath("//input[@class=\"container--H9L5q size_s--3_M-_\"]"))[0].SendKeys(TestSettings.FirstUserEmail);
            secondUserDriver.FindElements(By.XPath("//div[@role=\"textbox\"]/child::div"))[0].SendKeys(TestSettings.NewPseudonym);
            secondUserDriver.FindElement(By.XPath("//span[@title=\"Отправить\"]")).Click();

            secondUserDriver.Close();
            wait = new WebDriverWait(firstUserDriver, TestSettings.ImplicitWaitSpan);

            Thread.Sleep(70000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tbody/child::tr[1]")));
            firstUserDriver.FindElements(By.XPath("//tbody/child::tr[1]"))[5].Click();

            // Assert 2
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]")));
            message = firstUserDriver.FindElement(By.XPath("//div[@class=\"a3s aiL \"]/child::div[2]/div[1]")).GetAttribute("innerText");
            message.Should().BeEquivalentTo(TestSettings.NewPseudonym);
            firstUserDriver.Close();
        }
    }
}
