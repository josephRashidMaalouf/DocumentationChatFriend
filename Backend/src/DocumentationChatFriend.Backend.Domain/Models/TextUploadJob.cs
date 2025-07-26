using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Domain.Models;

public class TextUploadJob
{
    public required string CollectionName { get; set; }
    public required string Text { get; set; }
    public required IChunkService ChunkService { get; set; }
}