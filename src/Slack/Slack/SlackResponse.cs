using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;
using Slack;

namespace Slack {
    class SlackResponse {
        // shared response fields
        public bool ok { get; set; }
        public string error { get; set; }
        public string warning { get; set; }
        public List<string> args { get; set; }

        // tokens
        public string access_token { get; set; }
        public string scope { get; set; }

        // bots
        public Bot bot;

        // incoming webhook
        public IncomingWebhook incoming_webhook;

        // user info
        public string url { get; set; }
        public string team { get; set; }
        public string team_id { get; set; }
        public string user { get; set; }
        public string user_id { get; set; }

        }

    }
