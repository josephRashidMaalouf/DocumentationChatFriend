using DocumentationChatFriend.Backend.Domain.Models;
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
    /// <summary>
    /// Streams the response to a given question asynchronously, generating a sequence of responses.
    /// </summary>
    /// <param name="question">The question to be answered. Cannot be null or empty.</param>
    /// <param name="collectionName">The name of the collection to be used for generating the response. Cannot be null or empty.</param>
    /// <returns>An asynchronous stream of <see cref="StreamGenerationResponse"/> objects representing the generated responses.</returns>
    IAsyncEnumerable<StreamGenerationResponse> StreamAnswerQuestionAsync(string  question, string collectionName);
    /// <summary>
    /// Asynchronously retrieves a collection of facts related to the specified question.
    /// </summary>
    /// <param name="question">The question for which related facts are to be retrieved. Cannot be null or empty.</param>
    /// <param name="collectionName">The name of the collection from which facts are retrieved. Cannot be null or empty.</param>
    /// <param name="minScore">The minimum score threshold for facts to be included in the results. Must be a non-negative value.</param>
    /// <param name="limit">The maximum number of facts to retrieve. Must be greater than zero.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="Result"/> object with the
    /// retrieved facts.</returns>
    Task<Result> GetFactsAsync(string  question, string collectionName, float minScore, ulong limit);
}