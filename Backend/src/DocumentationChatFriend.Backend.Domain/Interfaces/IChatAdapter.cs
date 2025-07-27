using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IChatAdapter
{
    /// <summary>
    /// Asynchronously generates a result based on the provided question.
    /// </summary>
    /// <param name="question">The question to generate an answer to. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result"/> indicating the status of the operation.
    /// If successful the result will be of type <see cref="SuccessResult{T}"/> where T is <see cref="GenerationResponse"/>. If the operation fails the
    /// result will be of type <see cref="InternalErrorResult"/>.
    /// </returns>
    Task<Result> GenerateAsync(string question);
    IAsyncEnumerable<StreamGenerationResponse> GenerateStreamAsync(string question);
}