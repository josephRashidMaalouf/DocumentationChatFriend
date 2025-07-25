using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.TypedClients;

public interface IBackendClient
{
    Task<Result> UploadAsync(string collectionName, string text, int chunkingStyle = 1,
        int chunkLength = 100, int overlap = 30);

    Task<Result> QueryAsync(string collectionName, string question);
}