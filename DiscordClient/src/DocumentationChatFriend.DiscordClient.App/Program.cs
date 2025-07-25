using Discord;
using Discord.WebSocket;
using DocumentationChatFriend.DiscordClient.App.BackgroundServices;
using DocumentationChatFriend.DiscordClient.App.Configurations;
using DocumentationChatFriend.DiscordClient.App.Handlers;
using DocumentationChatFriend.DiscordClient.App.TypedClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
builder.Services.Configure<DiscordOptions>(
    builder.Configuration.GetSection(DiscordOptions.Name));


builder.Services.AddSingleton<DiscordSocketConfig>(sp => new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent 
});

builder.Services.AddSingleton<DiscordSocketClient>();

builder.Services.AddTransient<MessageHandler>();

builder.Services.AddHttpClient<IBackendClient, BackendClient>();

builder.Services.AddHostedService<DiscordService>();
var app  = builder.Build();

await app.RunAsync();