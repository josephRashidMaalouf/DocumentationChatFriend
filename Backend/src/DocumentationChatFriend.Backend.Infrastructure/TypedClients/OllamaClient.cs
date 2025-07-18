using System.Net.Http.Json;
using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;

namespace DocumentationChatFriend.Backend.Infrastructure.TypedClients;

public class OllamaClient : IChatAdapter
{
    private readonly HttpClient _httpClient;
    IOllamaClientConfigs _config;

    public OllamaClient(HttpClient httpClient, IOllamaClientConfigs config)
    {
        _httpClient = httpClient;
        _config = config;
        _httpClient.BaseAddress = config.Uri;
    }

    public async Task<GenerationResponse?> GenerateAsync(string prompt)
    {
        var req = new OllamaGenerateRequest(
            _config.Model,
            prompt,
            _config.MaxTokens,
            _config.Temperature);

        var result = await _httpClient.PostAsJsonAsync("generate", req);

        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

        return await result.Content.ReadFromJsonAsync<GenerationResponse>();
    }
}

file record OllamaGenerateRequest(
    string Model, 
    string Prompt, 
    int MaxTokens = 512, 
    double Temperature = 0.6, 
    bool Stream = false);