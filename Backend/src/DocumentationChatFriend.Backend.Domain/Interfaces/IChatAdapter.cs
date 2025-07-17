using DocumentationChatFriend.Backend.Domain.Models;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IChatAdapter
{
    Task<GenerationResponse> GenerateAsync(string prompt);
}