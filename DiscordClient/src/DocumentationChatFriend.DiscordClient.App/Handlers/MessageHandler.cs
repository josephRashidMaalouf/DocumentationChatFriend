﻿using DocumentationChatFriend.DiscordClient.App.CustomResults;
using DocumentationChatFriend.DiscordClient.App.Strategies;
using DocumentationChatFriend.DiscordClient.App.TypedClients;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.Handlers;

public class MessageHandler
{
    private readonly IBackendClient _client;
    private readonly Dictionary<string, IMessageStrategy> _messageStrategies;

    public MessageHandler(IBackendClient client)
    {
        _client = client;

        _messageStrategies = new Dictionary<string, IMessageStrategy>()
        {
            { "!upload", new UploadStrategy(_client) },
            { "!ask", new AskStrategy(_client) },
            {"!help", new HelpStrategy(_client)}
        };
    }

    public async Task<Result> Handle(string collectionName, string text)
    {
        var action = text.Split(' ', '\n')[0];

        if (action != "!upload" && action != "!ask" && action != "!help")
        {
            return new NoActionRequestedResult();
        }

        return await _messageStrategies[action].Execute(collectionName, text);
    }
}