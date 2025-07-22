using DocumentationChatFriend.Backend.Api.Configs;
using Microsoft.Extensions.Options;
using OllamaSharp;
using OllamaSharp.Models;

namespace DocumentationChatFriend.Backend.Api.Setup;

public static class OllamaExtensions
{
    public static async Task<WebApplication> UseOllama (this WebApplication app)
    {
        var client = app.Services.GetRequiredService<IOllamaApiClient>();
        var configs = app.Services.GetRequiredService<IOptions<OllamaModelConfigs>>();

        foreach (var model in configs.Value.Models)
        {
            Console.WriteLine($"Pulling: {model}");
            var result = client.PullModelAsync(
                new PullModelRequest()
                {
                    Model = model,
                    Stream = true
                }
            );

            await foreach (var response in result)
            {
                if (response is null)
                {
                    continue;
                }
                Console.WriteLine($"Percent: {response.Percent}, Status: {response.Status}");
            }
        }


        return app;
    }

}