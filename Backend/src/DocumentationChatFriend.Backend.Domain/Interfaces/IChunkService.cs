namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IChunkService
{
    List<string> Chunk(string text);
}

