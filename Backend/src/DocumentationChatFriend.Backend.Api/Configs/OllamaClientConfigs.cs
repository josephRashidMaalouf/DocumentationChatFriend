using DocumentationChatFriend.Backend.Api.Helpers;
using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Api.Configs;

public class OllamaClientConfigs : IOllamaClientConfigs
{
    public OllamaClientConfigs(IConfiguration _configuration)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var ollamaSection = _configuration.GetSection("OllamaClientConfigs");

        Model = ConfigHelper.MustBeSet(ollamaSection["Model"], "OllamaConfigs:Model");
        MaxTokens = ConfigHelper.MustBeSet<int>(ollamaSection["MaxTokens"], "OllamaConfigs:MaxTokens");

    }

    public string Model { get; }
    public Uri Uri { get; }
    public int MaxTokens { get; }
    public double Temperature { get; }
}