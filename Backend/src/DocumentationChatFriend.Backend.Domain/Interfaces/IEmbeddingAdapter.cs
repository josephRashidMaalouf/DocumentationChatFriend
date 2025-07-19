using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IEmbeddingAdapter
{
    /// <summary>
    /// Asynchronously embeds the specified text into a data structure for further processing.
    /// </summary>
    /// <param name="text">The text to be embedded. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="SuccessResult{T}"/> where T is a <see cref="EmbeddedResponse"/> when successful.
    /// When not successful the result will be an <see cref="InternalErrorResult"/>.</returns>
    Task<Result> EmbedTextAsync(string text);
}