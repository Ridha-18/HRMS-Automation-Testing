using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

using System.Net.NetworkInformation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HRMS_UIAutomation.Helpers
{
    public static class CommonFunctions
    {
        internal static IJavaScriptExecutor executor;
        internal static Actions actions;
        internal static void ClickOnElement(IWebElement element)
        {
            element.Click();
        }
        internal static void ClickOnElementIfEnabled(IWebElement elementToBeClicked, IWebElement elementToBeEnabled)
        {
            try
            {
                if (WaitFunctions.WaitToElementEnable(elementToBeEnabled))
                {
                    elementToBeClicked.Click();
                }
            }
            catch (ElementClickInterceptedException)
            {
                elementToBeClicked.Click();
            }
            catch (NoSuchElementException)
            {
                elementToBeClicked.Click();
            }
        }

        internal static void ClearText(IWebElement element)
        {
            element.Clear();
        }

        internal static void EnterText(IWebElement element, string text)
        {
            element.SendKeys(text);
        }

        internal static string GetText(IWebElement element)
        {
            return element.Text;
        }

        internal static void DoubleClickElement(IWebDriver driver, IWebElement element)
        {
            actions = new Actions(driver);
            actions.MoveToElement(element).DoubleClick().Build().Perform();
        }

        internal static bool IsDisplayed(IWebElement element)
        {
            return element.Displayed;
        }

        internal static void ClickOnElementUsingJavaScriptExecutor(IWebDriver driver, IWebElement element)
        {
            executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript("arguments[0].click();", element);
        }

        internal static int GetCountOfElements(IList<IWebElement> element)
        {
            return element.Count;
        }

        internal static void ClickRandomElementOnTableView(IWebDriver driver, IWebElement element, IList<IWebElement> elementList)
        {
            int rowsCount = GetCountOfElements(elementList);
            if (rowsCount > 1)
            {
                int i = rowsCount - (rowsCount - 2);
                ClickOnElement(driver.FindElement(By.XPath("(//tr)[" + i + "]")));
            }
            else
            {
                Assert.AreEqual("No results found", element.Text);
            }
        }

        internal static void ClickRandomElementOnGridView(IWebDriver driver, IWebElement element, IList<IWebElement> elementList)
        {
            int rowsCount = GetCountOfElements(elementList);
            if (rowsCount > 0)
            {
                int i = rowsCount - (rowsCount - 1);
                ClickOnElement(driver.FindElement(By.XPath("(//*[@class='btn-view'])[" + i + "]")));
            }
            else
            {
                Assert.AreEqual("No results found", element.Text);
            }
        }

        public static void click(By element, IWebDriver driver)
        {

            WaitFunctions.WaitTillElementIsVisible(driver, element);
            WaitFunctions.WaitTillElementIsClickable(driver, element).Click();            
        }

        public static string randomText()
        {
            Random ran = new Random();

            String b = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int length = 6;

            String random = "";

            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(52);
                random = random + b.ElementAt(a);
            }
            return random;
        }

        public static int RandomInteger()
        {
            Random ran = new Random();
            int result = 0;
            int length = 6; // Define how many digits you want
            for (int i = 0; i < length; i++)
            {
                int digit = ran.Next(0, 10); // Generate a random digit (0-9)
                result = result * 10 + digit; // Append the digit to form an integer
            }
            return result;
        }

    }
}