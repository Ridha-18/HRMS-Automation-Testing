using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

using System.Reflection;

namespace HRMS_UIAutomation.Helpers
{
    public static class WaitFunctions
    {
        internal static IWebElement WaitTillElementIsClickable(IWebDriver driver, By element)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(50)).Until(ExpectedConditions.ElementToBeClickable(element));
        }

        internal static IWebElement WaitTillElementIsVisible(IWebDriver driver, By element)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(50)).Until(ExpectedConditions.ElementIsVisible(element));
        }

        internal static IWebElement WaitTillPresenceOfElement(IWebDriver driver, By element)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(50)).Until(ExpectedConditions.ElementExists(element));
        }

        internal static bool WaitTillElementIsEnable(IWebElement element)
        {
            return element.Enabled;
        }

        internal static void WaitToElementClickable(IWebDriver driver, By element)
        {
            try
            {
                if (WaitTillElementIsClickable(driver, element) == null)
                {
                    WaitToElementClickable(driver, element);
                }
            }
            catch (ElementClickInterceptedException)
            {
                WaitTillPresenceOfElement(driver, element);
            }
            catch (NoSuchElementException)
            {
                WaitTillPresenceOfElement(driver, element);
            }
        }

        internal static void WaitToElementVisible(IWebDriver driver, By element)
        {
            try
            {
                if (WaitTillElementIsVisible(driver, element) == null)
                {
                    WaitToElementVisible(driver, element);
                }
            }
            catch (ElementClickInterceptedException)
            {
                WaitTillPresenceOfElement(driver, element);
            }
            catch (NoSuchElementException)
            {
                WaitTillPresenceOfElement(driver, element);
            }
        }

        internal static void WaitToElementPresent(IWebDriver driver, By element)
        {
            try
            {
                if (WaitTillPresenceOfElement(driver, element) == null)
                {
                    WaitToElementPresent(driver, element);
                }
            }
            catch (ElementClickInterceptedException)
            {
                WaitTillPresenceOfElement(driver, element);
            }
            catch (NoSuchElementException)
            {
                WaitTillPresenceOfElement(driver, element);
            }
        }

        internal static void WaitToElementVisibleAndClickable(IWebDriver driver, By element)
        {
            try
            {
                if ((WaitTillElementIsVisible(driver, element) == null)
                            && (WaitTillElementIsClickable(driver, element) == null))
                {
                    WaitToElementVisibleAndClickable(driver, element);
                }
            }
            catch (ElementClickInterceptedException)
            {
                WaitToElementVisibleAndClickable(driver, element);
            }
            catch (NoSuchElementException)
            {
                WaitToElementVisibleAndClickable(driver, element);
            }
        }

        internal static bool WaitToElementEnable(IWebElement element)
        {
            while (!WaitTillElementIsEnable(element))
            {
                WaitTillElementIsEnable(element);
            }
            return true;
        }

        internal static void FluentWait(IWebDriver driver, IWebElement element)
        {
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(10),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            fluentWait.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        internal static void WaitForLoad(IWebDriver driver)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(25)).Until(
            d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        internal static void WaitForDataLoad(int duration)
        {
            Thread.Sleep(duration);
        }

        internal static void WaitForElementToBeVisible(IWebDriver driver, IWebElement element, int timeout)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));

                wait.Until(drv => element);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool IsElementVisible(IWebElement element)
        {
            return element.Displayed && element.Enabled;
        }

        public static class CommonWaits
        {
            public static void WaitForPageAndDataLoad(IWebDriver driver)
            {
                WaitFunctions.WaitForLoad(driver);
                WaitFunctions.WaitForDataLoad(1000);
            }
        }
    }
}