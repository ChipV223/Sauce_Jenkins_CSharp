﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Jenkins_NUnit3
{
    //[TestFixture("chrome", "45", "Windows 7", "", "")]
    //[TestFixture("internet explorer", "10", "Windows 7", "", "")]
    //[TestFixture("firefox", "40", "Windows 8.1", "", "")]
    //[TestFixture("safari", "9.0", "OS X 10.11", "", "")]
    //[TestFixture("iPhone", "9.0", "OS X 10.10", "iPhone Simulator", "portrait")]
    //[TestFixture("Android", "5.1", "Linux", "Android Emulator", "landscape")]
    //[TestFixture("Android", "4.4", "Linux", "Android Emulator", "portrait")]

    //[Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class SeleniumTest
    {
        private IWebDriver driver;
        //private String browser;
        //private String version;
        //private String os;
        //private String deviceName;
        //private String deviceOrientation;

        /*public SeleniumTest(String browser, String version, String os, String deviceName, String deviceOrientation)
        {
            this.browser = browser;
            this.version = version;
            this.os = os;
            this.deviceName = deviceName;
            this.deviceOrientation = deviceOrientation;
        }*/

        [SetUp]
        public void Init()
        {
            DesiredCapabilities caps = new DesiredCapabilities();
            //caps.SetCapability(CapabilityType.BrowserName, browser);
            //caps.SetCapability(CapabilityType.Version, version);
            //caps.SetCapability(CapabilityType.Platform, os);
            //caps.SetCapability("deviceName", deviceName);
            //caps.SetCapability("deviceOrientation", deviceOrientation);
            caps.SetCapability(CapabilityType.BrowserName, System.Environment.GetEnvironmentVariable("SELENIUM_BROWSER"));
            caps.SetCapability(CapabilityType.Version, System.Environment.GetEnvironmentVariable("SELENIUM_VERSION"));
            caps.SetCapability(CapabilityType.Platform, System.Environment.GetEnvironmentVariable("SELENIUM_PLATFORM"));
            caps.SetCapability("deviceName", System.Environment.GetEnvironmentVariable("SELENIUM_DEVICE"));
            caps.SetCapability("username", Constants.SAUCE_LABS_ACCOUNT_NAME);
            caps.SetCapability("accessKey", Constants.SAUCE_LABS_ACCOUNT_KEY);
            caps.SetCapability("name", TestContext.CurrentContext.Test.Name);
            driver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), caps, TimeSpan.FromSeconds(840));

        }

        [Test]
        public void googleTest()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            StringAssert.Contains("Google", driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("Sauce Labs");
            query.Submit();
        }


        [TearDown]
        public void CleanUp()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            try
            {
                // Logs the result to Sauce Labs
                ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                // Terminates the remote webdriver session
                driver.Quit();
            }
        }
    }
}
