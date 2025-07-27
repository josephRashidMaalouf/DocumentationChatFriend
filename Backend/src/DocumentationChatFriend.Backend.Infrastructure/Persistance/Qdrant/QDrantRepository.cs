using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Interfaces.Configs;
using DocumentationChatFriend.Backend.Domain.Models;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Infrastructure.Persistance.Qdrant;

public class QDrantRepository : IVectorRepository
{
    private readonly QdrantClient _client;
    private readonly IVectorRepositoryConfigs _configs;

    public QDrantRepository(QdrantClient client, IVectorRepositoryConfigs configs)
    {
        _client = client;
        _configs = configs;
    }

    public async Task<Result> UpsertAsync(string collectionName, List<EmbeddedChunkModel> embeddedChunks)
    {
        try
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

            return new SuccessResult();
        }
        catch (Exception ex)
        {
            return new InternalErrorResult($"Could not perform an upsert to collection: {collectionName}\n" +
                                           $"Exception: {ex.Message}");
        }

    }

    public async Task<Result> QueryAsync(string collectionName, float[] vector)
    {
        try
        {
            var collectionExists = await _client.CollectionExistsAsync(collectionName);
            if (!collectionExists)
            {
                return new NotFoundErrorResult($"No collection with the name {collectionName} exists in the database");
            }


            var result = await _client.QueryAsync(
                collectionName: collectionName,
                query: vector,
                limit: _configs.Limit);

            var extractedPayload = result
                .Where(x => x.Score >= _configs.MinScore)
                .Select(x => x.Payload["text"].StringValue)
                .ToList();

            return new SuccessResult<List<string>>(extractedPayload);
        }
        catch (Exception ex)
        {
            return new InternalErrorResult($"Could not query the collection: {collectionName} in the database\n" +
                                           $"Exception: {ex.Message}");
        }
    }

    public async Task<Result> QueryForScoredFactsAsync(string collectionName, float[] vector, ulong limit, float minScore)
    {
        try
        {
            var collectionExists = await _client.CollectionExistsAsync(collectionName);
            if (!collectionExists)
            {
                return new NotFoundErrorResult($"No collection with the name {collectionName} exists in the database");
            }


            var result = await _client.QueryAsync(
                collectionName: collectionName,
                query: vector,
                limit: limit);

            var extractedPayload = result
                .Where(x => x.Score >= minScore)
                .Select(x => (x.Score, x.Payload["text"].StringValue))
                .ToList();

            return new SuccessResult<List<(float score, string fact)>>(extractedPayload);
        }
        catch (Exception ex)
        {
            return new InternalErrorResult($"Could not query the collection: {collectionName} in the database\n" +
                                           $"Exception: {ex.Message}");
        }
    }
}