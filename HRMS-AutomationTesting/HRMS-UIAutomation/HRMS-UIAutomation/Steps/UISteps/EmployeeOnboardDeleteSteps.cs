using System;
using HRMS_UIAutomation.Helpers;
using HRMS_UIAutomation.Models.UI;
using HRMS_UIAutomation.PageObjects;
using HRMS_UIAutomation.TestData;
using HRMS_UIAutomation.WebDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace HRMS_UIAutomation.Steps.UISteps
{
    [Binding]
    public class EmployeeOnboardDeleteSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly EmployeeOnboardPageObject _onboardPage;
        private readonly EmployeeManagementPageObject _managementPage;
        private readonly DashboardPageObject _dashboardPage;
        private readonly WebDriverWait _wait;
        private readonly IWebDriver _driver;
        public EmployeeOnboardDeleteSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _onboardPage = new EmployeeOnboardPageObject(_scenarioContext);
            _managementPage = new EmployeeManagementPageObject();
            _dashboardPage = new DashboardPageObject();
            _wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(10));
            _driver = Driver.Original;
        }

        //Click a left menu item
        [When(@"The user clicks on ""(.*)"" in the left menu")]
        public void WhenTheUserClicksOnInTheLeftMenu(string menuName)
        {
            _wait.Until(d => d.FindElement(By.XPath($"//div//span[contains(text(),'{menuName}')]")));
            _dashboardPage.ClickMenuButton(menuName);
        }

        //Verify page is visible
        [Then(@"The ""(.*)"" page should be visible")]
        public void ThenThePageShouldBeVisible(string pageName)
        {
            string expectedText = pageName switch
            {
                "Onboard Employee" => "New Employee Onboarding",
                "Employee Management" => "Employee Management",
                _ => pageName
            };

            bool isPageVisible = _dashboardPage.IsPageVisible(expectedText);
            Assert.IsTrue(isPageVisible, $"{pageName} page is not visible.");
        }

        //Step 1: Fill Step 1 details
        [Then(@"The user fills in Step 1 details")]
        public void ThenTheUserFillsStep1Details()
        { 
            //_onboardPage.FillStep1Details(name, email, phone, password);
            //_scenarioContext["EmployeeName"] = name;
            var employee = EmployeeDataFactory.GenerateEmployee();
            _scenarioContext["EmployeeData"] = employee;

            _onboardPage.FillStep1Details(employee.FullName, employee.Email, employee.Phone, employee.Password);
        
        }

        // Step 2: Fill Step 2 details
        [Then(@"The user fills in Step 2 details")]
        public void ThenTheUserFillsStep2Details()
        {
            var employee = (EmployeeModel)_scenarioContext["EmployeeData"];

            _onboardPage.FillStep2Details(
                employee.Department,
                employee.ReportingManagerID,
                employee.JoiningDate,
                employee.EmploymentType,
                employee.IsEmployeeAdmin,
                employee.DoesEmployeeHaveReportee
            );
        }

        //Step 3: Review details and submit
        [Then(@"The user reviews the details on Step 3")]
        public void ThenTheUserReviewsTheDetailsAndSubmits()
        {
            _onboardPage.ReviewAndSubmit();
        }

        //Popup verification
        [Then(@"A popup message containing ""(.*)"" should appear")]
        public void ThenPopupMessageShouldAppear(string expectedMessage)
        {
            _onboardPage.VerifyOnboardPopupMessage(expectedMessage);
        }

    

        //Verify employee is visible in Employee Management

        [Then(@"The generated employee should be visible on the dashboard")]
        public void ThenEmployeeShouldBeVisible()
        {

            var employee = (EmployeeModel)_scenarioContext["EmployeeData"];

            bool isVisible = _managementPage.IsEmployeeVisible(employee.FullName);
            Assert.IsTrue(isVisible, $"Employee {employee.FullName} is not visible on the dashboard.");
        }

        //Delete employee
        [When(@"The user clicks delete icon for that employee")]
        public void WhenUserDeletesEmployee()
        {
            var employee = (EmployeeModel)_scenarioContext["EmployeeData"];
            _managementPage.ClickDeleteIcon(employee.FullName);
        }

        //Popup verification
        [Then(@"A popup message having ""(.*)"" should appear")]
        public void ThenDeletePopupMessageAppears(string expectedtext)
        {
            _onboardPage.VerifyDeletePopupMessage(expectedtext);
        }

        // Verify employee deletion
        [Then(@"The employee should no longer be visible on the dashboard")]
        public void ThenEmployeeShouldBeDeleted()
        {
            var employee = (EmployeeModel)_scenarioContext["EmployeeData"];
            bool isDeleted = _managementPage.IsEmployeeDeleted(employee.FullName);
            Assert.IsTrue(isDeleted, $"Employee {employee.FullName} was not deleted successfully.");
        }


    }
}
