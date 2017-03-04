using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
using Web;

/// <summary>
/// Namespace holds classes that are used to represent Slack entities
/// The below classes follow the Slack API structure, see https://api.slack.com/docs/
/// </summary>
namespace Slack
{
    /// <summary>
    /// Class represents a Slack message.
    /// </summary>
    class Message
    {
        public string text { get; set; }
        public Attachment attachment { get; set; }

        /// <summary>
        /// Construct a Slack message given a text and an optional attachment
        /// </summary>
        /// <param name="Text">The text to be sent as a Slack message</param>
        public Message(string Text)
        {
            if (!String.IsNullOrEmpty(Text) && !String.IsNullOrWhiteSpace(Text)) this.text = Text;
        }

        /// <summary>
        /// Serializes the Slack message to JSON
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Posts the message to Slack on behalf of the Slack Client's user. Requires a valid OAuth token in Client.
        /// </summary>
        /// <returns></returns>
        public string Post(Client slackClient)
        {
            string response = null;

            try
            {
                if (slackClient == null) throw new Exception("A valid Slack client needs to be supplied.");

                // TODO : implement message posting factory
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return response;
        }
    }

    /// <summary>
    /// Class represents a Bot object in Slack.
    /// </summary>
    class Bot
    {
        public string bot_user_id { get; set; }
        public string bot_access_token { get; set; }
        public string bot_name { get; set; }
    }

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
        public Webhook(string Url, string Channel="general", string User = "DynaSlack", string EmojiIcon = null, string UrlIcon = null)
        {
            // check URL
            if (!String.IsNullOrEmpty(Url) && Web.Helpers.checkURI(new Uri(Url))) this.url = Url;
            else throw new Exception("Invalid webhook URL");

            // check channel & username
            if (!String.IsNullOrEmpty(Channel)) this.channel = Channel;
            if (!this.channel.StartsWith("#")) this.channel = "#" + this.channel;
            if (User != null) username = User;

            // check emoji & icon URL
            if (!String.IsNullOrEmpty(EmojiIcon)) this.icon_emoji = EmojiIcon;
            if (!String.IsNullOrEmpty(UrlIcon) && Web.Helpers.checkURI(new Uri(UrlIcon))) this.icon_url = UrlIcon;
        }

        /// <summary>
        /// Post a message using webhooks
        /// </summary>
        /// <param name="webhook">Specify a webhook object to use. If nothing is supplied, it defaults to the Client's webhook.</param>
        /// <param name="text">The text to send</param>
        /// <returns>The message JSON payload</returns>
        public string Post(string text)
        {
            // TODO : implement & refactor using webhook posting factory
            // perform checks before encoding objects
            if (String.IsNullOrEmpty(text)) throw new Exception("Message text cannot be empty!");

            // build payload
            SlackPayload payload = new SlackPayload();
            payload.Channel = this.channel;
            payload.Username = this.username;
            payload.Text = text;
            payload.Emoji = this.icon_emoji;
            payload.Icon = this.icon_url;

            // encode payload as JSON and POST it
            string jsonPayload = JsonConvert.SerializeObject(payload);
            string response = Web.Request.POST(this.url, jsonPayload);

            // validate response
            if (String.IsNullOrEmpty(response)) throw new Exception("Slack servers returned an error.");
            if (response.Trim().Contains("<html")) throw new Exception("Slack could not process the request : please check the webhook URL.");

            // POST the message and return the server response
            return response;

            // TODO : deserialise server response to the SlackResponse class instead of returning string
            // var serialiser = new Newtonsoft.Json.JsonSerializer();
        }

    }

    /// <summary>
    /// Represents attachments in Slack messages, which are used to send richly formatted messages.
    /// </summary>
    class Attachment
    {
        string fallback { get; set; }
        string color { get; set; }
        string pretext { get; set; }
        string author_name { get; set; }
        string author_link { get; set; }
        string author_icon { get; set; }
        string title { get; set; }
        string title_link { get; set; }
        string text { get; set; }
        string image_url { get; set; }
        string thumb_url { get; set; }
        string footer { get; set; }
        string footer_icon { get; set; }
        DateTime ts { get; set; }
        List<Field> fields { get; set; }

        /// <summary>
        /// Create a Slack message attachment, which are used to send richly formatted messages.
        /// </summary>
        public Attachment(string title=null, string title_link=null, string text=null, string color = null, string image_url = null)
        {
            if (title != null) this.title = title;
            if (title_link != null) this.title_link = title_link;
            if (text != null) this.text = text;
            if (color != null) this.color = color;
            if (image_url != null) this.image_url = image_url;
        }
    }

    /// <summary>
    /// Represents individual fields which form attachments in Slack messages.
    /// </summary>
    class Field
    {
        public string title { get; set; }
        public string value { get; set; }
        public bool @short { get; set; }
    }

    /// <summary>
    /// This class serializes to JSON the payload required by Slack Incoming WebHooks
    /// </summary>
    class SlackPayload
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
