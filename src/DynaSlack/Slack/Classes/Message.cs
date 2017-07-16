using System;
using Newtonsoft.Json;
using Slack.Client;

/// <summary>
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
        public string Post(SlackClient slackClient)
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
}
