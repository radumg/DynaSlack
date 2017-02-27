using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slack;

namespace DebugApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            // build a new Webhook object
            var webhook = new IncomingWebhook(
                "https://hooks.slack.com/services/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 ",
                "apitestground");

            // build a new Slack client object
            var slackClient = new Slack.SlackClient(webhook);

            // read a line from the console
            Console.WriteLine("Please enter a line of text to send to Slack : " + Environment.NewLine);
            var message = Console.ReadLine();

            // POST the message to Slack
            var response = slackClient.PostWebhookMessage(message, ":ghost:");

            // show output
            Console.WriteLine(response);
            Console.WriteLine("Press any key to exit now...");

            // wait before closing the window
            Console.ReadKey();
        }
    }
}
