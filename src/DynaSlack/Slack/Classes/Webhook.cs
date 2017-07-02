using System;
using Newtonsoft.Json;
using Slack.Client;

/// <summary>
/// The below classes follow the Slack API structure, see https://api.slack.com/docs/
/// </summary>
namespace Slack
{
    /// <summary>
    /// Class represents an Incoming Webhook connection in Slack
    /// </summary>
    public class Webhook
    {
        public string url { get; set; }
        public string channel { get; set; }
        public string username { get; set; }
        public string icon_emoji { get; set; }
        public string icon_url { get; set; }
        public string channel_id { get; set; }
        public string configuration_url { get; set; }

        /// <summary>
        /// Create a webhook for use with Slack.
        /// </summary>
        /// <param name="Url">The URL to send data to</param>
        /// <param name="Channel">The Slack channel to post to, defaults to #general</param>
        /// <param name="User">The name of the user to post as</param>
        /// <param name="EmojiIcon">The emoji to use as icon. Uses Slack syntax, like :ghost: or :rocket:</param>
        /// <param name="UrlIcon">An URL to an image to use as the icon.</param>
        public Webhook(string Url, string Channel = "general", string User = "DynaSlack", string EmojiIcon = null, string UrlIcon = null)
        {
            // check URL
            if (!String.IsNullOrEmpty(Url) && Web.Helpers.checkURI(new Uri(Url)))
                this.url = Url;
            else
                throw new Exception("Invalid webhook URL");

            // check channel & username
            if (!String.IsNullOrEmpty(Channel))
                this.channel = Channel;
            if (!this.channel.StartsWith("#"))
                this.channel = "#" + this.channel;
            if (User != null)
                username = User;

            // check emoji & icon URL
            if (!String.IsNullOrEmpty(EmojiIcon))
                this.icon_emoji = EmojiIcon;
            if (!String.IsNullOrEmpty(UrlIcon) && Web.Helpers.checkURI(new Uri(UrlIcon)))
                this.icon_url = UrlIcon;
        }

        /// <summary>
        /// Post a message using webhooks
        /// </summary>
        /// <param name="client">The Slack client to use when posting the webhook message.</param>
        /// <param name="text">The text to send</param>
        /// <returns>The message JSON payload</returns>
        public string Post(SlackClient client, string text)
        {
            if (String.IsNullOrEmpty(text))
                throw new Exception("Message text cannot be empty!");

            // build payload
            SlackPayload payload = new SlackPayload();
            payload.Channel = this.channel;
            payload.Username = this.username;
            payload.Text = text;
            payload.Emoji = this.icon_emoji;
            payload.Icon = this.icon_url;

            // encode payload as JSON and POST it
            string jsonPayload = JsonConvert.SerializeObject(payload);
            string response = SlackRequest.POST(this.url, jsonPayload);

            // validate response
            if (String.IsNullOrEmpty(response))
                throw new Exception("Slack servers returned an error.");
            if (response.Trim().Contains("<html"))
                throw new Exception("Slack could not process the request : please check the webhook URL.");

            // POST the message and return the server response
            return response;

            // TODO : deserialise server response to the SlackResponse class instead of returning string
            // var serialiser = new Newtonsoft.Json.JsonSerializer();
        }

    }
}
