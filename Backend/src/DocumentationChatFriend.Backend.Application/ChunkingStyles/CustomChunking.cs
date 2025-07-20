using DocumentationChatFriend.Backend.Domain.Exceptions;
using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Application.ChunkingStyles;
/// <summary>
/// Provides a custom chunking strategy for dividing text into chunks of specified length with optional overlap.
/// </summary>
/// <remarks>This class implements the <see cref="IChunkingStyle"/> interface, allowing text to be split into
/// chunks based on a specified number of words per chunk and an optional overlap between consecutive chunks.</remarks>
public class CustomChunking : IChunkingStyle
{
    private readonly int _chunkLength;
    private readonly int _overlap;
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomChunking"/> class with the specified chunk length and
    /// overlap.
    /// </summary>
    /// <param name="chunkLength">The length of each chunk. Must be greater than or equal to 1.</param>
    /// <param name="overlap">The number of overlapping elements between consecutive chunks. Must be greater than or equal to 0 and less than
    /// or equal to <paramref name="chunkLength"/>.</param>
    /// <exception cref="ChunkingException">Thrown if <paramref name="chunkLength"/> is less than 1, <paramref name="overlap"/> is less than 0, or <paramref
    /// name="chunkLength"/> is less than <paramref name="overlap"/>.</exception>
    public CustomChunking(int chunkLength, int overlap = 0)
    {
        _chunkLength = chunkLength;
        _overlap = overlap;
        if (chunkLength < 1 || chunkLength < overlap || overlap < 0)
        {
            throw new ChunkingException("Invalid chunking parameters: chunkLength must be >= 1, overlap must be >= 0, and chunkLength must be >= overlap.");
        }
    }

    public List<string> Chunk(string text)
    {
        var words = text.Split(new[] { ' ', '\n' },
            StringSplitOptions.RemoveEmptyEntries);

        var chunks = new List<string>();

        for (int i = 0; i < words.Length;)
        {
            string chunk = string.Empty;
            for (int j = 0; j < _chunkLength;)
            {
                chunk += words[i] + ' ';
                j++;
                i += j - _overlap;
            }
            chunks.Add(chunk.Trim());
        }
        return chunks;
    }
}