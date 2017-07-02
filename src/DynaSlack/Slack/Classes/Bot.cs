using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// The below classes follow the Slack API structure, see https://api.slack.com/docs/
/// </summary>
namespace Slack
{
    /// <summary>
    /// Class represents a Bot object in Slack.
    /// </summary>
    public class Bot
    {
        public string bot_user_id { get; set; }
        public string bot_access_token { get; set; }
        public string bot_name { get; set; }
    }

}
