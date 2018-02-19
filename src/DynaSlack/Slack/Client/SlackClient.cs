using RestSharp;
using System;

namespace Slack.Client
{
    /// <summary>
    /// Slack clients represent a single connection to a Slack team, with all associated properties and methods.
    /// </summary>
    public class SlackClient
    {
        #region Properties
        // OAuth properties, read-only so they can only be supplied at creation time
        internal string token { get; private set; }
        public string team { get; set; }

        // Webhooks and Bots are client-specific, so you could have different clients with different settings.
        public Webhook webhook { get; set; }
        public Bot bot { get; set; }

        public readonly string BaseUrl = "https://slack.com/api/";
        public string JsonTokenOverride { get; set; }

        internal RestClient restClient;
        #endregion

        /// <summary>
        /// Slack client constructor
        /// </summary>
        /// <param name="token">Optional OAuth token. If not supplied or invalid, posting will only be available as bots or webhooks.</param>
        /// <param name="webhook">Optional webhook override. If supplied, this will be used instead of OAuth webhook.</param>
        public SlackClient(string token = null)
        {
            if (!CheckToken(token))
                this.token = null;
            else
                this.token = token;
                // TODO : implement Slack's /api/check/ endpoint to test token is actually valid
        }

        /// <summary>
        /// Checks whether an OAuth token is valid
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <returns>True if token is valid and false otherwise</returns>
        Boolean CheckToken(string token)
        {
            if (String.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
                return false;

            // TODO : implement Slack's /api/check/ endpoint to test token is actually valid
            return true;
        }
    }
}
