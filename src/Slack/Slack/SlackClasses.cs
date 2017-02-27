using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slack
{
    // The below classes follow the Slack API structure
    // see https://api.slack.com/docs/

    public class Bot
    {
        public string bot_user_id { get; set; }
        public string bot_access_token { get; set; }
    }

    public class IncomingWebhook
    {
        public string url { get; set; }
        public string channel { get; set; }
        public string username { get; set; }
        public string channel_id { get; set; }
        public string configuration_url { get; set; }

        public IncomingWebhook(string Url, string Channel, string User= null)
        {
            this.url = Url;
            this.channel = Channel;
            if (!this.channel.StartsWith("#")) this.channel = "#" + this.channel;
            if (User != null) username = User;
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
            // perform checks before encoding objects
            if (text == null || text == String.Empty) return null;
            if (!emoji.StartsWith(":")) emoji = ":" + emoji;
            if (!emoji.EndsWith(":")) emoji = emoji + ":";

            // build payload + encode
            Payload payload = new Payload()
            {
                Channel = this.channel,
                Username = this.username,
                Text = text,
                Emoji = emoji,
            };

            // POST the message and return the server response
            return Slack.Requests.POST(this.url, JsonConvert.SerializeObject(payload));
        }

    }

    public class Attachment
    {
        public string fallback { get; set; }
        public string color { get; set; }
        public string pretext { get; set; }
        public string author_name { get; set; }
        public string author_link { get; set; }
        public string author_icon { get; set; }
        public string title { get; set; }
        public string title_link { get; set; }
        public string text { get; set; }
        public string image_url { get; set; }
        public string thumb_url { get; set; }
        public string footer { get; set; }
        public string footer_icon { get; set; }
        public DateTime ts { get; set; }
        public List<Field> fields { get; set; }
    }

    public class Field
    {
        public string title { get; set; }
        public string value { get; set; }
        public bool @short { get; set; }
    }

    /// <summary>
    /// This class serializes into the Json payload required by Slack Incoming WebHooks
    /// </summary>
    public class Payload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon_emoji")]
        public string Emoji { get; set; }

        [JsonProperty("icon_url")]
        public string Icon { get; set; }
    }
}
