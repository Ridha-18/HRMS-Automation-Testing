using System.Reflection;
using HRMS_UIAutomation.Helpers;
using HRMS_UIAutomation.WebDriver;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;
using RestSharp;

namespace HRMS_UIAutomation.Hooks
{
    [Binding]
    public class Hooks
    {
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        private readonly IObjectContainer _objectContainer;
        public static string ReportPath;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        [BeforeScenario(Order = 0)]
        public void SetUp(ScenarioContext scenarioContext)
        {
            // Initialize WebDriver for this scenario to prevent NullReferenceException
            Driver.Init(scenarioContext);
        }

        public static byte[] CaptureScreenshot()
        {
            return ((ITakesScreenshot)Driver.Original).GetScreenshot().AsByteArray;
        }

        [AfterScenario(Order = 0)]
        public void TearDown()
        {
            // Quit the driver after each scenario if initialized
            if (Driver.Instance != null)
            {
                Driver.Quit();
            }
        }

        [AfterFeature(Order = 0)]
        public static void QuitWebDriver()
        {
            if (Driver.Original != null)
            {
                Driver.Original.Quit();
            }
        }

        [BeforeTestRun(Order = 1)]
        public static void BeforeTestRunSetup()
        {
            LogAppsettingsValues();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Can add any cleanup code if needed
        }

        private static void LogAppsettingsValues()
        {
            _log.Debug("Log appsetings values for execution environment: " + BaseConfig.Environment);

            Type t = typeof(BaseConfig);
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);

            foreach (PropertyInfo pi in properties)
            {
                _log.Debug("{0} = {1}", pi.Name, pi.GetValue(null));
            }
        }
    }
}
