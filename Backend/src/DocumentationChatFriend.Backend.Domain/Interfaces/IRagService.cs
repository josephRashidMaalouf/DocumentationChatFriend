using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IRagService
{
    Task<Result> AnswerQuestionAsync(string  question);
}