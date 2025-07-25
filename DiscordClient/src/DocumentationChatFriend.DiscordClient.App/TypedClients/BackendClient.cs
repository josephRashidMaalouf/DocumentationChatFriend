using System.Net;
using System.Net.Http.Json;
using DocumentationChatFriend.DiscordClient.App.CustomResults;
using DocumentationChatFriend.DiscordClient.App.Helpers;
using Microsoft.Extensions.Configuration;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.TypedClients;

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

        var response = await _httpClient.PostAsJsonAsync($"api/upload?chunkLength={overlap}&overlap={overlap}", dto);

        if (!response.IsSuccessStatusCode)
        {
            return new HttpErrorResult($"HttpErrorResult: {response.StatusCode}, {response.ReasonPhrase}");
        }

        return new SuccessResult<string>("Your text was successfully uploaded.");
    }

    public async Task<Result> QueryAsync(string collectionName, string question)
    {
        var dto = new
        {
            Question = question,
            CollectionName = collectionName
        };

        var response = await _httpClient.PostAsJsonAsync("api/completions", dto);

        if (!response.IsSuccessStatusCode)
        {
            return new HttpErrorResult($"HttpErrorResult: {response.StatusCode}, {response.ReasonPhrase}");
        }

        return new SuccessResult<string>(await response.Content.ReadAsStringAsync());
    }
}