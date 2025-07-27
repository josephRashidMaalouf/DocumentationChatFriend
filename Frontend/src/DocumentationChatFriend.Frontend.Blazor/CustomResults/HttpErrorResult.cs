using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Frontend.Blazor.CustomResults;

public class HttpErrorResult : ErrorResult
{
    public HttpErrorResult(string reason) : base(reason)
    {

    }
}