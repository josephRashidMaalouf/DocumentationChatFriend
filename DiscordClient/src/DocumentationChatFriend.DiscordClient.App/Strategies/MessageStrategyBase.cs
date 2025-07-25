using DocumentationChatFriend.DiscordClient.App.TypedClients;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.Strategies;

public abstract class MessageStrategyBase : IMessageStrategy
{
    protected readonly IBackendClient _client;

    protected MessageStrategyBase(IBackendClient client)
    {
        _client = client;
    }

    public abstract Task<Result> Execute(string collectionName, string text);
}