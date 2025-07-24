namespace DocumentationChatFriend.Backend.Domain.Interfaces.Configs;

public interface IOllamaClientConfigs
{
    string LLMModel { get; }
    string EmbeddingModel { get; }
    public Uri Uri { get; }
    int MaxTokens { get; }
    double Temperature { get; }
}