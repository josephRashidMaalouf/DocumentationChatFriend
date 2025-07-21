using DocumentationChatFriend.Backend.Application.ChunkingStyles;
using DocumentationChatFriend.Backend.Application.Services;

namespace DocumentationChatFriend.Backend.Application.Factories;

public class ChunkServiceFactory
{

    public static ChunkService CreateSentenceChunker()
    {
        return new ChunkService(new SentenceChunking());
    }
    public static ChunkService CreateParagraphChunker()
    {
        return new ChunkService(new ParagraphChunking());
    }
    public static ChunkService CreateCustomChunker(int chunkLength, int overlap = 0)
    {
        return new ChunkService(new CustomChunking(chunkLength, overlap));
    }
}