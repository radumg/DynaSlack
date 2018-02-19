using System;
using System.Collections.Generic;

/// <summary>
/// The below classes follow the Slack API structure, see https://api.slack.com/docs/
/// </summary>
namespace Slack
{
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
}
