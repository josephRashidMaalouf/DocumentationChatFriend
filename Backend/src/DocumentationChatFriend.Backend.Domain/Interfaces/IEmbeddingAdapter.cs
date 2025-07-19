using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IEmbeddingAdapter
{
    Task<Result> EmbedTextAsync(string text);
}