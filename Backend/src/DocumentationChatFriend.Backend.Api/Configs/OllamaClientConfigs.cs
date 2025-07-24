using DocumentationChatFriend.Backend.Api.Helpers;
using DocumentationChatFriend.Backend.Domain.Interfaces.Configs;

namespace DocumentationChatFriend.Backend.Api.Configs;

public class OllamaClientConfigs : IOllamaClientConfigs
{
    public OllamaClientConfigs(IConfiguration _configuration)
    {

        var ollamaSection = _configuration.GetSection("OllamaClientConfigs");

        LLMModel = ConfigHelper.MustBeSet(ollamaSection["LLMModel"], "OllamaConfigs:LLMModel");
        EmbeddingModel = ConfigHelper.MustBeSet(ollamaSection["EmbeddingModel"], "OllamaConfigs:EmbeddingModel");
        MaxTokens = int.Parse(ConfigHelper.MustBeSet(ollamaSection["MaxTokens"], "OllamaConfigs:MaxTokens"));
        Temperature = double.Parse(ConfigHelper.MustBeSet(ollamaSection["Temperature"], "OllamaConfigs:Temperature"));

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        Uri = new Uri(
            env == "Docker" ?
                ConfigHelper.MustBeSet(ollamaSection["UriDocker"], "OllamaConfigs:UriDocker") :
                ConfigHelper.MustBeSet(ollamaSection["UriDevelopment"], "OllamaConfigs:UriDevelopment")
                );


    }

    public string LLMModel { get; }

    public string EmbeddingModel { get; }

    public Uri Uri { get; }
    public int MaxTokens { get; }
    public double Temperature { get; }
}