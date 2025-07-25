using DocumentationChatFriend.DiscordClient.App.TypedClients;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.Strategies;

public class AskStrategy : MessageStrategyBase
{
    public AskStrategy(IBackendClient client) : base(client)
    {
    }

    public override async Task<Result> Execute(string collectionName, string text)
    {
        var trimmedText = text.Replace("!ask", "").Trim();

        return await _client.QueryAsync(collectionName, trimmedText);
    }
}