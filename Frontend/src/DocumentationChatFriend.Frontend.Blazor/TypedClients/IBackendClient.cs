using DocumentationChatFriend.Frontend.Blazor.Dtos;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Frontend.Blazor.TypedClients;

public interface IBackendClient
{
    Task<Result> UploadAsync(
        string collectionName, 
        string text, 
        int chunkingStyle = 1,
        int chunkLength = 100, 
        int overlap = 30);
    Task<Result> QueryAsync(string collectionName, string question);
    Task<Result> QueryGetUnprocessedFactsAsync(string collectionName, string question, float minScore = (float)0.7, int limit = 3);
    Task<Result> GetCollectionNamesAsync();
    IAsyncEnumerable<StreamAnswerResponse> QueryStreamAnswerAsync(string collectionName, string question);
}