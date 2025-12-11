using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Reqnroll;
using System.Text.RegularExpressions;
using System.Threading;
using HRMS_UIAutomation.Extensions;
using HRMS_UIAutomation.WebDriver;
using HRMS_UIAutomation.Helpers;
using Newtonsoft.Json.Linq;
namespace HRMS_UIAutomation.PageObjects;

public class _SharedPageObjects
{
    public _SharedPageObjects(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }

    private readonly ScenarioContext scenarioContext;

    internal static IJavaScriptExecutor executor;

    internal By PageText(string text) => By.XPath($"//div[text()='{text}']");

    internal By PopupMessage => By.XPath("//div[contains(@class, 'Toastify__toast-container') and contains(@class, 'Toastify__toast-container--top-right')]");

    internal By Button(string text) => By.XPath($"//button[text()='{text}'] | //button[contains(@class, 'MuiFab') and contains(., '{text}')]");

    public void Verifypopup(string message)
    {
        PopupMessage.WaitForElement();
        PopupMessage.VerifyText(message);
    }

    public void ClickButtonOnPage(string button)
    {
        Button(button).WaitForElementToBeClickable();
        Button(button).Click();        
    }

    public void VerifyPage(string text)
    {
        Assert.IsTrue(PageText(text).IsVisible(), "Text is not displayed.");
    }


    public string GetURL()
    {
        return Driver.Original.Url;
    }   

    public void GoToURL(string url)
    {
        Driver.Original.Navigate().GoToUrl(url);
    }

    public static string GetBearerToken()
    {
        executor = (IJavaScriptExecutor)Driver.Original;
        var keyToken = executor.ExecuteScript("return Object.keys(localStorage).find((key) => key.includes(\"accesstoken\"))");
        var newtoken = (string)executor.ExecuteScript("return window.localStorage.getItem('" + keyToken + "')");
        JObject json = JObject.Parse(newtoken);
        newtoken = json.SelectToken("secret").ToString();
        return newtoken;
    }
}

