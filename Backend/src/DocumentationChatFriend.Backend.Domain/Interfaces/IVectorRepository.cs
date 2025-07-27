using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IVectorRepository
{
    /// <summary>
    /// Inserts a new document or updates an existing document in the specified collection.
    /// </summary>
    /// <param name="collectionName">The name of the collection where the document will be upserted. Cannot be null or empty.</param>
    /// <param name="embeddedChunks">A list of <see cref="EmbeddedChunkModel"/> instances representing the document data to be upserted. Cannot be
    /// null.</param>
    /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation. The task result contains a <see
    /// cref="Result"/> indicating the success or failure of the operation.
    /// If successful the result will be of type <see cref="SuccessResult"/>. Else the type will be an <see cref="InternalErrorResult"/>
    /// </returns>
    Task<Result> UpsertAsync(string collectionName, List<EmbeddedChunkModel> embeddedChunks);
    /// <summary>
    /// Asynchronously queries a collection to find the closest matching items based on a vector.
    /// </summary>
    /// <param name="collectionName">The name of the collection to query. Cannot be null or empty.</param>
    /// <param name="vector">The vector used to find matching items. Must not be null and should have a valid length as expected by the
    /// collection.</param>
    /// <returns>A task representing the asynchronous operation, containing a <see cref="Result"/> that indicates success or failure of the operation.
    /// If successful the result will be of type <see cref="SuccessResult{T}"/> where T is a list of strings containing each fact retrieved from the database.
    /// Else the type will be an <see cref="ErrorResult"/> of type <see cref="NotFoundErrorResult"/> if the collectionName does not exist in the database,
    /// or an <see cref="InternalErrorResult"/>
    /// </returns>
    Task<Result> QueryAsync(string collectionName, float[] vector);

    /// <summary>
    /// Asynchronously queries a collection to find the closest matching items based on a vector.
    /// </summary>
    /// <param name="collectionName">The name of the collection to query. Cannot be null or empty.</param>
    /// <param name="vector">The vector used to find matching items. Must not be null and should have a valid length.</param>
    /// <param name="limit">The maximum number of results to return. Must be greater than zero.</param>
    /// <param name="minScore">The minimum score threshold for results to be included. Results with a score below this value will be excluded.</param>
    /// <returns>A task representing the asynchronous operation, containing a <see cref="Result"/> that indicates success or failure of the operation.
    /// If successful the result will be of type <see cref="SuccessResult{T}"/> where T is a list of strings containing each fact retrieved from the database.
    /// Else the type will be an <see cref="ErrorResult"/> of type <see cref="NotFoundErrorResult"/> if the collectionName does not exist in the database,
    /// or an <see cref="InternalErrorResult"/>
    /// </returns>
    Task<Result> QueryForScoredFactsAsync(string collectionName, float[] vector, ulong limit, float minScore);

    Task<Result> GetCollectionNamesAsync();
}