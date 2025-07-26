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
    /// <param name="textUploadJob">The model containing the collection name, text content, and chunking service. All properties must be valid and non-null.</param>
    /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation. The task result contains a <see
    /// cref="Result"/> indicating the success or failure of the operation.
    /// If successful the result will be of type <see cref="SuccessResult"/>. Else the type will be an <see cref="InternalErrorResult"/>
    /// </returns>
    Task<Result> UpsertAsync(TextUploadJob textUploadJob);
}
