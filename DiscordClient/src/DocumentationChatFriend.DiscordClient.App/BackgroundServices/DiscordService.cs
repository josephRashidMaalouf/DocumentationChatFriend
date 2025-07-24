using Discord;
using Discord.WebSocket;
using DocumentationChatFriend.DiscordClient.App.Configurations;
using DocumentationChatFriend.DiscordClient.App.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.BackgroundServices;

public class DiscordService : BackgroundService
{
    private readonly DiscordSocketClient _client;
    private readonly IOptions<DiscordOptions> _options;
    private readonly MessageHandler _messageHandler;

    public DiscordService(DiscordSocketClient client, IOptions<DiscordOptions> options, MessageHandler messageHandler)
    {
        _client = client;
        _options = options;
        _messageHandler = messageHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client.Log += Log;

        _client.MessageReceived += ReadMessage;

    await _client.LoginAsync(TokenType.Bot, _options.Value.Token);
    await _client.StartAsync();

    await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private Task Log(LogMessage log)
    {
        Console.WriteLine($"[{log.Severity}] {log.Source}: {log.Message}");

        if (log.Exception is not null)
        {
            Console.WriteLine(log.Exception);
        }
        return Task.CompletedTask;
    }

    private async Task ReadMessage(SocketMessage message)
    {
        if (message.Author.IsBot)
        {
            return;
        }

        if (message.Content.StartsWith("!"))
        {
            var result = await _messageHandler.Handle(message.Channel.Name, message.Content);

            if (!string.IsNullOrWhiteSpace(result))
            {
                await message.Channel.SendMessageAsync(result);
            }
        }

    }
}