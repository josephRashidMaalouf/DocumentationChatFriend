using DocumentationChatFriend.Backend.Application.ChunkingStyles;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System.Runtime.Intrinsics.Arm;

namespace DocumentationChatFriend.Backend.Tests.ChunkingStyles;

public class CustomChunkingTests
{
    public static IEnumerable<object[]> TestData => new List<object[]>
    {
        new object[]
        {
            1,
            0,
            "One two three four five six seven eight nine ten eleven twelve",
            new List<string> {"One", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve"}
        },
        new object[]
        {
            3,
            0,
            "One two three four five six seven eight nine ten eleven twelve",
            new List<string> {"One two three", "four five six", "seven eight nine", "ten eleven twelve"}
        },
        new object[]
        {
        4,
        2,
        "One two three four five six seven eight nine ten eleven twelve",
        new List<string> {"One two three four", "three four five six", "five six seven eight", "seven eight nine ten", "nine ten eleven twelve" }
    }
    };

    [Theory]
    [MemberData(nameof(TestData))]
    public void Chunk_ChunksAsExpected(int chunkLength, int overlap, string text, List<string> expected)
    {
        //Arrange
        var sut = new CustomChunking(chunkLength, overlap);

        //Act
        var result = sut.Chunk(text);

        //Assert
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i], result[i]);
        }
    }

}