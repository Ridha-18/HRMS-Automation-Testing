using HRMS_UIAutomation.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace HRMS_UIAutomation.PageObjects
{
    public class DashboardPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public DashboardPageObject()
        {
            driver = Driver.Original;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); 
        }

        
        public void ClickMenuButton(string menuName)
        {
            var menuItem = driver.FindElement(By.XPath($"//div//span[contains(text(),'{menuName}')]"));
            menuItem.Click();
        }

        
        public bool IsPageVisible(string expectedText)
        {
            try
            {
                return wait.Until(d => d.PageSource.Contains(expectedText));
            }
            catch
            {
                return false;
            }
        }
    }
}
