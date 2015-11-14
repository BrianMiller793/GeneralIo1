// <copyright file="ShortTest.cs" company="None">
//     File copyright 2015, Brian C. Miller
// </copyright>

namespace GeneralUi
{
    using ShortenUri;
    using System;
    using System.Net;
    using Xunit;

    /// <summary>
    /// Contains unit tests for the ShortenUri class.
    /// </summary>
    public class ShortTest
    {
        /// <summary> Active API key to access the Google API. </summary>
        static string apiKey = System.Environment.GetEnvironmentVariable("GoogleShortenerApiKey");

        /// <summary>
        /// This unit test tests the basic happy path for the GetShortUri method.
        /// </summary>
        [Theory]
        [InlineData("http://www.google.com/")]
        [InlineData("https://www.google.com/")]
        [InlineData(@"https://en.wiktionary.org/wiki/%E7%93%9C")]   // These two show as Japanese Kanji in the browser.
        [InlineData(@"http://www.docoja.com:8080/kanji/keykanj?dbname=kanjig&keyword=%E3%81%86%E3%82%8A")]
        public void HappyPath(string testUri)
        {
            string shortUri = ShortenUri.GetShortUri(testUri, apiKey);
            Assert.True(string.IsNullOrEmpty(shortUri) == false, "shortUri");
            Assert.Equal(testUri, this.GetResponseUri(shortUri));
        }

        [Theory]
        [InlineData("1234.123")]
        [InlineData("1234 123")]
        [InlineData("this is not a pipe nor a good uri")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\\")]
        public void BadUrlFormat(string testUri)
        {
            string shortUri = string.Empty;
            Assert.Throws(typeof(System.Net.WebException), () => shortUri = ShortenUri.GetShortUri(testUri, apiKey));
        }

        /// <summary>
        /// Get final URI destination for a given URI with expected redirection.
        /// </summary>
        /// <param name="destination">URI destination.</param>
        /// <returns>Final URI, after redirections.</returns>
        private string GetResponseUri(string destination)
        {
            if (string.IsNullOrEmpty(destination))
            {
                throw new ArgumentNullException("destination");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destination);
            request.Method = "GET";
            request.ContentType = "application/text;charset=ascii";
            request.AllowAutoRedirect = true;
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            return webResponse.ResponseUri.OriginalString;
        }
    }
}
