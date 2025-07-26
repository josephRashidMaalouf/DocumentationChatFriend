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

        var options = ExtractOptions(trimmedText);

        if (options is not null)
        {
            return await _client.UploadAsync(collectionName, trimmedText, options.ChunkingStyle ?? 1, options.ChunkLength ?? 100, options.Overlap ?? 30);
        }

        return await _client.UploadAsync(collectionName, trimmedText);

    }

    public UploadOptions? ExtractOptions(string text)
    {
        var optionals = text.Split("|||")[0].Split(':');

        if (optionals.Length == 1 && !int.TryParse(optionals[0], out _))
        {
            return null;
        }

        UploadOptions options = new UploadOptions();
        for (int i = 0; i < optionals.Length; i++)
        {
            if (i > 2)
            {
                break;
            }
            bool parsable = int.TryParse(optionals[i], out int value);
            value = Math.Abs(value);

            if (!parsable)
            {
                continue;
            }

            if (i == 0)
            {
                options.ChunkingStyle = value > 2 ? 0 : value; 
            }

            if (i == 1)
            {
                options.ChunkLength = value;
            }

            if (i == 2)
            {
                options.Overlap = value;
            }
        }
        return options;
    }

}