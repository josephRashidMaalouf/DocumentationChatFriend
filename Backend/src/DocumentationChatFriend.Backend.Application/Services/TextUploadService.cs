using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using Microsoft.Extensions.Logging;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Application.Services;

public class TextUploadService : ITextUploadService
{
    private readonly IEmbeddingAdapter _embeddingAdapter;
    private readonly IVectorRepository _vectorRepository;
    private readonly ILogger<TextUploadService> _logger;

    public TextUploadService(IEmbeddingAdapter embeddingAdapter, IVectorRepository vectorRepository, ILogger<TextUploadService> logger)
    {
        _embeddingAdapter = embeddingAdapter;
        _vectorRepository = vectorRepository;
        _logger = logger;
    }

    public async Task<Result> UpsertAsync(string collectionName, string text, IChunkService chunkService)
    {
        var chunkedText = chunkService.Chunk(text);

        List<EmbeddedChunkModel> embeddedChunks = [];
        foreach (var chunk in chunkedText)
        {
            var embeddingResult = await _embeddingAdapter.EmbedTextAsync(chunk);
            if (embeddingResult is not SuccessResult<EmbeddedResponse> embedSuccess)
            {
                var error = (embeddingResult as ErrorResult)?.Reason;
                _logger.LogError("Embedding error: {Error}", error);
                return embeddingResult;
            }

            embeddedChunks.Add(new EmbeddedChunkModel(chunk, embedSuccess.Data.Embedding));
        }

        var upsertResult = await _vectorRepository.UpsertAsync(collectionName, embeddedChunks);
        if (upsertResult is ErrorResult upsertError)
        {
            _logger.LogError("Upsert error: {Error}", upsertError.Reason);

        }

        return upsertResult;
    }
}