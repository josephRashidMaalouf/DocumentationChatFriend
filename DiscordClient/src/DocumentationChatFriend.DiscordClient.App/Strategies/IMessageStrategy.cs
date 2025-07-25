using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.Strategies;

public interface IMessageStrategy
{
    Task<Result> Execute(string collectionName, string text);
}