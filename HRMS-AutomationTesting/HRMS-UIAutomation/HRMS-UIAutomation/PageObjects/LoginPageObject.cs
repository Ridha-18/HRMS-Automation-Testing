using HRMS_UIAutomation.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace HRMS_UIAutomation.PageObjects
{
    public class LoginPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public LoginPageObject()
        {
            driver = Driver.Original;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // Locators
        private By EmailInput => By.XPath("//input[@type='email']");
        private By PasswordInput => By.XPath("//input[@type='password']");
        private By LoginButton => By.XPath("//button[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'log in')]");

        // Page Actions
        public void Login(string email, string password)
        {
            var emailField = wait.Until(ExpectedConditions.ElementIsVisible(EmailInput));
            emailField.Clear();
            emailField.SendKeys(email);

            var passwordField = wait.Until(ExpectedConditions.ElementIsVisible(PasswordInput));
            passwordField.Clear();
            passwordField.SendKeys(password);

            var loginButton = wait.Until(ExpectedConditions.ElementToBeClickable(LoginButton));
            loginButton.Click();

            // Wait for redirect or dashboard confirmation
            wait.Until(d => d.Url.Contains("/home"));
        }

        public bool IsDashboardVisible()
        {
            try
            {
                wait.Until(d => d.Url.Contains("/home"));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
