using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Slack
{
    /// <summary>
    /// Class used to construct a Slack web request and send it
    /// </summary>
    public class SlackRequest
    {
        public string url;
        public NameValueCollection data;

        /// <summary>
        /// Utility function to send a POST request to a URL using a "payload" request parameter
        /// </summary>
        /// <param name="url">The string URL to send request to</param>
        /// <param name="jsonPayload">The POST data to send in JSON format</param>
        /// <returns>The server response as a string</returns>
        internal static string POST(string url, string jsonPayload)
        {
            // check URL to post to is valid before moving on
            if (!Web.Helpers.checkURI(new Uri(url)))
                return null;

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
}
