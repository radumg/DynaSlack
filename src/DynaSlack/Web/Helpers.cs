using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Slack;

namespace Web
{
    /// <summary>
    /// Utility class used to construct a web request and send it
    /// </summary>
    class Request
    {
        public string url;
        public NameValueCollection data;

        /// <summary>
        /// Utility function to send a POST request to a URL using a "payload" request parameter
        /// </summary>
        /// <param name="url">The string URL to send request to</param>
        /// <param name="jsonPayload">The POST data to send in JSON format</param>
        /// <returns>The server response as a string</returns>
        protected internal static string POST(string url, string jsonPayload)
        {
            // check URL to post to is valid before moving on
            if (!Web.Helpers.checkURI(new Uri(url))) return null;

            // the encoding is standardised and necessary for deserialisation.
            Encoding _encoding = new UTF8Encoding();

            // encapsulate POST attempt in a try/catch statement
            try
            {
                using (WebClient client = new WebClient())
                {
                    NameValueCollection POSTdata = new NameValueCollection();
                    POSTdata["payload"] = jsonPayload;

                    var response = client.UploadValues(url, "POST", POSTdata);

                    // Output the response content as string
                    return _encoding.GetString(response);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Class that Slack's web responses deserialize to
    /// </summary>
    class Response
    {
        // shared response fields
        public bool ok { get; set; }
        public string error { get; set; }
        public string warning { get; set; }
        public List<string> args { get; set; }

        // tokens
        public string access_token { get; set; }
        public string scope { get; set; }

        // bots
        Bot bot;

        // incoming webhook
        public Webhook incoming_webhook;

        // user info
        public string url { get; set; }
        public string team { get; set; }
        public string team_id { get; set; }
        public string user { get; set; }
        public string user_id { get; set; }
    }
    
    /// <summary>
    /// Utility functions that are used for validation and other auxiliary tasks
    /// </summary>
    class Helpers
    {
        /// <summary>Check the URI is valid</summary>
        /// <param name="uriToCheck">The URI to check</param>
        /// <returns>True if is valid, False otherwise</returns>
        public static Boolean checkURI(Uri uriToCheck)
        {
            if (uriToCheck.IsFile || uriToCheck.IsUnc)
            {
                throw new Exception("URI is file or is UNC pointing to internal network");
            }
            if (!Uri.CheckSchemeName(uriToCheck.Scheme))
                return false;

            return true;
        }

    }
}
