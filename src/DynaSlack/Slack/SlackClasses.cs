﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
using DynaSlack.Web;

/// <summary>
/// Namespace holds classes that are used to represent Slack entities
/// The below classes follow the Slack API structure, see https://api.slack.com/docs/
/// </summary>
namespace DynaSlack.Slack
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
    public class IncomingWebhook
    {
        public string url { get; set; }
        public string channel { get; set; }
        public string username { get; set; }
        public string channel_id { get; set; }
        public string configuration_url { get; set; }

        /// <summary>
        /// Webhook class constructor
        /// </summary>
        /// <param name="Url">The URL to send data to</param>
        /// <param name="Channel">The Slack channel to post to</param>
        /// <param name="User">The name of the user to post as</param>
        public IncomingWebhook(string Url, string Channel, string User = null)
        {
            this.url = Url;
            this.channel = Channel;
            if (!this.channel.StartsWith("#")) this.channel = "#" + this.channel;
            if (User != null) username = User;
        }

        /// <summary>
        /// Post a message using webhooks
        /// </summary>
        /// <param name="webhook">Specify a webhook object to use. If nothing is supplied, it defaults to the Client's webhook.</param>
        /// <param name="text">The text to send</param>
        /// <param name="emoji">The emoji to use as image. Uses Slack syntax, example :ghost: </param>
        /// <param name="icon">URL to an image to use as image. If an emoji is specified, this is disregarded.</param>
        /// <returns>The message JSON payload</returns>
        public string Post(string text, string emoji = null, string icon = null)
        {
            // TODO : implement & refactor using webhook posting factory
            // perform checks before encoding objects
            if (text == null || text == String.Empty) return null;
            if (!emoji.StartsWith(":") && emoji != null) emoji = ":" + emoji;
            if (!emoji.EndsWith(":") && emoji != null) emoji = emoji + ":";

            // build payload
            SlackPayload payload = new SlackPayload();
            payload.Channel = this.channel;
            payload.Username = this.username;
            payload.Text = text;
            if (emoji != null) payload.Emoji = emoji;
            else if (icon != null && DynaSlack.Web.Helpers.checkURI(new Uri(icon))) payload.Icon = icon;

            // encode payload as JSON
            string jsonPayload = JsonConvert.SerializeObject(payload);

            // POST the message and return the server response
            return DynaSlack.Web.Request.POST(this.url, jsonPayload);

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