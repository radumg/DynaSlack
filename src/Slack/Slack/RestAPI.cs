using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;



namespace aLL_Design.Slack
{
    class RestAPI
    {
        /// <summary>Call a REST API </summary>
        /// <returns>Returns true if successful, false otherwise</returns>
        public static bool SyncPackages(string userID)
        {
            var endpoint = "user/" + userID.Trim();
            try
            {
                var client = new RestClient("http://dynamopackages.com/");
                var request = new RestRequest(endpoint, Method.GET);

                // execute the request
                IRestResponse response = client.Execute(request);
                var content = response.Content; // raw content as string
                var headers = response.Headers.ToString();

                // check API response code
                if (response.StatusCode.ToString() != "OK")
                    throw new Exception("Invalid API response, error code : " + response.StatusCode.ToString());

                // Deserialize
                try
                {
                    dynamic des = JsonConvert.DeserializeObject(content);
                    if (des.success != true) throw new Exception("Failed to retrieve package information from API.");
                }
                catch (Exception)
                {
                    throw new Exception("Could not deserialise response from API");
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + " || source:"+ex.Source + "|| stack trace:"+ex.StackTrace);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

    }
}
