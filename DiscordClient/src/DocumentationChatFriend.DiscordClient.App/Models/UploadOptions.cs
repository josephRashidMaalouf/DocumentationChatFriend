namespace DocumentationChatFriend.DiscordClient.App.Models;

public class UploadOptions
{
    public int? ChunkingStyle { get; set; }
    public int? ChunkLength  { get; set; }
    public int? Overlap { get; set; }
}