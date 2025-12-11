using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using HRMS_UIAutomation.Enums;
using System;
using System.Diagnostics;
using System.Threading;
using Reqnroll;
using HRMS_UIAutomation.Helpers;

namespace HRMS_UIAutomation.WebDriver;

public sealed class Driver : IDisposable
{
    [ThreadStatic]
    private static IWebDriver _driver;
    private static readonly object driverLock = new();

    public static IWebDriver Original => _driver ?? throw new NullReferenceException("Driver is null.");

    public static void Init(ScenarioContext scenarioContext)
    {
        _driver = WebDriverFactory.GetWebDriver(BaseConfig.WebDriverType);
        scenarioContext.ScenarioContainer.RegisterInstanceAs(instance: _driver, dispose: true);
    }

    public static void Init(ScenarioContext scenarioContext, TestContext testContext)
    {
        //_driver = WebDriverFactory.GetWebDriver(Settings.WebDriverType);
        //scenarioContext.ScenarioContainer.RegisterInstanceAs(instance: _driver, dispose: true);
        scenarioContext.Add("ClassName",testContext.FullyQualifiedTestClassName);
        scenarioContext.Add("TestName", testContext.TestName.Replace(" ", string.Empty));
    }

    public static IWebDriver Instance
    {
        get
        {
            if (_driver == null)
            {
                lock (driverLock)
                {
                    _driver = WebDriverFactory.GetWebDriver(BaseConfig.WebDriverType);
                    //_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(Settings.DefaultImplicitlyWaitTimeInSeconds));
                    if (BaseConfig.WebDriverType == Browser.Chrome)
                    {
                        // _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(Settings.DefaultImplicitlyWaitTimeInSeconds);
                        //  _driver.Manage().Window.Maximize();
                    }                    
                    else if (BaseConfig.WebDriverType == Browser.Firefox)
                    {
                        _driver.Manage().Window.Maximize();

                    }
                    Console.WriteLine($"Driver intialized, Current browser is :{BaseConfig.WebDriverType}");
                }
            }
            return _driver;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (_driver == null) return;
        _driver.Close();
        _driver.Quit();
        _driver.Dispose();
        _driver = null;
        
    }

    public static void DisposeDriver()
    {
        _driver.Dispose();
    }
    public static void DisposeDriver(IWebDriver driver)
    {
        if (driver == null) return;
        driver.Close();
        driver.Quit();
        driver.Dispose();
        driver = null;
    }

    public static void Quit()
    {
        if (_driver == null) return;
        _driver.Quit();
    }

    public static void Close()
    {
        if (_driver == null) return;
        _driver.Close();
        _driver.Quit();
        _driver.Dispose();
        _driver = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    public static void KillOpenChromeDrivers()
    {
        Process[] killChrome = Process.GetProcessesByName("chromedriver");

        foreach (var process in killChrome)
        {
            process.Kill(true);
        }
    }

    public static void KillOpenIEDrivers()
    {
        Process[] killInternetExplorer = Process.GetProcessesByName("iexplore.exe");

        foreach (var process in killInternetExplorer)
        {
            process.Kill();
        }
    }

    public static void Wait(TimeSpan timeSpan)
    {
        Thread.Sleep((int)(timeSpan.TotalSeconds * 1000));
    }
    public static void SetImplicitWaitSeconds(int seconds)
    {       
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
    }
    public static void SetDefaultImplicitWait()
    {
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(BaseConfig.DefaultImplicitlyWaitTimeInSeconds);       
    }
    public static void DeleteAllCookies()
    {
        _driver.Manage().Cookies.DeleteAllCookies();
    }

    public static void GoTo(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }
    public static object ExecuteScript(string script, params object[] args)
    {
        try
        {
            var js = _driver as IJavaScriptExecutor;
            var result = js.ExecuteScript(script, args);
            return result;
        }
        catch
        {

        }
        return null;
    }

    public static void WaitSeconds(int i)
    {
        Wait(TimeSpan.FromSeconds(i));
    }

    public static void SwitchToTab(string switchToNext)
    {
        var windowHandles = _driver.WindowHandles;

        if (windowHandles.Count < 2)
        {
            throw new InvalidOperationException("There are not enough tabs open to switch.");
        }

        if (switchToNext.Contains("Next"))
        {            
            int currentIndex = windowHandles.IndexOf(_driver.CurrentWindowHandle);
           
            int nextIndex = (currentIndex + 1) % windowHandles.Count;
            
            _driver.SwitchTo().Window(windowHandles[nextIndex]);
        }
        else
        {           
            _driver.SwitchTo().Window(windowHandles[0]);
        }
    }
}
