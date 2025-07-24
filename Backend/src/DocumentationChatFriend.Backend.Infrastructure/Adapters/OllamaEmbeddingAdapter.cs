using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Interfaces.Configs;
using DocumentationChatFriend.Backend.Domain.Models;
using OllamaSharp;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Infrastructure.Adapters;

public class OllamaEmbeddingAdapter : IEmbeddingAdapter
{
    private readonly IOllamaApiClient _client;

    public OllamaEmbeddingAdapter(IOllamaApiClient client, IOllamaClientConfigs configs)
    {
        _client = client;
        _client.SelectedModel = configs.EmbeddingModel;
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
            return new InternalErrorResult(
                "Could not embed text: {text} because the OllamaApiClient could not be reached.\n" +
                $"Exception: {ex.Message}");
        }
        
    }
}