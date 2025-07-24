using System.Net;
using System.Net.Http.Json;
using DocumentationChatFriend.DiscordClient.App.CustomErrors;
using DocumentationChatFriend.DiscordClient.App.Helpers;
using Microsoft.Extensions.Configuration;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.TypedClients;

public class BackendClient
{
    private readonly HttpClient _httpClient;
    
    public BackendClient(HttpClient httpClient, IConfiguration conf)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(
            ConfigHelper.MustBeSet(conf["BackendClientUri"], "BackendClientUri")
        );
    }

    public async Task<Result> UploadAsync(string collectionName, string text)
    {
        var dto = new
        {
            CollectionName = collectionName,
            Text = text,
            ChunkingStyle = 0
        };

        var response = await _httpClient.PostAsJsonAsync("api/upload", dto);

        if (!response.IsSuccessStatusCode)
        {
            return new HttpErrorResult($"HttpErrorResult: {response.StatusCode}, {response.ReasonPhrase}");
        }

        return new SuccessResult();
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