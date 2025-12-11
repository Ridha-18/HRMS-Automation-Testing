using HRMS_UIAutomation.PageObjects;
using OpenQA.Selenium;
using Reqnroll;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRMS_UIAutomation.PageObjects;
using HRMS_UIAutomation.Helpers;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;

namespace HRMS_UIAutomation.Steps.UISteps
{
    [Binding, Scope(Tag = "Homepage")]
    public class HRMSCommonSteps
    {
        private _SharedPageObjects GetSharedPageObjects()
        => new _SharedPageObjects(_scenarioContext);

        private static FeatureContext _featureContext { get; set; }
        private static ScenarioContext _scenarioContext { get; set; }
        public HRMSCommonSteps( FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [Then("user can see the popup message {string}")]
        public void ThenUserCanSeeThePopupMessage(string popupmessage)
        {
            GetSharedPageObjects().Verifypopup(popupmessage);
        }

        [When("user clicks the {string} button on page")]
        public void WhenUserClicksTheButtonOnPage(string button)
        {
            GetSharedPageObjects().ClickButtonOnPage(button);
        }      


        [Then("user can see the {string} page")]
        public void ThenUserCanSeeThePage(string PageText)
        {
            GetSharedPageObjects().VerifyPage(PageText);
        }


    }

}
