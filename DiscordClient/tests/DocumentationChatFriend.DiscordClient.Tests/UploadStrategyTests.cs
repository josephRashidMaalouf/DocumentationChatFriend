using DocumentationChatFriend.DiscordClient.App.Strategies;
using DocumentationChatFriend.DiscordClient.App.TypedClients;
using FakeItEasy;

namespace DocumentationChatFriend.DiscordClient.Tests
{
    public class UploadStrategyTests
    {
        private readonly IBackendClient _backendClient;
        private readonly UploadStrategy _sut;

        public UploadStrategyTests()
        {
            _backendClient = A.Fake<IBackendClient>();
            _sut = new UploadStrategy(_backendClient);
        }

        [Fact]
        public void ExtractOptions_AllOptionsProvided_ReturnsFullyConfiguredUploadOptions()
        {
            var text = "0:100:30||| Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.NotNull(result);
            Assert.Equal(0, result.ChunkingStyle);
            Assert.Equal(100, result.ChunkLength);
            Assert.Equal(30, result.Overlap);
        }
    }
}
