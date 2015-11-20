// <copyright file="ShortenUri.cs" company="None">
//     File copyright 2015, Brian C. Miller
// </copyright>

namespace GeneralUi
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Support.UI;
    using Xunit;

    /// <summary>
    /// Shortener tests using Selenium browser access.
    /// </summary>
    public class SeleniumTest
    {
        private string googleUserId = System.Environment.GetEnvironmentVariable("GoogleUserId");
        private string googlePassword = System.Environment.GetEnvironmentVariable("GooglePassword");

        /// <summary>
        /// This unit test tests the basic happy path for using Selenium with the Google URL shortener.
        /// </summary>
        [Theory]
        [InlineData("http://www.google.com/")]
        [InlineData("https://www.google.com/")]
        [InlineData(@"https://en.wiktionary.org/wiki/%E7%93%9C")]   // These two show as Japanese Kanji in the browser.
        [InlineData(@"http://www.docoja.com:8080/kanji/keykanj?dbname=kanjig&keyword=%E3%81%86%E3%82%8A")]
        public void HappyPath(string testUri)
        {
            string shortUri = this.ShortenUrl(testUri);
            Assert.True(string.IsNullOrEmpty(shortUri) == false, "shortUri");
            Assert.Equal(testUri, RestTest.GetResponseUri(shortUri));
        }

        /// <summary>
        /// Shortens a URL using the Google URL shortener, Selenium, and Firefox browser.
        /// </summary>
        /// <param name="longUrl">A URL to be passed to the shortening service.</param>
        /// <returns>A short version of the URL.</returns>
        private string ShortenUrl(string longUrl)
        {
            const string shortenerUi = "https://goo.gl/";
            if (longUrl == null)
            {
                throw new ArgumentNullException("longUrl");
            }

            // Use FireFox to get the page.
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(shortenerUi);
            // <input name="is_logged_in" id="is_logged_in" value="false" type="hidden">
            IWebElement userLoggedIn = driver.FindElement(By.Id("is_logged_in"));
            if (userLoggedIn.GetAttribute("value") == "false")
            {
                this.GoogleSignIn(driver);
            }

            // Get the page elements for the input box and its submit button.
            // <input tabindex="1" id="shortenerInputText" class="IXTYPID-x-a IXTYPID-b-a IXTYPID-d-b IXTYPID-r-b" type="text">
            IWebElement shortenerInputText = driver.FindElement(By.Id("shortenerInputText"));
            // <div aria-disabled="false" role="button" aria-labelledby="gwt-uid-33" id="shortenerSubmitButton" tabindex="1" class="IXTYPID-d-a IXTYPID-w-a IXTYPID-b-a IXTYPID-r-g"><span class="IXTYPID-b-a IXTYPID-w-f"></span> <span class="IXTYPID-b-a IXTYPID-w-h">Shorten URL</span></div>
            IWebElement shortenerSubmitButton = driver.FindElement(By.Id("shortenerSubmitButton"));
            System.Threading.Thread.Sleep(1000);
            shortenerInputText.SendKeys(longUrl);
            shortenerSubmitButton.Click();

            // Wait for the element to be populated
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until((d) => { return d.FindElement(By.ClassName("IXTYPID-p-b")); });

            // <input tabindex="2" class="IXTYPID-p-b" readonly="readonly" value="" type="text">
            IWebElement shortenedUrl = driver.FindElement(By.ClassName("IXTYPID-p-b"));
            string urlValue = shortenedUrl.GetAttribute("value");
            driver.Close();
            return urlValue;
        }

        /// <summary>
        /// Sign into a Google page.
        /// </summary>
        /// <param name="driver">Selenium web driver.</param>
        private void GoogleSignIn(IWebDriver driver)
        {
            // Click on the sign-in link.
            IWebElement signInLink = driver.FindElement(By.Id("gb_70"));
            signInLink.Click();

            // Wait for the email address screen.
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until((d) => { return d.FindElement(By.Id("Email")); });

            // Enter email address, then click Next.
            IWebElement emailInput = driver.FindElement(By.Id("Email"));
            emailInput.SendKeys(googleUserId);
            IWebElement nextButton = driver.FindElement(By.Id("next"));
            nextButton.Click();

            // Wait for the password screen.
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until((d) => { return d.FindElement(By.Id("Passwd")); });

            // Enter the password and click Sign In button.
            IWebElement passwordInput = driver.FindElement(By.Id("Passwd"));
            passwordInput.SendKeys(googlePassword);
            IWebElement signInButton = driver.FindElement(By.Id("signIn"));
            signInButton.Click();
            
            // Wait for sign in to complete.
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until((d) =>
            {
                IWebElement userLoggedIn = driver.FindElement(By.Id("is_logged_in"));
                return userLoggedIn.GetAttribute("value") == "true";
            });
        }
    }
}
