using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Slack.Webhooks.Elements;
using Slack.Webhooks;
using System.Text.Json;
using Slack.Webhooks.Blocks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SNS2Slack;

public class Function
{
    private readonly SlackClient _slackClient;

    public Function()
    {
        var webHook = Environment.GetEnvironmentVariable("SlackWebHook");

        _slackClient = new SlackClient(webHook);
    }

    public async Task FunctionHandler(SNSEvent evnt, ILambdaContext context)
    {
        foreach (var record in evnt.Records)
        {
            await ProcessRecord(record, context);
        }
    }

    private async Task ProcessRecord(SNSEvent.SNSRecord record, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processed record {record.Sns.Message}");

        if (record.Sns.Message == null)
        {
            return;
        }

        var alarm = JsonSerializer.Deserialize<Alarm>(record.Sns.Message);

        if (string.IsNullOrEmpty(alarm?.AlarmName))
        {
            return;
        }

        var slackMessage = new SlackMessage
        {
            Markdown = true
        };

        slackMessage.Blocks = new List<Block>
        {
            new Section
            {
                Text = new TextObject($"{alarm.AlarmDescription}\n")
                {
                    Type = TextObject.TextType.Markdown
                }
            }
        };

        await _slackClient.PostAsync(slackMessage);
    }
}
