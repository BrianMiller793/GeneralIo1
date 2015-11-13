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
        static string apiKey = "";

        /// <summary>
        /// This unit test tests the basic happy path for the GetShortUri method.
        /// </summary>
        [Fact]
        public void Test1()
        {
            string testUri = "http://www.google.com/";
            string shortUri = ShortenUri.GetShortUri(testUri, apiKey);
            Assert.True(string.IsNullOrEmpty(shortUri) == false, "shortUri");
            Assert.Equal(testUri, this.GetResponseUri(shortUri));
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
