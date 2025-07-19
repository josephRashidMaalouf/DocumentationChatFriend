using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IChatAdapter
{
    Task<Result> GenerateAsync(string question);
}