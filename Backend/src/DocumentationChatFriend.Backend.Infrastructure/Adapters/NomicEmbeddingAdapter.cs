using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using OllamaSharp;
using ResultPatternJoeget.Errors;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Infrastructure.Adapters;

public class NomicEmbeddingAdapter : IEmbeddingAdapter
{
    private readonly IOllamaApiClient _client;
    private const string ModelName = "nomic-embed-text";

    public NomicEmbeddingAdapter(IOllamaApiClient client)
    {
        _client = client;
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
            return new ErrorResult(
                new ThirdPartyError($"Could not embed text: {text} because the OllamaApiClient could not be reached"));
        }
        
    }
}