namespace DocumentationChatFriend.DiscordClient.App.Configurations;

public class DiscordOptions
{
    public const string Name = nameof(DiscordOptions);
    public required string ApplicationId { get; set; }   
    public required string PublicKey { get; set; }
    public required string ClientSecret { get; set; }
    public required string Token { get; set; }
}