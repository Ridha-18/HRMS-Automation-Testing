using System.Collections.Generic;
using System.Linq;
using HRMS_UIAutomation.Extensions;
using HRMS_UIAutomation.WebDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace HRMS_UIAutomation.PageObjects
{
    public class EmployeeManagementPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        public EmployeeManagementPageObject()
        {
            driver = Driver.Original;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

       
        private By EmployeeCard(string fullName) => By.XPath($"//h3[normalize-space()='{fullName}']");
        private By DeleteIcon(string fullName) => By.XPath($"//h3[normalize-space()='{fullName}']/ancestor::div[contains(@class,'rounded-2xl')]//button[@title='Delete']");

        // --- Actions ---
        public bool IsEmployeeVisible(string fullName)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(EmployeeCard(fullName)));
                return true; 
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public void ClickDeleteIcon(string fullName)
        {
            By deleteLocator = DeleteIcon(fullName);

            IWebElement deleteButton = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(deleteLocator)
            );

            try
            {
                deleteButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", deleteButton);
            }
        }

        public bool IsEmployeeDeleted(string fullName)
        {
          
            var remaining = driver.FindElements(EmployeeCard(fullName));
            return remaining.Count == 0;
        }
    }
}
