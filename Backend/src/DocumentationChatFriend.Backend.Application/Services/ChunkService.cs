using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Application.Services;

public class ChunkService : IChunkService
{
    private readonly IChunkingStyle _chunkingStyle;

    public ChunkService(IChunkingStyle chunkingStyle)
    {
        _chunkingStyle = chunkingStyle;
    }

    public List<string> Chunk(string text)
    {
        return _chunkingStyle.Chunk(text);
    }
}