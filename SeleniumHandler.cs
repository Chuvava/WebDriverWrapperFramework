using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace WebDriverWrapper
{
    public class SeleniumHandler
    {
        private string webDriverParams = "{\"Driver\":\"Chrome\"}";
        public string WebDriverParams
        {
            get
            {
                return webDriverParams;
            }
            set
            {
                webDriverParams = value;
            }
        }
        private IWebDriver webDriver = null;
        public IWebDriver WebDriver
        {
            get
            {
                if (webDriver == null)
                {
                    webDriver = SetWebDriver();
                }
                return webDriver;
            }

        }

        private IWebDriver SetWebDriver()
        {
            try
            {
                var driverParams = JObject.Parse(WebDriverParams);
                if (driverParams["Driver"].ToString()=="Firefox")
                {
                    return SetFirefoxDriver();
                }
                else if (driverParams["Driver"].ToString() == "IE")
                {
                    return SetInternetExplorerDriver();
                }
                else if (driverParams["Driver"].ToString() == "Chrome")
                {
                    return SetChromeDriver();
                }
                return SetChromeDriver();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IWebDriver SetFirefoxDriver()
        {
            try
            {
                var options = new FirefoxOptions();
                options.AcceptInsecureCertificates = true;

                return new FirefoxDriver(options);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IWebDriver SetChromeDriver()
        {
            try
            {
                return new ChromeDriver();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IWebDriver SetInternetExplorerDriver()
        {
            try
            {
                var options = new InternetExplorerOptions();
                options.EnsureCleanSession = true;
                options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                options.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;
                options.PageLoadStrategy = PageLoadStrategy.Normal;

                return new InternetExplorerDriver(options);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void GoToUrl(string url)
        {
            try
            {
                WebDriver.Navigate().GoToUrl(url);
                WebDriver.Manage().Window.Maximize();
                WebDriver.SwitchTo().ActiveElement();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IWebElement FindElement(By by, int interval=500, int timeout=15000)
        {
            IWebElement webElement = null;
            var tick = 0;
            try
            {
                do
                {
                    try
                    {
                        webElement = WebDriver.FindElement(by);
                    }
                    catch
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }
                } while (webElement == null && tick < timeout);

                if(webElement == null)
                {
                    throw new TimeoutException(
                    string.Format("Element(s) were not found within {0} seconds", (timeout / 1000).ToString()));
                }
                return webElement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IWebElement> FindElements(By by, int interval = 500, int timeout = 15000)
        {
            var elements = new List<IWebElement>();
            var tick = 0;
            try
            {
                do
                {
                    try
                    {
                        elements = WebDriver.FindElements(by).ToList();
                        if (elements.Count == 0)
                        {
                            Thread.Sleep(interval);
                            tick += interval;
                        }
                    }
                    catch
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }
                } while (elements.Count == 0 && tick < timeout);
                return elements;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IWebElement GetDisplayedElement(By by, int interval = 500, int timeout = 15000)
        {
            try
            {
                var elements = FindElements(by, interval, timeout);

                foreach(IWebElement element in elements)
                {
                    if (element.Displayed)
                    {
                        return element;
                    }                    
                }
                throw new ElementNotVisibleException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IWebElement> GetDisplayedElements(By by, int interval = 500, int timeout = 15000)
        {
            try
            {
                var elements = FindElements(by, interval, timeout);
                var dispalyedElements = new List<IWebElement>();

                elements.ForEach(element =>
                {
                    if (element.Displayed)
                    {
                        dispalyedElements.Add(element);
                    }
                });

                return dispalyedElements;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IWebElement WaitForDisplayedElement(By by, int interval = 500, int timeout = 15000)
        {
            try
            {
                IWebElement webElement = null;
                var tick = 0;
                do
                {
                    try
                    {
                        webElement = FindElement(by, interval, timeout);
                        if (!webElement.Displayed)
                        {
                            throw new ElementNotVisibleException();
                        }

                        return webElement;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }
                } while (tick < timeout);

                throw new TimeoutException(
                    string.Format("Element(s) were not found within {0} seconds", (timeout / 1000).ToString()));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
