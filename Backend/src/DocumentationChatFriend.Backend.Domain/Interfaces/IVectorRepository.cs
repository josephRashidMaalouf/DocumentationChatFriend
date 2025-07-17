using DocumentationChatFriend.Backend.Domain.Models;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IVectorRepository
{
    Task UpsertAsync(string collectionName, List<EmbeddedChunkModel> embeddedChunks);
    Task<List<string>> QueryAsync(string collectionName, float[] vector, ulong limit = 3, float minScore = (float)0.6);
}