using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Application.ChunkingStyles;

public class SentenceChunking : IChunkingStyle
{
    public List<string> Chunk(string text)
    {
        return text.Split(
            new[] { '.', '!', '?' },
            StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }
}