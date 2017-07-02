using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
using Web;

/// <summary>
/// The below classes follow the Slack API structure, see https://api.slack.com/docs/
/// </summary>
namespace Slack
{
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
