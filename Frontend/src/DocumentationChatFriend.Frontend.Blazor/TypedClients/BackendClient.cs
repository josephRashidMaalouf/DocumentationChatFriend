using System.Net.Http.Json;
using DocumentationChatFriend.Frontend.Blazor.CustomResults;
using DocumentationChatFriend.Frontend.Blazor.Dtos;
using DocumentationChatFriend.Frontend.Blazor.Helpers;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Frontend.Blazor.TypedClients;

public class BackendClient : IBackendClient
{
    private readonly HttpClient _httpClient;
    
    public BackendClient(HttpClient httpClient, IConfiguration conf)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(
            ConfigHelper.MustBeSet(conf["BackendClientUri"], "BackendClientUri")
        );
    }

    public async Task<Result> UploadAsync(string collectionName, string text, int chunkingStyle = 1, int chunkLength = 100, int overlap = 30)
    {
        var dto = new
        {
            CollectionName = collectionName,
            Text = text,
            ChunkingStyle = chunkingStyle
        };

        var response = await _httpClient.PostAsJsonAsync($"api/upload?chunkLength={chunkLength}&overlap={overlap}", dto);

        if (!response.IsSuccessStatusCode)
        {
            return new HttpErrorResult($"HttpErrorResult: {response.StatusCode}, {response.ReasonPhrase}");
        }

        return new SuccessResult<string>("Your text is now being processed. This may take a few minutes. Thank you for your for being patient.");
    }

    public async Task<Result> QueryAsync(string collectionName, string question)
    {
        var dto = new
        {
            Question = question,
            CollectionName = collectionName
        };

        var response = await _httpClient.PostAsJsonAsync("api/ask", dto);

        if (!response.IsSuccessStatusCode)
        {
            return new HttpErrorResult($"HttpErrorResult: {response.StatusCode}, {response.ReasonPhrase}");
        }

        return new SuccessResult<string>(await response.Content.ReadAsStringAsync());
    }

    public async Task<Result> QueryGetUnprocessedFactsAsync(string collectionName, string question, float minScore = (float)0.7, int limit = 3)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> GetCollectionNamesAsync()
    {
        var response = await _httpClient.GetAsync("api/collections");
        if (!response.IsSuccessStatusCode)
        {
            return new HttpErrorResult($"HttpErrorResult: {response.StatusCode}, {response.ReasonPhrase}");

        }

        var collections = await response.Content.ReadFromJsonAsync<List<string>>();

        if (collections is null)
        {
            return new DeserializationErrorResult(
                $"DeserializationErrorResult: could not deserialize response content to type: {nameof(List<string>)}");
        }

        return new SuccessResult<List<string>>(collections);
    }

    public IAsyncEnumerable<StreamAnswerResponse> QueryStreamAnswerAsync(string collectionName, string question)
    {
        throw new NotImplementedException();
    }
}