using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IVectorRepository
{
    Task<Result> UpsertAsync(string collectionName, List<EmbeddedChunkModel> embeddedChunks);
    Task<Result> QueryAsync(string collectionName, float[] vector, ulong limit = 3, float minScore = (float)0.6);
}