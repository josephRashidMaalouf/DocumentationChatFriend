using DocumentationChatFriend.DiscordClient.App.TypedClients;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.DiscordClient.App.Strategies;

public class HelpStrategy : MessageStrategyBase
{
    public HelpStrategy(IBackendClient client) : base(client)
    {
    }

    public override async Task<Result> Execute(string collectionName, string text)
    {
        return new SuccessResult<string>("""
                                         
                                         # Ask a question
                                         !ask your-question
                                         
                                         # Upload data
                                         
                                         ## Formats
                                         
                                         You can either upload text by attatching a .txt, or just pasting text and sending it as a chat message:
                                         
                                         !upload info-text | info.txt
                                         
                                         ## Chunking options
                                         
                                         You can specify how you want your text to be chunked before being embedded and stored:
                                         
                                         !upload:int:int:int||| info-text | info.txt
                                         
                                         The first parameter is the chunking style:
                                         - 0 = Sentence based
                                         - 1 = Paragraph based
                                         - 2 = Custom chunking
                                         
                                         If custom chunking is requested, the next two parameters will be applied, otherwise ignored.
                                         
                                         The second parameter is only applicable for custom chunking and represents the chunking length. This equals the amount of words within in each chunk.
                                         
                                         The third parameter is also only applicable for custom chunking and represents overlap in words between chunks.
                                         
                                         """);
    }
}