using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HRMS_UIAutomation.Enums;
using Microsoft.Extensions.Configuration.Json;


namespace HRMS_UIAutomation.Helpers
{
    internal static class BaseConfig
    {

        public static string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));       
        public static IConfigurationRoot BasicConfig => new ConfigurationBuilder()
        .AddJsonFile(path + "\\Config\\appsettings.json")
        .Build();            
        
        public static string Environment => string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("CURRENT_ENV")) ?  (BasicConfig?["environment"] ?? string.Empty): System.Environment.GetEnvironmentVariable("CURRENT_ENV");

        public static string Browser => GetBrowser();        

        private static IConfigurationRoot Config => new ConfigurationBuilder()
            .AddJsonFile(path + $"/Config/Environments/appsettings.{Environment.ToLower()}.json")            
            .Build();

        public static string BaseUrl => Config?["BaseUrl"] ?? string.Empty;
       
        public static string email => Config?["email"] ?? string.Empty;
        public static string password => Config?["password"] ?? string.Empty;

        public static bool IsHeadlessMode => GetHeadlessMode();   
       
        private static bool GetHeadlessMode()
        {
            if (string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("HEADLESS_MODE")))
            {
                return Convert.ToBoolean(Config["HeadlessMode"]);
            }
            else
            {
                return Convert.ToBoolean(System.Environment.GetEnvironmentVariable("HEADLESS_MODE"));
                
            }
        }

        private static string GetBrowser()
        {
            if (string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("BROWSER")))
            {
                return Config?["Browser"]?? string.Empty;
            }
            else
            {
                return System.Environment.GetEnvironmentVariable("BROWSER");

            }
        }
               
        public static Browser WebDriverType
        {
            get
            {
                return Enum.Parse<Browser>(Browser);
            }

        }
        public static int DefaultImplicitlyWaitTimeInSeconds { get; set; } = 30;
    }
}
