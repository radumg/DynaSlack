using System;

namespace DynaSlack.Slack
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
        public IncomingWebhook webhook { get; set; }
        Bot bot { get; set; }

        /// <summary>
        /// Slack client constructor
        /// </summary>
        /// <param name="token">Optional OAuth token. If not supplied or invalid, posting will only be available as bots or webhooks.</param>
        public Client(string token = null)
        {
            if (!CheckToken(token)) this.token = null;
            else
            {
                this.token = token;
                // TODO : implement Slack's /api/check/ endpoint to test token is actually valid
            }
        }

        /// <summary>
        /// Post a message using the client webhook
        /// </summary>
        /// <param name="text">The text to send</param>
        /// <param name="emoji">The emoji to use as image. Uses Slack syntax, example :ghost: </param>
        /// <returns>The message JSON payload</returns>
        public string PostWebhookMessage(string text, string emoji = null)
        {
            return this.webhook.Post(text, emoji);
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
