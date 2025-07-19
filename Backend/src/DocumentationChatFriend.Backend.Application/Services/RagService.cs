using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Application.Services;

public class RagService : IRagService
{
    private readonly IEmbeddingAdapter _embedding;
    private readonly IChatAdapter _chatAdapter;
    private readonly IVectorRepository _vectorRepository;

    public RagService(IEmbeddingAdapter embedding, IChatAdapter chatAdapter, IVectorRepository vectorRepository)
    {
        _embedding = embedding;
        _chatAdapter = chatAdapter;
        _vectorRepository = vectorRepository;
    }

    public async Task<Result> AnswerQuestionAsync(string question, string collectionName)
    {
        var embeddingResult = await _embedding.EmbedTextAsync(question);

        if (embeddingResult is not SuccessResult<EmbeddedResponse> embeddingSuccess)
        {
            return embeddingResult;
        }

        var queryResult = await _vectorRepository.QueryAsync(collectionName, embeddingSuccess.Data.Embedding.ToArray());

        if (queryResult is not SuccessResult<List<string>> querySuccess)
        {
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
            return answerResult;
        }

        return new SuccessResult<string>(answerSuccess.Data.Response);
    }
}