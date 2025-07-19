using DocumentationChatFriend.Backend.Domain.Interfaces;
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

    public async Task<Result> AnswerQuestionAsync(string question)
    {
        var embedding = await _embedding.EmbedTextAsync(question);
        var result = await _vectorRepository.QueryAsync("dog-facts", embedding.Embedding.ToArray());

        var facts = string.Join("\n", result);

        var answer = await _chatAdapter.GenerateAsync($"Facts: {facts} \nQuestion: {question}");

        return answer.Response;
    }
}