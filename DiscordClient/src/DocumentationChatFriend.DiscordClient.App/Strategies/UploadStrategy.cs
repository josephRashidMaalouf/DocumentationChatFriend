using DocumentationChatFriend.DiscordClient.App.Models;
using DocumentationChatFriend.DiscordClient.App.TypedClients;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.Strategies;

public class UploadStrategy : MessageStrategyBase
{
    public UploadStrategy(IBackendClient client) : base(client)
    {

    }

    public override async Task<Result> Execute(string collectionName, string text)
    {
        var trimmedText = text.Replace("!upload", "").Trim();


        

        return await _client.UploadAsync(collectionName, trimmedText);

    }

    public UploadOptions? ExtractOptions(string text)
    {
        var optionals = text.Split("|||")[0].Split(':');

        if (!optionals.Any())
        {
            return null;
        }

        UploadOptions options = new UploadOptions();
        for (int i = 0; i < optionals.Length || (i > 2) == false; i++)
        {
            bool parsable = int.TryParse(optionals[i], out int value);

            if (!parsable)
            {
                continue;
            }

            if (i == 0)
            {
                if (value > 2)
                {
                    value = 0;
                }
                options.ChunkingStyle = Math.Abs(value);
            }

            if (i == 1)
            {
                options.ChunkLength = Math.Abs(value);
            }

            if (i == 2)
            {
                options.Overlap = Math.Abs(value);
            }
        }
        return options;
    }

}