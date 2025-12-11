using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gherkin;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.Security.Authentication;
using RestSharp.Authenticators.OAuth2;
using Newtonsoft.Json.Linq;
using System.Drawing;
using Reqnroll;
using System.Diagnostics;

namespace HRMS_UIAutomation.Helpers
{
    public static class APIHelpers
    {
        private static ScenarioContext? _scenarioContext { get; set; }

        private static FeatureContext? _featureContext { get; set; }
        
        private static RestClientOptions? _restClientOptions { get; set; }

        private static RestRequest? request;

        private static RestResponse? response;
        public static void Init(FeatureContext featureContext, ScenarioContext scenarioContext, RestClient restClient, RestClientOptions restClientOptions)
        {

            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _restClientOptions = restClientOptions;
        }

        public static RestResponse MakeRequest(string _method, string _targerUrl, string token, string body)
        {

            if (_method == "GET" && _restClientOptions != null)
            {
                request = new RestRequest(_targerUrl);
                request.AddHeader("Accept", "application/json");
                if (body != null)
                    request.AddStringBody(body, DataFormat.Json);
                if (token != "")
                    _restClientOptions.Authenticator = new JwtAuthenticator(token);
                var _restClientGET = new RestClient(_restClientOptions);
                response = _restClientGET.ExecuteGetAsync(request).GetAwaiter().GetResult();
                return response;

            }
            else if (_method == "POST" && _restClientOptions != null)
            {
                request = new RestRequest(_targerUrl);
                request.AddHeader("Accept", "application/json");
                if (body != null)
                    request.AddStringBody(body, DataFormat.Json);
                _restClientOptions.Authenticator = new JwtAuthenticator(token);
                var _restClientPOST = new RestClient(_restClientOptions);
                response = _restClientPOST.ExecutePostAsync(request).GetAwaiter().GetResult();
                return response;

            }
            else if (_method == "PUT" && _restClientOptions != null)
            {
                request = new RestRequest(_targerUrl);
                request.AddHeader("Accept", "application/json");
                if (body != null)
                    request.AddStringBody(body, DataFormat.Json);
                _restClientOptions.Authenticator = new JwtAuthenticator(token);
                var _restClientPOST = new RestClient(_restClientOptions);
                response = _restClientPOST.ExecutePutAsync(request).GetAwaiter().GetResult();
                return response;

            }
            else if (_method == "DELETE" && _restClientOptions != null)
            {
                request = new RestRequest(_targerUrl);
                request.AddHeader("Accept", "application/json");
                if (body != null)
                    request.AddStringBody(body, DataFormat.Json);
                _restClientOptions.Authenticator = new JwtAuthenticator(token);
                var _restClientDELETE = new RestClient(_restClientOptions);
                response = _restClientDELETE.DeleteAsync(request).GetAwaiter().GetResult();
                return response;


            }
            else
            {
                return null;
            }
        }

        public static string getToken(string header)
        {
            if (_featureContext != null)
            {
                switch (header)
                {
                    case "JWTAuth":
                        _featureContext.TryGetValue("ftoken", out string token);
                        return token;

                    case "JWTAuthIncorrect":
                        return "njkshdkasn54543da5sd45sa4d5sa4d5s4d54sd4s54dsa4d45sa5d4s";

                    case "HMACAuthIncorrect":
                        return "dnsjkahdbkasbndjkasddkmsdklsma";

                    case "NoHeader":
                        return "";
                    default:
                        return null;
                }
            }
            else
                return null;
        }
    
    
    }
}