namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IRagService
{
    Task<string> AnswerQuestionAsync(string  question);
}