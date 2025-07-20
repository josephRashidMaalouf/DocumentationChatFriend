using DocumentationChatFriend.Backend.Domain.Models;
using DocumentationChatFriend.Backend.Domain.Options;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface ITextUploadService
{
    /// <summary>
    /// Inserts or updates a text in the specified collection asynchronously.
    /// </summary>
    /// <remarks>This method ensures that the text is either inserted if it does not exist or updated if
    /// it already exists in the specified collection.</remarks>
    /// <param name="collectionName">The name of the collection where the document will be upserted. Cannot be null or empty.</param>
    /// <param name="text">The text content of the document to be upserted. Cannot be null or empty.</param>
    /// <param name="chunkingOptions">Options for chunking the document text. Determines how the text is divided into chunks for processing.</param>
    /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation. The task result contains a <see
    /// cref="Result"/> indicating the success or failure of the operation.
    /// If successful the result will be of type <see cref="SuccessResult"/>. Else the type will be an <see cref="InternalErrorResult"/>
    /// </returns>
    Task<Result> UpsertAsync(string collectionName, string text, ChunkingOptions chunkingOptions);
}
