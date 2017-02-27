using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using System.Windows.Forms;
using Slack;

namespace Slack
{
    //A simple C# class to post messages to a web API service using POST and JSON data
    //Note: This class uses the Newtonsoft Json.NET serializer available via NuGet
    public class SlackClient
    {
        // Webhooks and Bots are client-specific, so you could have different clients with different settings.
        public IncomingWebhook webhook;
        public Bot bot;

        // CONSTRUCTOR : take in a webhook and build a new client.
        public SlackClient(IncomingWebhook webhookObject = null)
        {
            // if no webhook is supplied, default to bimmanagers.slack.com, on apitestground channel
            if (webhookObject == null)
            {
                webhook = new IncomingWebhook(
                    "https://hooks.slack.com/services/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",
                    "apitestground");
            }
            else
            {
                webhook = webhookObject;
            }
        }

        /// <summary>
        /// Post a message using webhooks
        /// </summary>
        /// <param name="webhook">Specify a webhook to use. If nothing is supplied, it defaults to the Client's webhook.</param>
        /// <param name="text">The text to send</param>
        /// <param name="emoji">The emoji to use as image. Uses Slack syntax, example :ghost: </param>
        /// <returns>The message JSON payload</returns>
        public string PostWebhookMessage(string text, string emoji = null)
        {
            return this.webhook.PostWebhookMessage(text, emoji);
        }
    }
}
