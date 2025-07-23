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
        var embeddingResult = await _embedding.EmbedTextAsync(question);

        if (embeddingResult is not SuccessResult<EmbeddedResponse> embeddingSuccess)
        {
            var error = (embeddingResult as ErrorResult)?.Reason;
            _logger.LogError(error);
            return embeddingResult;
        }

        var queryResult = await _vectorRepository.QueryAsync(collectionName, embeddingSuccess.Data.Embedding.ToArray());

        if (queryResult is not SuccessResult<List<string>> querySuccess)
        {
            var error = (queryResult as ErrorResult)?.Reason;
            _logger.LogError(error);
            return queryResult;
        }

        if (!querySuccess.Data.Any())
        {
            return new SuccessResult<string>("I don't have the facts to answer your question.");
        }

        var facts = string.Join("\n\n\n-", querySuccess.Data);

        var answerResult = await _chatAdapter.GenerateAsync($"Facts: {facts} \n\nQuestion: {question}");

        if (answerResult is not SuccessResult<GenerationResponse> answerSuccess)
        {
            var error = (answerResult as ErrorResult)?.Reason;
            _logger.LogError(error);
            return answerResult;
        }

        return new SuccessResult<string>(answerSuccess.Data.Response);
    }
}