using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IRagService
{
    /// <summary>
    /// Asynchronously processes a question and returns an answer.
    /// </summary>
    /// <param name="question">The question to be answered. Cannot be null or empty.</param>
    /// <param name="collectionName">The name of the collection to search for the answer. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result"/> indicating the status of the operation.
    /// If successful the result will be of type <see cref="SuccessResult{T}"/> where T is a string containing the answer to the question. If the operation fails the
    /// result will be of type <see cref="NotFoundErrorResult"/> if the collectionName does not exist in the database. Else it will be an <see cref="InternalErrorResult"/>.
    /// </returns>
    Task<Result> AnswerQuestionAsync(string  question, string collectionName);
}