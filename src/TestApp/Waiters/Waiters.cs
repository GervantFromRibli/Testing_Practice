using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using TestApp.Services;

namespace TestApp.Waiters
{
    public static class Waiters
    {
        public static bool WaitForUrlToBe(this IWebDriver driver, string url)
        {
            return new WebDriverWait(driver, SettingsService.ImplicitWaitSpan).
                Until(ExpectedConditions.UrlContains(url));
        }

        public static IWebElement WaitForElementToBeClickable(this IWebDriver driver, By locator)
        {
            return new WebDriverWait(driver, SettingsService.ImplicitWaitSpan).
                Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public static IWebElement WaitForElementToExist(this IWebDriver driver, By locator)
        {
            return new WebDriverWait(driver, SettingsService.ImplicitWaitSpan).
                Until(ExpectedConditions.ElementExists(locator));
        }

        public static IWebElement WaitForElementToBeVisible(this IWebDriver driver, By locator)
        {
            return new WebDriverWait(driver, SettingsService.ImplicitWaitSpan).
                Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public static IWebElement WaitForAllElementsToExist(this IWebDriver driver, By locator, int count)
        {
            var isWaitSuccessful = new WebDriverWait(driver, SettingsService.ImplicitWaitSpan).
                Until(e => e.FindElements(locator).Count >= count);
            if (isWaitSuccessful)
            {
                return driver.FindElements(locator)[count - 1];
            }
            else
            {
                throw new WebDriverTimeoutException("Not all elements are loaded.");
            }
        }

        public static bool WaitForOneOfThreeTextesToAppear(this IWebDriver driver, By locator, string firstText,
            string secondText, string thirdText, int timeout)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).
                Until(e => e.FindElement(locator).Text.Contains(firstText) ||
                e.FindElement(locator).Text.Contains(secondText) || e.FindElement(locator).Text.Contains(thirdText));
        }
    }
}
