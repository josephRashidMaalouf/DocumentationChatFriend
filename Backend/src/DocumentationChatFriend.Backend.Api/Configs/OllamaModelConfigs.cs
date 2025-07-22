namespace DocumentationChatFriend.Backend.Api.Configs;

public class OllamaModelConfigs
{
    public const string Name = nameof(OllamaModelConfigs);

    public required List<string> Models { get; set; }
}