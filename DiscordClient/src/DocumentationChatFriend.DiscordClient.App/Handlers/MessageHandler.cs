using DocumentationChatFriend.DiscordClient.App.TypedClients;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.Handlers;

public class MessageHandler
{
    private readonly BackendClient _client;

    public MessageHandler(BackendClient client)
    {
        _client = client;
    }

    public async Task<string> Handle(string collectionName, string text)
    {
        var action = text.Split(' ', '\n')[0];

        if (action != "!upload" && action != "!ask")
        {
            return "";
        }

        var trimmedText = text.Replace(action, "").Trim();

        if (action == "!upload")
        {
            var result = await _client.UploadAsync(collectionName, text);
            if (result is SuccessResult)
            {
                return "Data has been uploaded";
            }
        }

        if (action == "!ask")
        {
            var result = await _client.QueryAsync(collectionName, text);

            if (result is SuccessResult<string> success)
            {
                return success.Data;
            }
        }


        return "";
    }
}