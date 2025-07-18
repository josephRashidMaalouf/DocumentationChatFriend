using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;

namespace DocumentationChatFriend.Backend.Infrastructure.TypedClients;

public class OllamaClient : IChatAdapter
{
    private readonly HttpClient _httpClient;
    private readonly string _url;

    public OllamaClient(HttpClient httpClient, string url)
    {
        _httpClient = httpClient;
        _url = url;
        _httpClient.BaseAddress = new Uri(_url);
    }

    public Task<GenerationResponse> GenerateAsync(string prompt)
    {
        throw new NotImplementedException();
    }
}

//file record OllamaGenerateRequest(string Model = )