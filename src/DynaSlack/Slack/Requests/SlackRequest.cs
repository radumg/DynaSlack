using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Slack.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Slack
{
    /// <summary>
    /// Class used to construct a Slack web request and send it
    /// </summary>
   /* public class SlackRequest
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
    */

    internal class SlackRequest
    {
        internal TimeSpan timeToComplete { get; private set; }
        internal RestRequest restRequest { get; private set; }
        internal SlackResponse errorResponse;

        /// <summary>
        /// Construct a Slack request from a supplied client, web method and resource targeted.
        /// </summary>
        /// <param name="client">The Slack client, required for authentication.</param>
        /// <param name="method">The method to use. Ex : Method.GET, Method.POST, etc.</param>
        /// <param name="resource">(optional) The URL fragment for the resource targeted. Ex : "tasks/".
        /// Note : does not require leading slash.</param>
        internal SlackRequest(SlackClient client, Method method, string resource = null)
        {
            /// The following sets the correct communication protocol, required as Slack API uses https.
            /// Otherwise, any requests to API fail, irrespective of using RestSharp or System.Web
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            this.restRequest = new RestRequest();
            this.restRequest.AddHeader("Authorization", client.token);
            this.restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");

            /// The following headers are not required by useful.
            /// cache-control encourages system not to cache requests and fetch new results every time
            /// DynaSlack-token is used by the OAuth authentication flow to verify the authentication request came from the DynaSlack library.
            this.restRequest.AddHeader("cache-control", "no-cache");
            this.restRequest.AddHeader("DynaSlack-token", Assembly.GetExecutingAssembly().GetType().GUID.ToString());

            /// The properties below point each request to the appropriate (supplied) endpoint
            /// This abstraction allows re-use of the SlackRequest class for all endpoints and all methods (GET, POST, etc).
            this.restRequest.Resource = resource;
            this.restRequest.Method = method;
        }

        /// <summary>
        /// Executes an Slack Request
        /// </summary>
        /// <typeparam name="T">The Slack object type to deserialize as.</typeparam>
        /// <param name="request">The Slack Request to execute.</param>
        /// <returns>Response from Slack API deserialized as the supplied type.</returns>
        internal T Execute<T>(SlackClient client) where T : new()
        {
            var startTime = DateTime.Now;
            var response = client.restClient.Execute(this.restRequest);
            this.timeToComplete = DateTime.Now - startTime;
            Console.WriteLine("Slack [" + this.restRequest.Method.ToString() + "] request to [" + this.restRequest.Resource + "] took " + timeToComplete.TotalMilliseconds.ToString() + "ms");

            /// For legibility and future-proofing, deserialisation is de-coupled from the execution of the request
            /// Currently, they are daisy-chained but they might be decoupled at a later date if required.
            /// Note the JsonTokenOverride is supplied here, but globally set in the Client class
            return Deserialize<T>(response, client.JsonTokenOverride);
        }

        /// <summary>
        /// Deserializes a response from the Slack API to the supplied object type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="response">The response from Slack API to deserialize.</param>
        /// <param name="JsonToken">Optional : specify a JSON token as the root element to deserialise from.
        /// Default for Slack is : "$.data"</param>
        /// <returns>The supplied response from Slack API, deserialized as the supplied type.</returns>
        internal T Deserialize<T>(IRestResponse response, string JsonToken = null) where T : new()
        {
            /// We first need to check there is something to serialise
            /// If Slack didn't return a success code, we parse the error message instead of deserialising.
            /// Successful web response codes are in the 100 and 200 series. Larger numbers indicate warnings or errors.
            if (Helpers.Web.ServerReturnedSuccessfulResponse(response) == false)
            {
                /// See SlackResponse.cs for the class definitions of the error responses
                this.errorResponse = JsonConvert.DeserializeObject<SlackResponse>(response.Content);

                /// The Slack API is capable of returning multiple errors
                /// We need to parse each one and record its error message
                /// ForEach loop is inefficient here but more legible, will likely change later on
                string errorMessage = "";
                foreach (var error in errorResponse.Errors)
                {
                    if (error != null && !String.IsNullOrEmpty(error.Message))
                        errorMessage += "Slack [Response Error] :" + error.Message + Environment.NewLine;
                }
                Console.WriteLine(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            /// Because Slack wraps all responses in a "data{}" object in JSON,
            /// we need to check if the client has a Json token override.
            /// Specifying this at client level will allow simultaneous usage of different Slack API versions should this change.
            /// This could be achieved with RestSharps's Request.RootElement but i've not had consistent results with that.
            string taskData = "";
            if (String.IsNullOrEmpty(JsonToken) == false)
            {
                JObject o = JObject.Parse(response.Content);
                taskData = o.SelectToken(JsonToken).ToString();
            }
            else
                taskData = response.Content;

            /// We don't want the deserialisation to break if some properties are empty.
            /// So we need to specify the behaviour when such values are encountered.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;

            return JsonConvert.DeserializeObject<T>(taskData, settings);
        }

    }
}
