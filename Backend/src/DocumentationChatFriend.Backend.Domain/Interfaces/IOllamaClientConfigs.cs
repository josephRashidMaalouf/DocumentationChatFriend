namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IOllamaClientConfigs
{
    string Model { get; }
    public Uri Uri { get; }

    int MaxTokens { get; }
    double Temperature { get; }
}