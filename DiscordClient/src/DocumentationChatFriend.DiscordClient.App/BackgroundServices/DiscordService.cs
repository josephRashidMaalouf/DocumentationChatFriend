using Discord;
using Discord.WebSocket;
using DocumentationChatFriend.DiscordClient.App.Configurations;
using DocumentationChatFriend.DiscordClient.App.CustomResults;
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
    private readonly HttpClient _httpClient;

    public DiscordService(DiscordSocketClient client, IOptions<DiscordOptions> options, MessageHandler messageHandler, IHttpClientFactory _httpClientFactory)
    {
        _client = client;
        _options = options;
        _messageHandler = messageHandler;
        _httpClient = _httpClientFactory.CreateClient();
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

        var content = await ExtractMessageContent(message);

        var result = await _messageHandler.Handle(message.Channel.Name, content);

        if (result is NoActionRequestedResult)
        {
            return;
        }

        if (result is SuccessResult<string> successResult)
        {
            await message.Channel.SendMessageAsync(successResult.Data);
            return;
        }

        await message.Channel.SendMessageAsync(
            "Something went wrong while processing your request. Try again, or tell Joe to fix me ASAP >:(");

    }

    private async Task<string> ExtractMessageContent(SocketMessage message)
    {
        var content = message.Content;

        foreach (var a in message.Attachments)
        {
            if (a.Filename.EndsWith(".txt"))
            {
                var stream = await _httpClient.GetStreamAsync(a.Url);

                var reader = new StreamReader(stream);
                content += '\n' + await reader.ReadToEndAsync();
            }
        }

        return content;
    }
}