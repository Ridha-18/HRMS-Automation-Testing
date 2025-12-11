using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using HRMS_UIAutomation.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using OpenQA.Selenium.Interactions;
using HRMS_UIAutomation.Helpers;

namespace HRMS_UIAutomation.WebDriver;

public class WebDriverFactory
{       
    static IWebDriver _driver = null;
    
    internal static IWebDriver GetWebDriver(Browser? browser)
    {
        
        switch (browser)
        {
            case Browser.Chrome:

                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("ignore-certificate-errors");               
                chromeOptions.AddArgument("--incognito");
                chromeOptions.AddArgument("disable-extensions");
                chromeOptions.AddArgument("allow-running-insecure-content");
                chromeOptions.AddArguments("no-sandbox");
                chromeOptions.AddArguments("--disable-infobars");
                chromeOptions.AddArguments("--window-size=1720,980");
                chromeOptions.AddArguments("--disk-cache-size=0");
                chromeOptions.AddArguments("start-maximized");
                chromeOptions.AddArgument("disable-features=DownloadBubble,DownloadBubbleV2");
                if (BaseConfig.IsHeadlessMode)
                {
                    // * Please note:  chrome://downloads/ page can't be accessed in headless mode (shows a blank screen). 
                    chromeOptions.AddArguments("--headless");
                    chromeOptions.AddArguments("--disable-gpu");
                    chromeOptions.AddArguments("--window-size=1280,800");
                    chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
                    chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                    chromeOptions.AddUserProfilePreference("download.directory_upgrade", true);
                    chromeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                }
                var chrome = new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeOptions, TimeSpan.FromMinutes(3));
                var logs = chrome.Manage().Logs;
                _driver = chrome;
               
                break;           

            case Browser.Edge:
                var edgeOptions = new EdgeOptions();
                edgeOptions.AddArgument("--disable-infobars");
                edgeOptions.AddArgument("--incognito");
                edgeOptions.AddArguments("--window-size=1920,1080");
                edgeOptions.AddArgument("no-sandbox");
                if (BaseConfig.IsHeadlessMode)
                {
                    // * Please note:  chrome://downloads/ page can't be accessed in headless mode (shows a blank screen). 
                    edgeOptions.AddArguments("--headless");
                    edgeOptions.AddArguments("--disable-gpu");
                    edgeOptions.AddArguments("--window-size=1280,800");
                    edgeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
                    edgeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                    edgeOptions.AddUserProfilePreference("download.directory_upgrade", true);
                    edgeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                }
                _driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), edgeOptions, TimeSpan.FromSeconds(30));

                break;
            case Browser.Firefox:

                break;
            case (Browser.PhantomJS):
                break;
            case (Browser.ChromeHeadless):
                var chromeHeadLessOptions = new ChromeOptions();
                chromeHeadLessOptions.AddArguments("headless");
                _driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeHeadLessOptions, TimeSpan.FromSeconds(30));
                break;
            default:
                _driver = new FirefoxDriver();
                break;
        }
        return _driver;
    }
    public void Dispose()
    {
        Reset();
    }
    public void Reset()
    {
        if (_driver == null) return;
        _driver.Quit();
        _driver = null;
    }
}
