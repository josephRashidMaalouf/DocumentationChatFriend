using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;

namespace DocumentationChatFriend.Backend.Infrastructure.TypedClients;

public class OllamaClient : IChatAdapter
{
    private readonly HttpClient _httpClient;
    private readonly string _url;

    public OllamaClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<GenerationResponse> GenerateAsync(string prompt)
    {
        throw new NotImplementedException();
    }
}