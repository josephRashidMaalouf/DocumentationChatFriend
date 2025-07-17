using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace DocumentationChatFriend.Backend.Infrastructure.Persistance.Qdrant;

public class QDrantRepository : IVectorRepository
{
    private readonly QdrantClient _client;

    public QDrantRepository(QdrantClient client)
    {
        _client = client;
    }
    
    //TODO: Implement error handling with custom result types
    public async Task UpsertAsync(string collectionName, List<EmbeddedChunkModel> embeddedChunks)
    {
        bool collectionExists = await _client.CollectionExistsAsync(collectionName);

        if (!collectionExists)
        {
            await _client.CreateCollectionAsync(
                collectionName: collectionName,
                vectorsConfig: new VectorParams
                {
                    Size = 768,
                    Distance = Distance.Cosine
                });
        }

        var points = embeddedChunks
            .Select(item =>
            {
                string id = Guid.NewGuid().ToString();

                return new PointStruct
                {
                    Id = new PointId { Uuid = id },
                    Vectors = item.Vector.ToArray(),
                    Payload =
                    {
                        ["text"] = item.Text
                    }
                };
            }).ToArray();

        await _client.UpsertAsync(
            collectionName: collectionName,
            points: points);

    }

    public async Task<List<string>> QueryAsync(string collectionName, float[] vector, ulong limit = 3, float minScore = (float)0.6)
    {
        var result = await _client.QueryAsync(
            collectionName: collectionName,
            query: vector,
            limit: limit);

        List<string> extractedPayload = result
            .Where(x => x.Score >= minScore)
            .Select(x => x.Payload["text"].StringValue)
            .ToList();

        return extractedPayload;
    }
}