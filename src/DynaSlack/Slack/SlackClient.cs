using System;

namespace Slack
{
    /// <summary>
    /// Slack clients represent a single connection to a Slack team, with all associated properties and methods.
    /// </summary>
    public class Client
    {
        // OAuth properties, read-only so they can only be supplied at creation time
        private string token { get; }
        public string team { get; set; }

        // Webhooks and Bots are client-specific, so you could have different clients with different settings.
        public Webhook webhook { get; set; }
        Bot bot { get; set; }

        /// <summary>
        /// Slack client constructor
        /// </summary>
        /// <param name="token">Optional OAuth token. If not supplied or invalid, posting will only be available as bots or webhooks.</param>
        /// <param name="webhook">Optional webhook override. If supplied, this will be used instead of OAuth webhook.</param>
        public Client(string token = null, Webhook webhook=null)
        {
            if (!CheckToken(token)) this.token = null;
            else
            {
                this.token = token;
                // TODO : implement Slack's /api/check/ endpoint to test token is actually valid
            }
            // check if there is an override on the webhook
            if (webhook != null) this.webhook = webhook;
        }

        /// <summary>
        /// Post a message using the client webhook
        /// </summary>
        /// <param name="text">The text to send</param>
        /// <returns>The message JSON payload</returns>
        public string PostWebhookMessage(string text, string emoji = null)
        {
            return this.webhook.Post(text);
        }

        /// <summary>
        /// Checks whether an OAuth token is valid
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <returns>True if token is valid and false otherwise</returns>
        Boolean CheckToken(string token)
        {
            if (String.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token)) return false;

            // TODO : implement Slack's /api/check/ endpoint to test token is actually valid
            return true;
        }
    }
}
