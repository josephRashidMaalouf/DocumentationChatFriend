using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Application.Services;

public class TextUploadService : ITextUploadService
{
    private readonly IEmbeddingAdapter _embeddingAdapter;
    private readonly IVectorRepository _vectorRepository;

    public TextUploadService(IEmbeddingAdapter embeddingAdapter, IVectorRepository vectorRepository)
    {
        _embeddingAdapter = embeddingAdapter;
        _vectorRepository = vectorRepository;
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
                return embeddingResult;
            }

            embeddedChunks.Add(new EmbeddedChunkModel(chunk, embedSuccess.Data.Embedding));
        }

        return await _vectorRepository.UpsertAsync(collectionName, embeddedChunks);
    }
}