using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Infrastructure.Adapters;

public class NomicEmbeddingAdapter : IEmbeddingAdapter
{
    private readonly IOllamaApiClient _client;
    private const string ModelName = "nomic-embed-text";
    private readonly ILogger<NomicEmbeddingAdapter> _logger;

    public NomicEmbeddingAdapter(IOllamaApiClient client, ILogger<NomicEmbeddingAdapter> logger)
    {
        _client = client;
        _logger = logger;
        _client.SelectedModel = ModelName;
    }


    public async Task<Result> EmbedTextAsync(string text)
    {
        try
        {
            var result = await _client.EmbedAsync(text);
            return new SuccessResult<EmbeddedResponse>(new EmbeddedResponse
            {
                Embedding = result.Embeddings[0].ToList()
            });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            return new InternalErrorResult(
                "Could not embed text: {text} because the OllamaApiClient could not be reached");
        }
        
    }
}