using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.CustomResults;

public class HttpErrorResult : ErrorResult
{
    public HttpErrorResult(string reason) : base(reason)
    {

    }
}