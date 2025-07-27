using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Frontend.Blazor.CustomResults;

public class DeserializationErrorResult : ErrorResult
{
    public DeserializationErrorResult(string reason) : base(reason)
    {
    }
}