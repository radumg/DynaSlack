using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Slack;
using RestSharp;

namespace Slack.Helpers
{
    /// <summary>
    /// Utility functions that are used for validation and other auxiliary tasks, during interactions with the Web
    /// </summary>
    internal static class Web
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

        internal static Boolean ServerReturnedSuccessfulResponse(IRestResponse response)
        {
            if (response.Request.Method == RestSharp.Method.GET)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    return false;
            }
            else
            {
                if ((int)response.StatusCode >= 300)
                    return false;
            }
            return true;
        }
    }

}
