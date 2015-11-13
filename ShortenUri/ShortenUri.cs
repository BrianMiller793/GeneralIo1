// <copyright file="ShortenUri.cs" company="None">
//     File copyright 2015, Brian C. Miller
// </copyright>

namespace ShortenUri
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Provides functionality to use the Google URL shortener service.
    /// </summary>
    public class ShortenUri
    {
        /// <summary> REST endpoint URI for Google service. </summary>
        private static string googleService = "https://www.googleapis.com/urlshortener/v1/url?key={0}";

        /// <summary> JSON body for the shortener service. </summary>
        private static string jsonBody = "{{'longUrl': '{0}'}}";

        /// <summary>
        /// Get a short URI, using the Google service.
        /// </summary>
        /// <param name="longUri">Long URI to be shortened.</param>
        /// <param name="apiKey">API key used to access the service.</param>
        /// <returns>Shortened version of URI.</returns>
        public static string GetShortUri(string longUri, string apiKey)
        {
            if (string.IsNullOrEmpty(longUri))
            {
                throw new ArgumentNullException("uri");
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(googleService, apiKey));
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] encodedBytes;

            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            encodedBytes = encoding.GetBytes(string.Format(jsonBody, longUri));
            request.ContentLength = encodedBytes.Length;

            // Send the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(encodedBytes, 0, encodedBytes.Length);
            }

            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GoogleShortenerResponse));
            Stream responseStream = webResponse.GetResponseStream();
            GoogleShortenerResponse shortenerResponse = (GoogleShortenerResponse)serializer.ReadObject(responseStream);
            return shortenerResponse.id;
        }

        /// <summary>
        /// Response data for Google Shortener Response.
        /// </summary>
        [DataContract]
        internal class GoogleShortenerResponse
        {
            /// <summary>Kind (type) of API response.</summary>
            [DataMember]
            internal string kind;

            /// <summary>The shortened version of the long URL.</summary>
            [DataMember]
            internal string id;

            /// <summary>The original long URL.</summary>
            [DataMember]
            internal string longUrl;
        }
    }
}
