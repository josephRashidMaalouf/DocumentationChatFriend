using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Domain.Options;

public class ChunkingOptions
{
    private readonly IChunkingStyle _chunkingStyle;
    //Sentences

    //Paragraphs

    //Certain Length
    //Overlap

    Func<string, List<string>> GetChunkStyle(string text)
    {
        return _chunkingStyle.Chunk;
    }
}