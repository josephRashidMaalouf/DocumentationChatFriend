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

        [Fact]
        public void ExtractOptions_PartialOptionsProvided_ReturnsPartiallyConfiguredUploadOptions()
        {
            var text = "0:100||| Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.NotNull(result);
            Assert.Equal(0, result.ChunkingStyle);
            Assert.Equal(100, result.ChunkLength);
            Assert.Null(result.Overlap);
        }
        [Fact]
        public void ExtractOptions_PartiallyOptionsProvided_ButWeirdlyFormatted_ReturnsPartiallyConfiguredUploadOptions()
        {
            var text = "0:100:||| Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.NotNull(result);
            Assert.Equal(0, result.ChunkingStyle);
            Assert.Equal(100, result.ChunkLength);
            Assert.Null(result.Overlap);
        }
        [Fact]
        public void ExtractOptions_OnlyOneOptionProvided_ReturnsThatOptionConfigured()
        {
            var text = "0||| Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.NotNull(result);
            Assert.Equal(0, result.ChunkingStyle);
            Assert.Null(result.ChunkLength);
            Assert.Null(result.Overlap);
        }
        [Fact]
        public void ExtractOptions_NoOptionsProvided_ReturnsNull()
        {
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.Null(result);
        }
        [Fact]
        public void ExtractOptions_OptionsWronglyProvided_NoEndMarker_ReturnsNull()
        {
            var text = "0:100:30 Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.Null(result);
        }
        public void ExtractOptions_OptionsWronglyProvided_NoColonSeparation_ReturnsNull()
        {
            var text = "0 100 30 Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.Null(result);
        }
        [Fact]
        public void ExtractOptions_ChunkStyleGreaterThan2_ReturnsChunkingStyleSentence()
        {
            var text = "5||| Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.NotNull(result);
            Assert.Equal(0, result.ChunkingStyle);
            Assert.Null(result.ChunkLength);
            Assert.Null(result.Overlap);
        }
        [Fact]
        public void ExtractOptions_AllOptionsProvided_ButNegativeNumbers_ReturnsFullyConfiguredUploadOptions_WithPositiveValues()
        {
            var text = "-1:-100:-30||| Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.NotNull(result);
            Assert.Equal(1, result.ChunkingStyle);
            Assert.Equal(100, result.ChunkLength);
            Assert.Equal(30, result.Overlap);
        }
        [Fact]
        public void ExtractOptions_MoreThanAllOptionsProvidedReturnsFullyConfiguredUploadOptions_IgnoresTrailingvalues()
        {
            var text = "1:100:30:34:2:4:5||| Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            var result = _sut.ExtractOptions(text);

            Assert.NotNull(result);
            Assert.Equal(1, result.ChunkingStyle);
            Assert.Equal(100, result.ChunkLength);
            Assert.Equal(30, result.Overlap);
        }
    }
}
