using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// The below classes follow the Slack API structure, see https://api.slack.com/docs/
/// </summary>
namespace Slack
{
    /// <summary>
    /// Represents individual fields which form attachments in Slack messages.
    /// </summary>
    class Field
    {
        public string title { get; set; }
        public string value { get; set; }
        public bool @short { get; set; }
    }
}
