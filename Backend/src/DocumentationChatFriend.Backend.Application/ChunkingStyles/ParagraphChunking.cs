using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Application.ChunkingStyles;

public class ParagraphChunking : IChunkingStyle
{
    public List<string> Chunk(string text)
    {
        return text.Split(
            '\n',
            StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }
}