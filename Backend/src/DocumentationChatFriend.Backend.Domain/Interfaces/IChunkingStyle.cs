namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IChunkingStyle
{
    List<string> Chunk(string text);
}