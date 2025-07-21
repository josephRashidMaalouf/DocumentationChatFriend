using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface ITextUploadService
{

    /// <summary>
    /// Inserts a new document or updates an existing document in the specified collection.
    /// </summary>
    /// <remarks>This method performs an upsert operation, meaning it will insert the document if it does not
    /// exist, or update the existing document if it does. Ensure that the collection name and text are valid and
    /// properly formatted for the underlying data store.</remarks>
    /// <param name="collectionName">The name of the collection where the document will be upserted. Cannot be null or empty.</param>
    /// <param name="text">The text content of the document to be upserted. Cannot be null or empty.</param>
    /// <param name="chunkService">Provides the desired chunking style. Must implement <see cref="IChunkService"/>.</param>
    /// 
    /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation. The task result contains a <see
    /// cref="Result"/> indicating the success or failure of the operation.
    /// If successful the result will be of type <see cref="SuccessResult"/>. Else the type will be an <see cref="InternalErrorResult"/>
    /// </returns>

    Task<Result> UpsertAsync(string collectionName, string text, IChunkService chunkService);
}
