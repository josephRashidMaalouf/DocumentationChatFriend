using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using OllamaSharp;

namespace DocumentationChatFriend.Backend.Infrastructure.Services;

public class NomicEmbeddingAdapter : IEmbeddingAdapter
{
    private readonly IOllamaApiClient _client;
    private const string ModelName = "nomic-embed-text";

    public NomicEmbeddingAdapter(IOllamaApiClient client)
    {
        _client = client;
        _client.SelectedModel = ModelName;
    }


    public async Task<EmbeddedResponse> EmbedTextAsync(string text)
    {
        var result = await _client.EmbedAsync(text);

        return new EmbeddedResponse()
        {
            Embedding = result.Embeddings[0].ToList()
        };
    }
}