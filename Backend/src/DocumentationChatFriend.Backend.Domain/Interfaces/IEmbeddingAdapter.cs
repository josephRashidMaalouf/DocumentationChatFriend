using DocumentationChatFriend.Backend.Domain.Models;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IEmbeddingAdapter
{
    Task<EmbeddedResponse> EmbedTextAsync(string text);
}