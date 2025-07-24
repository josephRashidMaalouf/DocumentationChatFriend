using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.CustomErrors;

public class HttpErrorResult : ErrorResult
{
    public HttpErrorResult(string reason) : base(reason)
    {

    }
}