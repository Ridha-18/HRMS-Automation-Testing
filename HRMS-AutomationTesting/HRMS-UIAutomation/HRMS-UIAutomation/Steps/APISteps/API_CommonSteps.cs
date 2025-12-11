using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Linq;
using System.Threading.Tasks;
using HRMS_UIAutomation.Helpers;
using OpenQA.Selenium;
using Reqnroll;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace HRMS_UIAutomation.Steps.APISteps
{
    [Binding]
    public class API_CommonSteps
    {
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
       
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        public API_CommonSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {            
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;           
        }

        [When(@"""([^""]*)"" token is set")]
        public void WhenTokenIsSet(string auth)
        {
            string headers = APIHelpers.getToken(auth);
            _featureContext["auth"] = headers;
        }

        [Then(@"'([^']*)' and '([^']*)' are expected")]
        public void ThenAndAreExpected(int code, string status)
        {
            _scenarioContext.TryGetValue("resp", out RestResponse resObj);
            Assert.AreEqual((int)resObj.StatusCode, code, "The response code status was different from expected.");
            Assert.AreEqual(resObj.StatusCode.ToString(), status, "The response status message was different from expected.");

        }
    }
}
