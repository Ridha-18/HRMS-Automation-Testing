using System;
using System.Threading;
using HRMS_UIAutomation.Extensions;
using HRMS_UIAutomation.Helpers;
using HRMS_UIAutomation.WebDriver;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SeleniumExtras.WaitHelpers;

namespace HRMS_UIAutomation.PageObjects
{
    public class EmployeeOnboardPageObject : _SharedPageObjects
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public EmployeeOnboardPageObject(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            driver = Driver.Original;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // Step 1 fields
        internal By FullNameField => By.Name("name");
        internal By EmailField => By.Name("email");
        internal By PhoneField => By.Name("phone");
        internal By PasswordField => By.Name("password");

        // Step 2 fields
        internal By DepartmentDropdown => By.Name("department");
        internal By ReportingManagerIDField => By.Name("reportingManager");
        internal By JoiningDateCalendar => By.Name("joiningDate");
        internal By EmploymentTypeDropdown => By.Name("employmentType");
        internal By IsEmployeeAdminDropdown => By.Name("isAdmin");
        internal By HasReporteeDropdown => By.Name("hasSubordinates");

        // Buttons
        internal By NextButton => By.XPath("//button[contains(text(),'Next')]");
        internal By SubmitButton => By.XPath("//button[contains(text(),'Submit')]");
        internal By OKButton => By.XPath("//button[normalize-space()='OK']");

        // Step 1: Personal Details
        public void FillStep1Details(string fullName, string email, string phone, string password)
        {
            wait.Until(d => d.PageSource.Contains("New Employee Onboarding"));

            FullNameField.EnterText(fullName);
            EmailField.EnterText(email);
            PhoneField.EnterText(phone);
            PasswordField.EnterText(password);

            NextButton.Click();
            WaitFunctions.WaitForDataLoad(2000);
        }

        // Step 2: Job Details & Structure
        public void FillStep2Details(string department, string managerId, string joiningDate, string employmentType, string isAdmin, string hasReportee)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("department")));


            DepartmentDropdown.EnterText(department);
            ReportingManagerIDField.EnterText(managerId);
            JoiningDateCalendar.EnterText(joiningDate);
            EmploymentTypeDropdown.SelectDropDownByText(employmentType);
            IsEmployeeAdminDropdown.SelectDropDownByText(isAdmin);
            HasReporteeDropdown.SelectDropDownByText(hasReportee);

            NextButton.Click();
            WaitFunctions.WaitForDataLoad(2000);
        }

        // Step 3: Review and Submit
        public void ReviewAndSubmit()
        {
            wait.Until(d => d.PageSource.Contains("Review and Confirm"));
            SubmitButton.Click();
            WaitFunctions.WaitForDataLoad(2000);
        }

        // Popup verification
        public void VerifyOnboardPopupMessage(string expectedTextPart)
        {
            try
            {
                // Wait for the JavaScript alert to appear
                var alertWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                alertWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());

                var alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;

                // Verify the alert text
                Assert.IsTrue(alertText.Contains(expectedTextPart),$"Expected alert text to contain '{expectedTextPart}', but got '{alertText}'.");

               
                alert.Accept();

                WaitFunctions.WaitForDataLoad(1000);
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Expected alert did not appear after submitting the employee onboarding form.");
            }
        }

        public void VerifyDeletePopupMessage(string expectedText)
        {
            // Wait for JS alert
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
            var alert = Driver.Original.SwitchTo().Alert();
            Assert.AreEqual("Are you sure you want to delete this employee?", alert.Text);
            alert.Accept();
        }

        public void ClickDeleteIcon(string fullName)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var rowXPath = $"//td[contains(text(), '{fullName}')]/ancestor::tr";
            var deleteBtnXPath = $"{rowXPath}//button[@title='Delete']";

            // Wait for employee row
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(rowXPath)));

            // Wait for all images to finish loading
            wait.Until(d => d.FindElements(By.TagName("img")).All(img =>
            {
                try
                {
                    return (bool)((IJavaScriptExecutor)d).ExecuteScript(
                        "return arguments[0].complete && arguments[0].naturalWidth > 0;", img);
                }
                catch { return false; }
            }));

            // Wait for delete button and scroll into view
            var deleteBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(deleteBtnXPath)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", deleteBtn);

           
            try { deleteBtn.Click(); }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", deleteBtn);
            }
        }


    }
}
