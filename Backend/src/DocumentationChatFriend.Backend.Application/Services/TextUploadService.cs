using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Options;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Application.Services;

public class TextUploadService : ITextUploadService
{
    private readonly IEmbeddingAdapter _embeddingAdapter;
    private readonly IVectorRepository _vectorRepository;
    public Task<Result> UpsertAsync(string collectionName, string text, ChunkingOptions chunkingOptions)
    {
        throw new NotImplementedException();
    }
}