using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SeleniumExtras.WaitHelpers;
using HRMS_UIAutomation.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HRMS_UIAutomation.WebDriver;
namespace HRMS_UIAutomation.Extensions;

public static class ElementExtensions
{
    #region Wait methods

    public static void WaitForPageToLoad(string title, int seconds = 30)
    {
        try
        {
            waitForJQueryToLoad();
            WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
            wait.Until(e => e.Title.Contains(title));
        }
        catch (Exception)
        {
            throw new Exception($"The expected page title: {title}, does not match the actual page title: {Driver.Original.Title}");
        }
    }

    public static IWebElement WaitForElement(this By by, int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        return wait.Until(e => e.FindElement(by));
    }

    public static bool WaitForElementToBeNotPresent(this By by, int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        return wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
    }

    public static IWebElement WaitForElementToBeClickable(this By by, int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        return wait.Until(ExpectedConditions.ElementToBeClickable(by));
    }

    public static void WaitForElementToBeClickableThenClick(this By by, int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(by));
        element.Click();
    }

    public static void WaitForElementDisplayed(this IWebElement element, int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        wait.Until(x => element.Displayed);
    }

    public static void WaitForElementDisplayed(this string XPath, string texttoreplace, int seconds = 30)
    {
        By xpath = By.XPath(XPath.Replace("$$$", texttoreplace));
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        wait.Until(x => Driver.Original.FindElement(xpath).Displayed);
    }

    public static void WaitForElementEnabled(this IWebElement element, int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        wait.Until(x => element.Enabled);
    }

    public static void waitForJQueryToLoad(int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        wait.Until(
            d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

        try
        {
            wait.Until(
            d => ((IJavaScriptExecutor)d).ExecuteScript("return jQuery.active==0").Equals(true));
        }
        catch (Exception)
        {
            // no jQuery present
        }
    }

    public static IWebElement WaitForElement(this IWebElement element, int seconds = 30)
    {
        try
        {
            new Actions(Driver.Original).MoveToElement(element).Perform();
            var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(seconds));
            wait.Until(x => element.Displayed);
        }
        catch { }
        return element;
    }

    public static bool WaitForElementAndClick(this IWebElement element)
    {
        try { element.WaitForElement().Click(); return true; } catch { return false; }
    }

    #endregion


    #region Dropdown methods

    public static void SelectDropDownByText(this By by, string value)
    {
        Func<bool> func = () =>
        {
            new SelectElement(by.WaitForElement()).SelectByText(value);
            bool result = new SelectElement(by.WaitForElement()).SelectedOption.Text == value;
            return result;
        };
        Retry.Do(func, i => i == true);
    }

    public static void SelectDropDownByText(this IWebElement element, string value)
    {
        Func<bool> func = () =>
        {
            new SelectElement(element).SelectByText(value);
            bool result = new SelectElement(element).SelectedOption.Text == value;
            return result;
        };
        Retry.Do(func, i => i == true);
    }

    public static void SelectDropDownByText(this string XPath, string texttoreplace, string text)
    {
        By xpath = By.XPath(XPath.Replace("$$$", texttoreplace));
        xpath.SelectDropDownByText(text);
    }

    public static void SelectDropDownByValue(this By by, string value)
    {
        new SelectElement(by.WaitForElement()).SelectByValue(value);
    }
    public static void SelectDropDownByTextAndWait(this By by, string value)
    {
        Retry.Do(() => new SelectElement(by.WaitForElement()).SelectByText(value));
    }

    public static List<string> GetDropDownValues(this By element, int seconds = 30)
    {
        SelectElement dropDownElement = new SelectElement(Driver.Original.FindElement(element));
        var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(seconds));
        IList<IWebElement> dropdownOptions = dropDownElement.Options;
        return dropdownOptions.Select(option => option.Text.Trim()).ToList();
    }

    public static List<string> GetPaginationOptions(this By by, int seconds = 30)
    {
        var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(seconds));
        IList<IWebElement> dropdownOptions = wait.Until(driver => driver.FindElements(by));
        return dropdownOptions.Select(option => option.Text.Trim()).ToList();
    }

    #endregion


    #region Get Attribute methods

    public static bool IsDisabled(this IWebElement element)
    {
        try
        {
            return element.GetAttribute("aria-disabled") == "true" || !element.Enabled;
        }

        catch
        {
            return false;
        }
    }

    private static string GetElemetnId(IWebElement element)
    {
        Driver.SetImplicitWaitSeconds(0);
        string elementId = null;
        try
        {
            elementId = element.GetAttribute("id");
        }
        catch
        {
            //Element id is not present all the time   
        }
        try
        {
            if (string.IsNullOrWhiteSpace(elementId))
            {
                elementId = element.GetAttribute("class");
            }

        }
        catch
        {

        }
        finally
        {
            Driver.SetDefaultImplicitWait();
        }
        return elementId;
    }

    public static string GetAttributeVal(this IWebElement element, string attributeName)
    {
        return element.GetAttribute(attributeName);
    }

    #endregion


    #region Textbox methods

    public static void EnterText(this string XPath, string texttoreplace, string text)
    {
        By xpath = By.XPath(XPath.Replace("$$$", texttoreplace));
        xpath.EnterText(text);
    }

    public static void EnterText(this By by, string text, int seconds = 30)
    {
        var element = by.WaitForElement();
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        element = wait.Until(ExpectedConditions.ElementToBeClickable(by));
        element.Focus();
        element.Clear();
        element.SendKeys(text);
    }

    public static void Enter(this By by)
    {
        var element = by.WaitForElement();
        element.Focus();
        element.SendKeys(Keys.Enter);
    }

    public static void EnterText(this IWebElement element, string text, int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        element = wait.Until(ExpectedConditions.ElementToBeClickable(element));
        element.Focus();
        element.Clear();
        element.SendKeys(text);
    }

    public static void Escape(this By by)
    {
        var element = by.WaitForElement();
        element.Focus();
        element.SendKeys(Keys.Escape);
    }

    /// <summary>
    /// Validate if a specific text value is available on the page
    /// method takes a param as text to find.
    /// </summary>
    /// <param name="by"></param>
    /// <param name="text"></param>
    /// <returns>
    /// Returns true/false
    /// </returns>
    public static bool CheckIfTextIsPresent(this By by, string text, int seconds = 10)
    {
        IWebElement element = by.WaitForElement(seconds);
        element.WaitForElementDisplayed();
        return element.Text.Contains(text);
    }

    /// <summary>
    /// Validate if a specific text value is available on the page
    /// This method will replace $$$ in a defined XPath
    /// XPath supported only
    /// </summary>
    /// <param name="XPath"></param>
    /// <param name="texttoreplace"></param>
    /// <returns></returns>
    public static bool CheckIfTextIsPresent(this string XPath, string texttoreplace, int seconds = 10)
    {
        return By.XPath(XPath.Replace("$$$", texttoreplace)).CheckIfTextIsPresent(texttoreplace, seconds);
    }

    public static string GetTextValue(this IWebElement element)
    {
        return element.GetAttribute("value");
    }

    public static string GetText(this IWebElement element, int seconds = 30)
    {
        try
        {
            var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(seconds));
            wait.Until(x => element.Displayed);
            return element.Text;
        }
        catch (StaleElementReferenceException)
        {
            Retry.Do(() => element.WaitForElement(), null, 1);
            return element.Text;
            //return RetryingText(element, 1);
        }
        catch
        {
            throw;
        }
    }

    public static string GetTextFromInput(this By by)
    {
        return by.WaitForElement().GetTextValue();
    }

    public static void VerifyValue(this By by, string value)
    {
        Assert.IsTrue(by.WaitForElement().GetTextValue() == value);
    }

    public static void VerifyText(this By by, string value)
    {
        var webElement = Driver.Original.FindElement(by);
        Assert.IsTrue(webElement.WaitForElement().GetText() == value);
    }

    #endregion


    #region Checkbox methods

    public static void SelectCheckbox(this By by, int seconds = 30)
    {
        IWebElement checkbox = by.WaitForElement();
        if (!checkbox.Selected || checkbox.GetAttribute("value") == "false")
        {
            checkbox = by.WaitForElement().FindElement(By.XPath(".//../div"));
            WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(checkbox)).Click();
            }
            catch (ElementClickInterceptedException)
            {
                checkbox.ClickJs();
            }
        }
    }

    public static void SelectCheckbox(this IWebElement checkbox, int seconds = 30)
    {
        if (!checkbox.Selected || checkbox.GetAttribute("value") == "false")
        {
            checkbox = checkbox.FindElement(By.XPath(".//../div"));
            WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(checkbox)).Click();
            }
            catch (ElementClickInterceptedException)
            {
                checkbox.ClickJs();
            }
        }
    }

    public static void SelectCheckbox(this string XPath, string texttoreplace, int seconds = 30)
    {
        var xpath = By.XPath(XPath.Replace("$$$", texttoreplace));
        IWebElement checkbox = xpath.WaitForElement();
        if (!checkbox.Selected || checkbox.GetAttribute("value") == "false")
        {
            checkbox = xpath.WaitForElement().FindElement(By.XPath(".//../div"));
            WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(checkbox)).Click();
            }
            catch (ElementClickInterceptedException)
            {
                checkbox.ClickJs();
            }
        }
    }

    public static void DeSelectCheckbox(this By by, int seconds = 30)
    {
        IWebElement checkbox = by.WaitForElement();
        if (checkbox.Selected)
        {
            checkbox = by.WaitForElement().FindElement(By.XPath(".//../div"));
            WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(checkbox)).Click();
            }
            catch (ElementClickInterceptedException)
            {
                checkbox.ClickJs();
            }
        }
    }

    public static void DeSelectCheckbox(this IWebElement checkbox, int seconds = 30)
    {
        if (checkbox.Selected)
        {
            checkbox = checkbox.FindElement(By.XPath(".//../div"));
            WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(checkbox)).Click();
            }
            catch (ElementClickInterceptedException)
            {
                checkbox.ClickJs();
            }
        }
    }

    #endregion


    #region Click methods

    public static void Click(this By by, int seconds = 30)
    {
        try
        {
            waitForJQueryToLoad();
            by.WaitForElement(seconds);
            IWebElement element = WaitForElementToBeClickable(by);
            element.WaitForElementEnabled();
            element.Click();
        }
        catch (Exception ex)
        {
            if (ex is ElementClickInterceptedException || ex is ElementNotInteractableException)
            {
                by.WaitForElement().ClickJs();
            }
            else if (ex is StaleElementReferenceException)
            {
                Retry.Do(() => WaitForElementToBeClickableThenClick(by));
            }
            else
            {
                throw;
            }

        }
    }

    /// <summary>
    /// Find and click element by XPath/id/cssselector/etc
    /// Will dynamically wait for element before clicking
    /// </summary>
    /// <param name="by"></param>
    public static void ClickAndWait(this By by, int seconds = 30)
    {
        by.Click(seconds);
    }

    public static bool ClickByHovering(this IWebElement element)
    {
        try
        {
            Actions builder = new(Driver.Original);
            Actions hoverClick = builder.MoveToElement(element).MoveByOffset(5, 0).Click();
            hoverClick.Build().Perform();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void JavascriptClick(this By by, int seconds = 30)
    {
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        var result = wait.Until(e => e.FindElements(by));
        foreach (IWebElement element in result)
        {
            by.WaitForElement().ClickJs();
        }
    }

    public static bool ClickJs(this IWebElement element)
    {
        try
        {
            //element.WaitForElement();
            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)Driver.Original;
            javascriptExecutor.ExecuteScript("arguments[0].click();", element); return true;

        }
        catch { return false; }
    }

    /// <summary>
    /// Find and click element via XPath
    /// This method will replace $$$ in a defined XPath
    /// XPath supported only
    /// </summary>
    /// <param name="XPath"></param>
    /// <param name="texttoreplace"></param>
    public static void ClickAndWait(this string XPath, string texttoreplace)
    {
        By xpath = By.XPath(XPath.Replace("$$$", texttoreplace));
        xpath.Click();
    }

    public static void ClickAndWait(this IWebElement element)
    {
        try
        {
            string exception = String.Empty;
            string elementId = GetElemetnId(element);
            element.Focus();
            element.Highlight();

            if (!element.Displayed)
            {
                element.ClickJs();
            }
            else if (!element.WaitForElementAndClick())
            {
                if (!element.ClickByHovering())
                {
                    if (!element.ClickJs())
                    {
                        throw new ApplicationException("Failed to click the element");
                    }
                }

            }
        }
        catch (StaleElementReferenceException)
        {
            Retry.Do(() => element.WaitForElement(), null, 1);
            Retry.Do(() => element.Click(), null, 1);
            //RetryingClick(element, 1);
        }
        catch
        {
            throw;
        }

    }

    #endregion


    #region File methods

    public static void UploadFile(this By by, string filepath, int seconds = 30)
    {
        IAllowsFileDetection allowsDetection = Driver.Original as IAllowsFileDetection;
        if (allowsDetection != null)
        {
            allowsDetection.FileDetector = new LocalFileDetector();
        }
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));
        IWebElement element = wait.Until(e => e.FindElement(by));
        IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)Driver.Original;
        javascriptExecutor.ExecuteScript("arguments[0].style = ''; arguments[0].style.display = 'block'; arguments[0].style.visibility = 'visible';", element);
        element.SendKeys(filepath);
    }

    public static void DeleteFile(string expectedFilePath, string fileName)
    {
        File.Delete(expectedFilePath + fileName);
    }

    public static void WaitForFileDownload(string expectedFilePath, string fileName, int seconds = 30)
    {
        bool fileExists = false;
        IAllowsFileDetection allowsDetection = Driver.Original as IAllowsFileDetection;
        if (allowsDetection != null)
        {
            allowsDetection.FileDetector = new LocalFileDetector();
        }
        WebDriverWait wait = new(Driver.Original, TimeSpan.FromSeconds(seconds));

        try
        {
            Assert.IsTrue(wait.Until<bool>(x => fileExists = Directory.EnumerateFiles(expectedFilePath, fileName).Any()));
        }
        catch
        {
            // this page doesn't work in headless mode (shows a blank screen)
            //Driver.Original.Navigate().GoToUrl("chrome://downloads/");
            //IJavaScriptExecutor js = (IJavaScriptExecutor)Driver.Original;
            //String script = "return document.querySelector('downloads-manager').shadowRoot.querySelector('#downloadsList').items.filter(e => e.state === 'COMPLETE').map(e => e.filePath || e.file_path || e.fileUrl || e.file_url);";
            ////String script = "return downloads.Manager.get().items_.filter(e => e.state === 'COMPLETE').map(e => e.file_url);";
            //var files = js.ExecuteScript(script).ToString();
            //throw new Exception("File has not downloaded.  Files in " + Settings.DownloadsDir + ": " + files);
            string[] files = Directory.GetFiles(expectedFilePath);
            var separator = ", "; 
            string filesList = string.Join(separator, files);

            throw new Exception($"File: {fileName} has not downloaded after {seconds} seconds.  List of files found:  {filesList}");
        }
    }
     

    #endregion


    #region Find element methods

    public static IWebElement FindElementByXPath(this string XPath, string texttoreplace)
    {
        By xpath = By.XPath(XPath.Replace("$$$", texttoreplace));
        IWebElement element = xpath.WaitForElement();
        return element;
    }

    public static List<IWebElement> FindElementsByXPath(this string XPath, string texttoreplace)
    {
        By xpath = By.XPath(XPath.Replace("$$$", texttoreplace));
        List<IWebElement> elements;

        try
        {
            elements = Driver.Original.FindElements(xpath).ToList();
        }
        catch (Exception e)
        {

            throw new Exception(e.Message);
        }
        return elements;
    }

    #endregion


    #region Verify methods

    public static bool VerifyErrorMessageDisplayedByPressingEnterKey(string message, int seconds = 10)
    {
        var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(seconds));
        By errorMessage = By.XPath($"//div[contains(text(),'{message}')]");

        try
        {
            var errorMessageElement = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(errorMessage)).FirstOrDefault();
            if (errorMessageElement != null)
            {
                Console.WriteLine("Error message found and visible.");
                return true;
            }
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("HTML-based error message not found.");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Timeout occurred waiting for error message element.");
        }

        return false;
    }

  
    public static void VerifyTableRow(this By by, Table table, bool noHeaders = false)
    {
        by.WaitForElement().WaitForElementDisplayed();
        // Check headers (if applicable)
        if (!noHeaders)
        {
            var headers = by.WaitForElement().FindElements(By.XPath(".//thead/tr[last()]/th"));
            if (headers.Count == 0) // if .//thead is not present
            {
                headers = by.WaitForElement().FindElements(By.XPath(".//tbody/tr/th"));
            }

            if (headers.Count != table.Header.Count)
            {
                throw new Exception($"Expected number of columns {table.Header.Count} does not match Actual number of columns {headers.Count}");
            }

            for (int i = 0; i < table.Header.Count; i++)
            {
                try
                {
                    Assert.IsTrue(headers[i].Text == table.Header.ElementAt(i));
                }
                catch (Exception)
                {
                    throw new Exception($"The expected column header: {table.Header.ElementAt(i)}, does not match the actual column header: {headers[i].Text}");
                }
            }
        }

        if (table.Rows.Count != 0)
        {
            var rows = Driver.Original.FindElements(By.XPath(".//tbody/tr"));
            var columns = table.Header.Count;
            bool finder = false;
            for (int i = 1; i <= rows.Count; i++)
            {
                if (!finder)
                {
                    var row = by.WaitForElement().FindElement(By.XPath(".//tbody/tr[" + i + "]/td[" + 1 + "]"));
                    string rowText = row.Text;
                    if (rowText != table.Rows[0].Values.ElementAt(0))
                    {

                    }
                    else
                    {
                        Assert.IsTrue(rowText == table.Rows[0].Values.ElementAt(0));
                        for (int t = 2; t <= columns; t++)
                        {
                            row = by.WaitForElement().FindElement(By.XPath(".//tbody/tr[" + i + "]/td[" + t + "]"));
                            rowText = row.Text;
                            if (rowText != table.Rows[0].Values.ElementAt(t - 1))
                            {
                                break;
                            }
                            else
                            {
                                Assert.IsTrue(rowText == table.Rows[0].Values.ElementAt(t - 1));
                                if (t == columns)
                                {
                                    finder = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (!finder)
            {
                throw new Exception($"The row does not exist in the table");
            }
        }
    }

    public static void VerifyRowCount(this By by, int count)
    {
        var rows = by.WaitForElement().FindElements(By.XPath(".//tbody/tr"));
        rows.Count.Should().Be(count);
    }

    public static void VerifyValuesSorted(this By by, string columnName, string order)
    {
        int columnIndex = 0;
        var headers = by.WaitForElement().FindElements(By.XPath("//thead/tr[1]/th"));
        for (int i = 1; i <= headers.Count; i++)
        {
            var header = by.WaitForElement().FindElement(By.XPath(".//thead/tr[1]/th[" + i + "]"));
            string headerText = header.Text;
            if (headerText == columnName)
            {
                columnIndex = i;
            }
        }
        var rows = by.WaitForElement().FindElements(By.XPath(".//tbody/tr"));
        string[] rowValues = new string[rows.Count];
        for (int i = 0; i < rows.Count; i++)
        {
            int j = i + 1;
            var row = by.WaitForElement().FindElement(By.XPath(".//tbody/tr[" + j + "]/td[" + columnIndex + "]"));
            if (columnName == "Employee")
            {
                string[] words = row.Text.Split(' ');
                rowValues[i] = words[2];
            }
            else
                rowValues[i] = row.Text;
        }
        IsSorted(rowValues, order).Should().Be(true);
    }

    public static bool IsSorted(string[] array, string order)
    {
        if (order == "ascending")
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (string.Compare(array[i], array[i + 1]) > 0)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (string.Compare(array[i], array[i + 1]) < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public static void VerifyFields(this string XPath, Table table, string textToReplace)
    {
        var element = XPath.FindElementByXPath(textToReplace);
        foreach (var row in table.Rows)
        {
            By fieldXpath = By.XPath($"(.//label[normalize-space(text())='{row[0]}'])[last()]");
            if (row[0].Contains('\''))
            {
                fieldXpath = By.XPath($"(.//label[normalize-space(text())=\"{row[0]}\"])[last()]");
            }

            IWebElement field;

            try
            {
                field = element.FindElement(fieldXpath).WaitForElement();               
            }
            catch
            {
                try
                {
                    fieldXpath = By.XPath($".//*[normalize-space(text())='{row[0]}']");
                    field = element.FindElement(fieldXpath).WaitForElement();
                }
                catch
                {
                    fieldXpath = By.XPath($".//*[@data-id='{row[0]}'] | .//*[@id='{row[0]}']");
                    field = (WebElement)element.FindElement(fieldXpath).WaitForElement();
                }
            }

            field.WaitForElementDisplayed();
            if (table.Header.Count > 2 && row[2] == "dropdown")
            {
                field = field.FindElement(By.XPath("./..//button")); 
            }
            string fieldValue;

            try
            {
                fieldValue = ElementExtensions.GetAttributeVal(field.FindElement(By.XPath("./..//input")), "value");
            }
            catch
            {
                field = field.FindElement(By.XPath("./..//select"));
                fieldValue = ElementExtensions.GetAttributeVal(field, "value");
            }

            Assert.AreEqual(row[1], fieldValue);
        }
    }

    public static void VerifyFieldValues(this string XPath, Table table, string textToReplace)
    {
        var element = XPath.FindElementByXPath(textToReplace);

        foreach (var row in table.Rows)
        {
            By fieldXpath = By.XPath($"(.//label[normalize-space(text())='{row[0]}'])[last()]");
            if (row[0].Contains('\''))
            {
                fieldXpath = By.XPath($"(.//label[normalize-space(text())=\"{row[0]}\"])[last()]");
            }

            IWebElement field;

            try
            {
                field = element.FindElement(fieldXpath).WaitForElement();
            }
            catch
            {
                try
                {
                    fieldXpath = By.XPath($".//*[normalize-space(text())='{row[0]}']");
                    field = element.FindElement(fieldXpath).WaitForElement();
                }
                catch
                {
                    fieldXpath = By.XPath($".//*[@data-id='{row[0]}'] | .//*[@id='{row[0]}']");
                    field = element.FindElement(fieldXpath).WaitForElement();
                }
            }

            field.WaitForElementDisplayed();

            string fieldValue = "";

            try
            {
                if (table.Header.Count > 2 && row[2].ToLower() == "dropdown") 
                {
                    field = field.FindElement(By.XPath("./..//select"));
                    var select = new SelectElement(field);
                    fieldValue = select.SelectedOption.Text.Trim();
                }
                else if (table.Header.Count > 2 && row[2].ToLower() == "checkbox") 
                {
                    var checkbox = field.FindElement(By.XPath("./..//input[@type='checkbox']"));
                    fieldValue = checkbox.Selected ? "true" : "false";
                }
                else
                {
                    fieldValue = ElementExtensions.GetAttributeVal(field.FindElement(By.XPath("./..//input")), "value");
                }
            }
            catch
            {
                throw new Exception($"Failed to retrieve value for field '{row[0]}'");
            }

            Assert.AreEqual(row[1], fieldValue, $"Field '{row[0]}' did not match. Expected: '{row[1]}', Actual: '{fieldValue}'");
        }
    }

    public static void VerifyStatusIcons(this string XPath, Table table, string textToReplace)
    {
        var element = XPath.FindElementByXPath(textToReplace);
        foreach (var row in table.Rows)
        {
            string featureName = row["Field Name"];
            bool expectedCheckedStatus = bool.Parse(row["Value"]);
            var statusIconXPath = By.XPath($"//div[p[text()='{featureName}']]/preceding-sibling::div[1]//i");
            var statusIcon = element.FindElement(statusIconXPath);
            bool isIconCheck = statusIcon.GetAttribute("class").Contains("fa-check");
            Assert.AreEqual(expectedCheckedStatus, isIconCheck, $"Expected status for '{featureName}' to be '{expectedCheckedStatus}', but found '{isIconCheck}'.");
        }
    }

    public static void VerifyErrorMessages(this string XPath, Table table, string textToReplace)
    {
        var element = XPath.FindElementByXPath(textToReplace);
        foreach (var row in table.Rows)
        {
            By errorMessageXPath = By.XPath($".//span[@id='{row[0]}']");
            var errorMessageElement = element.FindElement(errorMessageXPath).WaitForElement();
            errorMessageElement.WaitForElementDisplayed();
            string actualErrorMessage = errorMessageElement.Text;
            Assert.IsTrue(row[1] == actualErrorMessage, $"Expected error '{row[1]}' but found '{actualErrorMessage}' for field {row[0]}.");
        }
    }

    #endregion

    public static void UpdateFields(this string XPath, Table table, string texttoreplace, string company)
    {
        foreach (var row in table.Rows)
        {
            var element = XPath.FindElementByXPath(texttoreplace);

            By fieldXpath = By.XPath($".//label[text()='{row[0]}']");
            if (row[0].Contains('\''))
            {
                fieldXpath = By.XPath($".//label[text()=\"{row[0]}\"]");
            }
            try
            {
                element = element.FindElement(fieldXpath).WaitForElement();
            }
            catch
            {
                try
                {
                    fieldXpath = By.XPath($".//*[normalize-space(text())='{row[0]}']");
                    if (row[0].Contains('\''))
                    {
                        fieldXpath = By.XPath($".//*[normalize-space(text())=\"{row[0]}\"]");
                    }
                    element = element.FindElement(fieldXpath).WaitForElement();
                }
                catch
                {
                    fieldXpath = By.XPath($".//*[@data-id='{row[0]}'] | .//*[@id='{row[0]}']");
                    element = element.FindElement(fieldXpath).WaitForElement();
                }
            }
            try
            {
                element.WaitForElementDisplayed();
            }
            catch { }
            if (table.Header.Count > 2)
            {
                if (row[2] == "dropdown")
                {
                    element = element.FindElement(By.XPath("./..//button"));
                }
                else
                {
                    try
                    {
                        element = element.FindElement(By.XPath("./..//input"));
                    }
                    catch
                    {
                        element = element.FindElement(By.XPath("./../..//input"));
                    }
                }
            }
            else
            {
                element = element.FindElement(By.XPath("./..//input"));
            }

            string fieldType;
            try
            {
                fieldType = ElementExtensions.GetAttributeVal(element, "type");
            }
            catch
            {
                fieldType = "";
            }

            if (fieldType == "checkbox")
            {
                if (row[1] == "true")
                {
                    element.SelectCheckbox();
                }
                else
                {
                    element.DeSelectCheckbox();
                }
            }
            else if (fieldType == "text" || fieldType == "")
            {
                element.EnterText(row[1]);
            }
            else if (fieldType == "hidden" || fieldType == "button")
            {
                try
                {
                    element = element.FindElement(By.XPath("./..//select"));
                    if (row[1] == "{EmployerName}")
                    {
                        element.SelectDropDownByText(company);
                    }
                    else
                    {
                        element.SelectDropDownByText(row[1]);
                    }
                }
                catch
                {
                    element = element.FindElement(By.XPath($"//input[@id='{row[0]}']"));
                    element.EnterText(row[1]);
                }
            }
            else if (fieldType == "password" || fieldType == "number")
            {
                element = element.FindElement(By.XPath("./..//input"));
                element.EnterText(row[1]);
            }
        }
    }

    public static void Focus(this IWebElement element)
    {
        try { new Actions(Driver.Original).MoveToElement(element).Perform(); }
        catch { }

    }

    public static void Highlight(this IWebElement element)
    {
        try
        {
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            Driver.ExecuteScript(highlightJavascript, new object[] { element });
        }
        catch (Exception)
        {
            //Highlighting is not important. Do not break if this doesnt work
        }

    }

    public static void ScrollToPosition(this IWebElement element)
    {
        try
        {

            
            var yPosition = element.Location.Y;
            if (yPosition > 200)
                yPosition -= 100;
            var js = String.Format("window.scrollTo({0}, {1})", 0, yPosition);
            // jse.ExecuteScript(js);
            // Thread.Sleep(1000);
            Driver.ExecuteScript("arguments[0].scrollIntoView();", element);
        }
        catch
        { }
    }

    public static void SwitchToDialog()
    {
        Driver.Original.SwitchTo().DefaultContent();
    }

    public static void SwitchToFrame(this By by)
    {
        IWebElement frame = by.WaitForElement();
        Driver.Original.SwitchTo().Frame(frame);
    }

    public static bool IsVisible(this By by)
    {
        return by.WaitForElement().Displayed;
    }

    public static bool IsElementPresent(this By by)
    {
        try
        {
            Driver.Original.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    //public static string RetryingText(IWebElement webElement, int noOfAttempts)
    //{
    //    string result = null;
    //    int attempts = 0;
    //    while (attempts < noOfAttempts)
    //    {
    //        try
    //        {
    //            webElement.WaitForElement();
    //            result = webElement.Text;
    //            break;
    //        }
    //        catch (StaleElementReferenceException)
    //        {

    //        }
    //        attempts++;
    //    }
    //    return result;
    //}

    //public static Boolean RetryingClick(IWebElement webElement, int noOfAttempts)
    //{
    //    Boolean result = false;
    //    int attempts = 0;
    //    while (attempts < noOfAttempts)
    //    {
    //        try
    //        {
    //            webElement.WaitForElement();
    //            webElement.Click();
    //            result = true;
    //            break;
    //        }
    //        catch (StaleElementReferenceException)
    //        {

    //        }
    //        attempts++;
    //    }
    //    return result;
    //}

}