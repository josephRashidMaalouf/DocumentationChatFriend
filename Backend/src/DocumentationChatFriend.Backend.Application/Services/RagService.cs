using DocumentationChatFriend.Backend.Domain.CustomResults;
using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using Microsoft.Extensions.Logging;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Application.Services;

public class RagService : IRagService
{
    private readonly IEmbeddingAdapter _embedding;
    private readonly IChatAdapter _chatAdapter;
    private readonly IVectorRepository _vectorRepository;
    private readonly ILogger<RagService> _logger;

    public RagService(IEmbeddingAdapter embedding, IChatAdapter chatAdapter, IVectorRepository vectorRepository, ILogger<RagService> logger)
    {
        _embedding = embedding;
        _chatAdapter = chatAdapter;
        _vectorRepository = vectorRepository;
        _logger = logger;
    }

    public async Task<Result> AnswerQuestionAsync(string question, string collectionName)
    {

        var getFactsResult = await GetFactsAsync(question, collectionName);

        if (getFactsResult is not SuccessResult<string> facts)
        {
            return getFactsResult;
        }

        var answerResult = await _chatAdapter.GenerateAsync($"Facts: {facts.Data} \n\nQuestion: {question}");

        if (answerResult is not SuccessResult<GenerationResponse> answerSuccess)
        {
            var error = (answerResult as ErrorResult)?.Reason;
            _logger.LogError("Answer generation error: {Error}", error);
            return answerResult;
        }

        return new SuccessResult<string>(answerSuccess.Data.Response);
    }

    public async IAsyncEnumerable<StreamGenerationResponse> StreamAnswerQuestionAsync(string question, string collectionName)
    {
        var getFactsResult = await GetFactsAsync(question, collectionName);

        if (getFactsResult is not SuccessResult<string> facts)
        {
            yield return new StreamGenerationResponse("I don't have the facts to answer your question", true);
        }
        else
        {
            var responses = _chatAdapter.GenerateStreamAsync($"Facts: {facts.Data} \n\nQuestion: {question}");

            await foreach (var response in responses)
            {
                yield return response;
            }
        }

    }

    public async Task<Result> GetFactsAsync(string question, string collectionName, float minScore, ulong limit)
    {
        var embeddingResult = await _embedding.EmbedTextAsync(question);

        if (embeddingResult is not SuccessResult<EmbeddedResponse> embeddingSuccess)
        {
            var error = (embeddingResult as ErrorResult)?.Reason;
            _logger.LogError("EmbeddingError: {Error}", error);
            return embeddingResult;
        }

        var queryResult = await _vectorRepository.QueryForScoredFactsAsync(collectionName, embeddingSuccess.Data.Embedding.ToArray(), limit, minScore);

        if (queryResult is not SuccessResult<List<(float, string)>> querySuccess)
        {
            var error = (queryResult as ErrorResult)?.Reason;
            _logger.LogError("Query error: {Error}", error);
            return queryResult;
        }

        if (!querySuccess.Data.Any())
        {
            return new NoFactsFoundResult();
        }

        return querySuccess;
    }

    private async Task<Result> GetFactsAsync(string question, string collectionName)
    {
        var embeddingResult = await _embedding.EmbedTextAsync(question);

        if (embeddingResult is not SuccessResult<EmbeddedResponse> embeddingSuccess)
        {
            var error = (embeddingResult as ErrorResult)?.Reason;
            _logger.LogError("EmbeddingError: {Error}", error);
            return embeddingResult;
        }

        var queryResult = await _vectorRepository.QueryAsync(collectionName, embeddingSuccess.Data.Embedding.ToArray());

        if (queryResult is not SuccessResult<List<string>> querySuccess)
        {
            var error = (queryResult as ErrorResult)?.Reason;
            _logger.LogError("Query error: {Error}", error);
            return queryResult;
        }

        if (!querySuccess.Data.Any())
        {
            return new NoFactsFoundResult();
        }

        var facts = string.Join("\n\n\n-", querySuccess.Data);

        return new SuccessResult<string>(facts);
    }
}