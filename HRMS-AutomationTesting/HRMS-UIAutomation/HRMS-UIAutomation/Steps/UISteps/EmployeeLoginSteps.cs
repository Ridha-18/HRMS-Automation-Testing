using HRMS_UIAutomation.Helpers;
using HRMS_UIAutomation.PageObjects;
using HRMS_UIAutomation.WebDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace HRMS_UIAutomation.Steps.UISteps
{
    [Binding]
    public class EmployeeLoginSteps
    {
        private readonly LoginPageObject _loginPage;

        public EmployeeLoginSteps(LoginPageObject loginPage)
        {
            _loginPage = loginPage;
        }

        [Given(@"The user has navigated to the login page")]
        public void GivenTheUserHasNavigatedToTheLoginPage()
        {
            Driver.Original.Navigate().GoToUrl(BaseConfig.BaseUrl);
        }

        [When(@"The user enters valid email and password")]
        public void WhenTheUserEntersValidEmailAndPassword()
        {
            _loginPage.Login(BaseConfig.email, BaseConfig.password);
        }

        [Then(@"The dashboard should be visible")]
        public void ThenTheDashboardShouldBeVisible()
        {
            bool isDashboardVisible = _loginPage.IsDashboardVisible();
            Assert.IsTrue(isDashboardVisible, "Dashboard is not visible after login.");
        }
    }
}
