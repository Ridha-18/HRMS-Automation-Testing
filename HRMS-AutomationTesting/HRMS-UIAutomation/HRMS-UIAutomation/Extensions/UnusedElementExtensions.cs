using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using HRMS_UIAutomation.WebDriver;

namespace HRMS_UIAutomation.Extensions;

public static class ReadOnlyCollection
{
    public static IReadOnlyCollection<TResult> Empty<TResult>()
    {
        return EmptyReadOnlyCollection<TResult>.Instance;
    }

    private static class EmptyReadOnlyCollection<TElement>
    {
        static volatile TElement[] _instance;

        public static IReadOnlyCollection<TElement> Instance
        {
            get { return _instance ??= Array.Empty<TElement>(); }
        }
    }




    public static void ClickElementByHovering(this IWebElement element)
    {
        Actions builder = new(Driver.Original);
        Actions hoverClick = builder.MoveToElement(element).Click();
        hoverClick.Build().Perform();
    }


    // ***** Commented out until a suitable pdf reader is found

    //public static void VerifyPdfContent(string file1, Table table)
    //{
    //    PdfReader pdfReader = new PdfReader(file1);
    //    PdfDocument pdfDoc = new PdfDocument(pdfReader);
    //    string pageContent = "";
    //    string expected = "";
    //    string actual = "";

    //    for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
    //    {
    //        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
    //        pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
    //    }
    //    pdfDoc.Close();
    //    pdfReader.Close();

    //    var pdfContent = Regex.Split(pageContent, "\r\n|\r|\n");
    //    if (table.Header.ElementAt(1) == "Verify")
    //    {
    //        expected = Regex.Replace(table.Header.ElementAt(0), @"\s+", "");
    //        actual = Regex.Replace(pdfContent[0], @"\s+", "");
    //        try
    //        {
    //            Assert.IsTrue(actual == expected);
    //        }
    //        catch (Exception)
    //        {
    //            throw new Exception($"The expected header: {expected}, does not match the actual row: {actual}");
    //        }
    //    }

    //    for (int i = 0; i < table.Rows.Count; i++)
    //    {
    //        if (table.Rows[i].Values.ElementAt(1) == "Verify")
    //        {
    //            expected = Regex.Replace(table.Rows[i].Values.ElementAt(0), @"\s+", "");
    //            actual = Regex.Replace(pdfContent[i + 1], @"\s+", "");
    //            try
    //            {
    //                Assert.IsTrue(actual == expected);
    //            }
    //            catch (Exception)
    //            {
    //                throw new Exception($"The expected row: {expected}, does not match the actual row: {actual}");
    //            }
    //        }
    //    }
    //}


    public static string GetTextByScrollingToElement(this IWebElement element)
    {
        element.ScrollToPosition();
        element.Focus();
        return ElementExtensions.GetText(element);
    }
    public static IList<IWebElement> GetChildElements(this IWebElement element, int seconds = 30)
    {
        var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(seconds));
        wait.Until(x => element.Displayed);
        return element.FindElementsByCssSelector("*");
    }

    public static List<IWebElement> FindElementsByCssSelector(this IWebElement webElement, string selector)
    {
        List<IWebElement> elements;

        try
        {
            elements = webElement.FindElements(By.CssSelector(selector)).ToList();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        return elements;
    }
    public static List<IWebElement> FindElementsByAttribute(this IWebElement element, string attributeName, string attributeValue)
    {
        try
        {
            return element.FindElements(By.CssSelector($"[{attributeName}='{attributeValue}']")).ToList();
        }
        catch (Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public static string GetElementHtml(this By by)
    {
        return by.WaitForElement().GetAttribute("outerHTML");
    }

    public static string GetTextNotDisplayed(this IWebElement element)
    {
        return GetInnerText(element);
    }
    public static string GetTextAndWaitManually(this IWebElement element)
    {
        element.WaitForElementManually();
        return element.Text;
    }
    public static string GetTextNoWait(this IWebElement element)
    {

        Driver.SetImplicitWaitSeconds(1);
        try
        {
            return element.Text;

        }
        catch (NoSuchElementException)
        {
            return null;
        }
        finally
        {
            Driver.SetDefaultImplicitWait();
        }
    }
    public static bool IsTextBox(this IWebElement element)
    {
        bool isTextBox = false;
        try
        {
            isTextBox = element.GetAttribute("type") == "text";
        }
        catch { }
        return isTextBox;
    }
    //public static IWebElement FindElementNoWait(this IWebDriver driver, By by)
    //{
    //    Driver.SetImplicitWaitSeconds(1);
    //    try
    //    {
    //        IWebElement element = null;
    //        element = Driver.Original.FindElement(by);
    //        return element;
    //    }
    //    catch (Exception e)
    //    {
    //        //throw new Exception(e.Message);
    //        return null;
    //    }
    //    finally
    //    {
    //        Driver.SetDefaultImplicitWait();
    //    }
    //}

    public static string GetInnerText(this IWebElement element)
    {
        return element.GetAttribute("innerText");
    }

    public static ReadOnlyCollection<IWebElement> FindElementsNoWait(this IWebElement element, By by)
    {
        Driver.SetImplicitWaitSeconds(1);
        try
        {
            return element.FindElements(by);

        }
        catch (NoSuchElementException)
        {
            return null;
        }
        finally
        {
            Driver.SetDefaultImplicitWait();
        }
    }
    public static IWebElement FindElementNoWait(this IWebElement element, By by)
    {
        Driver.SetImplicitWaitSeconds(1);
        try
        {
            return element.FindElement(by);

        }
        catch (NoSuchElementException)
        {
            return null;
        }
        finally
        {
            Driver.SetDefaultImplicitWait();
        }
    }


    public static bool IsClickable(this IWebElement element)
    {
        Driver.SetImplicitWaitSeconds(0);
        try
        {
            element.Click();
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            Driver.SetDefaultImplicitWait();
        }

    }

    public static IWebElement FindElementByAttribute(this IWebElement webElement, string attribute, string value)
    {
        try
        {
            return webElement.FindElement(By.CssSelector($"[{attribute}='{value}']"));
        }
        catch (Exception e)
        {
            // QAAssert.LogError(e.Message, e.StackTrace);
            throw new Exception(e.Message);
        }
    }
    public static string GetDropDownValue(this IWebElement element)
    {
        var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(30));
        wait.Until(x => element.Displayed);
        return new SelectElement(element).SelectedOption.GetTextValue();
    }



    public static List<IWebElement> FindElementByTagName(this IWebElement webElement, string tag)
    {
        try
        {
            return webElement.FindElements(By.TagName(tag)).ToList();
        }
        catch (Exception e)
        {

            throw new Exception(e.Message);
        }
    }


    public static bool IsHidden(this IWebElement element)
    {
        return (element == null) || !element.Displayed;
    }

    public static bool Hover(this IWebElement element)
    {
        try
        {
            Actions builder = new(Driver.Original);
            Actions hoverClick = builder.MoveToElement(element).MoveByOffset(5, 0);
            hoverClick.Build().Perform();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void EnterTextAndTriggerChangeEvent(this IWebElement element, string value)
    {
        try
        {
            // element.Click();
            //   element.EnterTextJs("");
            for (int i = 0; i < element.GetAttribute("value").Length; i++)
            {
                element.SendKeys(Keys.Backspace);
            }
            element.SendKeys(Keys.Backspace);
            element.SendKeys(Keys.Backspace);
            // element.EnterTextJs(value);
            element.EnterText(value);
            Driver.ExecuteScript("$(arguments[0]).change();return true;", element);
            Driver.Original.FindElement(By.TagName("body")).Click();

        }
        catch
        {
            throw;
        }
    }
    public static void EnterTextJsAndTriggerChangeEvent(this IWebElement element, string value)
    {
        try
        {
            // element.Click();
            //   element.EnterTextJs("");
            for (int i = 0; i < element.GetAttribute("value").Length; i++)
            {
                element.SendKeys(Keys.Backspace);
            }
            element.SendKeys(Keys.Backspace);
            element.SendKeys(Keys.Backspace);
            // element.EnterTextJs(value);
            element.EnterTextJs(value);
            Driver.ExecuteScript("$(arguments[0]).change();return true;", element);
            Driver.Original.FindElement(By.TagName("body")).Click();

        }
        catch
        {
            throw;
        }
    }

    public static void EnterTextJs(this IWebElement element, string value)
    {
        try
        {
            // var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(30));
            // wait.Until(d => element.Displayed);
            // element.ScrollToPosition();
            // element.Clear();
            // element.Highlight();
            Driver.ExecuteScript("arguments[0].setAttribute('value', arguments[1])", element, value);
        }
        catch
        {
            throw;
        }
    }

    public static void TriggerChangeEvent(this IWebElement element)
    {
        try
        {
            element.Click();
            Driver.ExecuteScript("$(arguments[0]).change();return true;", element);
            Driver.Original.FindElement(By.TagName("body")).Click();
        }
        catch
        {
            // throw ex;
        }
    }

    public static void EnterTextAndNoWait(this IWebElement element, string value)
    {
        Driver.SetImplicitWaitSeconds(1);
        try
        {
            element.SendKeys(value);
        }
        catch
        {
        }
        finally
        {
            Driver.SetDefaultImplicitWait();
        }
    }
    public static void EnterTextAndWaitManually(this IWebElement element, string value)
    {
        try
        {
            element.WaitForElementManually();
            element.ScrollToPosition();
            element.Clear();

            element.SendKeys(value);
        }
        catch
        {
            throw;
        }
    }

    public static void ClickAndWaitManually(this IWebElement element)
    {
        element.WaitForElementManually();
        element.Click();
    }

    public static bool HasAttribute(this IWebElement element, string text)
    {
        try
        {
            if (element.GetAttribute(text) != null)
                return true;
        }
        catch
        {

        }
        return false;
    }


    /// <summary>
    /// This method only works if element has id
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static string GetTextJS(this IWebElement element)
    {
        element.ScrollToPosition();
        var id = element.GetAttribute("id");
        var jsToExecute = $"return $(\"#{id}\").val() ";
        return Driver.ExecuteScript(jsToExecute).ToString();
    }

    public static bool ClickJsNoWait(this IWebElement element)
    {
        Driver.SetImplicitWaitSeconds(1);
        try
        {
            Driver.ExecuteScript("arguments[0].click()", element); return true;
        }
        catch { return false; }
        finally
        {
            Driver.SetDefaultImplicitWait();
        }
    }

    public static IWebElement WaitForElementPeformance(this IWebElement element)
    {
        var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(30));
        wait.Until(x => element.Displayed);
        return element;
    }

    //public static bool SetPageZoomLevel(this IWebElement element)
    //{
    //    try
    //    {
    //        //Actions builder = new Actions(Driver.Original);
    //        //Actions hoverClick = builder.MoveToElement(element).MoveByOffset(5, 0);
    //        //hoverClick.Build().Perform();
    //        element.SendKeys(Keys.Cho);
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        //throw ex;
    //        return false;
    //    }
    //}

    //public static IWebElement FindElementsByMatchingText(this IWebElement webElement, By by, string text)
    //{

    //    IWEBELEMENT ELEMENT = NULL;

    //    TRY
    //    {
    //        var elements = webElement.FindElements(by).ToList();
    //        foreach (var liElement in elements)
    //        {
    //            if (liElement.GetText().ToLower() == text.ToLower())
    //            {
    //                element = liElement;
    //                break;
    //            }
    //        }

    //    }
    //    catch (Exception e)
    //    {

    //        throw new Exception(e.Message);
    //    }
    //    return element;
    //}

    public static void ClickOnElementByText(this IReadOnlyCollection<IWebElement> elements, string text)
    {
        foreach (var webElement in elements)
        {
            if (webElement.GetText().ToLower() == text.ToLower())
            {
                webElement.Focus();
                webElement.ClickAndWait();
                //  webElement.ClickByHovering();

            }
        }

    }


    public static void SwitchToNewWindow(this IWebElement element)
    {
        string foundHandle = null;
        // string originalWindowHandle = Driver.Original.CurrentWindowHandle;
        IList<string> existingHandles = Driver.Original.WindowHandles;
        element.ClickAndWait();
        DateTime timeout = DateTime.Now.Add(TimeSpan.FromSeconds(10));
        while (DateTime.Now < timeout)
        {
            IList<string> currentHandles = Driver.Original.WindowHandles;
            IList<string> differentHandles = currentHandles.Except(existingHandles).ToList();
            if (differentHandles.Count > 0)
            {
                // There will ordinarily only be one handle in this list,
                // so it should be safe to return the first one here.
                foundHandle = differentHandles[0];
                break;
            }

            // Sleep for a very short period of time to prevent starving the driver thread.
            System.Threading.Thread.Sleep(250);
        }

        if (string.IsNullOrEmpty(foundHandle))
        {
            throw new Exception("didn't find popup window within timeout");
        }
        Driver.Original.SwitchTo().Window(foundHandle);
    }

    public static string GetTextAndWait(this IWebElement element, int seconds = 30)
    {
        try
        {
            element.WaitForElementManually();
            var wait = new WebDriverWait(Driver.Original, TimeSpan.FromSeconds(seconds));
            wait.Until(d => element.Displayed);
            element.ScrollToPosition();
            return element.Text;
        }
        catch
        {
            throw;
        }
    }

    public static IWebElement WaitForElementManually(this IWebElement element)
    {
        var timeout = DateTime.Now.AddSeconds(30);
        while (DateTime.Now < timeout)
        {
            try
            {
                if (element.Displayed)
                    break;
                Thread.Sleep(250);
            }

            catch

            {
                Thread.Sleep(250);
            }
        }
        //if (element == null)
        //    throw new ApplicationException(string.Format("Not found or timeout exceeded waiting"));
        return element;
    }


    public static bool IsTextFound(this IList<IWebElement> elements, string text)
    {
        bool found = false;
        foreach (var recordid in elements)
        {
            if (recordid.GetTextAndWait().ToLower().Trim().Contains(text.ToLower().Trim()))
            {
                found = true;
                break;
            }

        }

        return found;

    }
    public static bool ClickElement(this IList<IWebElement> elements, string text)
    {
        bool found = false;
        var localElements = elements.ToList();
        foreach (var recordid in localElements)
        {
            if (recordid.GetText().Contains(text))
            {
                found = true;
                recordid.ClickJsWait();
                break;
            }
        }
        return found;

    }

    public static bool ClickJsWait(this IWebElement element)
    {
        try
        {
            element.WaitForElement();
            Driver.ExecuteScript("arguments[0].click()", element);
            return true;
        }
        catch { return false; }
    }

    //public static bool SetPageZoomLevel(this IWebElement element)
    //{
    //    try
    //    {
    //        //Actions builder = new Actions(Driver.Original);
    //        //Actions hoverClick = builder.MoveToElement(element).MoveByOffset(5, 0);
    //        //hoverClick.Build().Perform();
    //        element.SendKeys(Keys.Cho);
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        //throw ex;
    //        return false;
    //    }
    //}

    //public static IWebElement FindElementsByMatchingText(this IWebElement webElement, By by, string text)
    //{

    //    IWEBELEMENT ELEMENT = NULL;

    //    TRY
    //    {
    //        var elements = webElement.FindElements(by).ToList();
    //        foreach (var liElement in elements)
    //        {
    //            if (liElement.GetText().ToLower() == text.ToLower())
    //            {
    //                element = liElement;
    //                break;
    //            }
    //        }

    //    }
    //    catch (Exception e)
    //    {

    //        throw new Exception(e.Message);
    //    }
    //    return element;
    //}



    public static bool HasValidationError(this IWebElement element, string error)
    {
        return element
             .GetAttribute("title")
             .ToLower()
             .Contains(error.ToLower());
    }


    //public static IList<IWebElement> FindElementByTagName(this IWebElement webElement, string tag)
    //{
    //    try
    //    {
    //        return webElement.FindElements(By.TagName(tag)).ToList();
    //    }
    //    catch (Exception e)
    //    {
    //       // QAAssert.LogError(e.Message, e.StackTrace);
    //        throw new Exception(e.Message);
    //    }
    //}

}


