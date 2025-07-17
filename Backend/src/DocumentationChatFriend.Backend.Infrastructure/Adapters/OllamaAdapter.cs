using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using OllamaSharp;
using OllamaSharp.Models;

namespace DocumentationChatFriend.Backend.Infrastructure.Adapters;

//public class OllamaAdapter : IChatAdapter
//{
//    IOllamaApiClient _client;
//    private readonly string _systemPrompt;
//    public OllamaAdapter(IOllamaApiClient client, string model)
//    {
//        _client = client;
//        _client.SelectedModel = model;

//        _systemPrompt = """
//                        You are a strict fact-based assistant.
//                        You will be given a list of facts and a question.
//                        Answer the question using only the provided facts.
//                        Do not use external knowledge.
//                        If the answer can be clearly derived from the facts, provide the answer followed by a brief explanation.
//                        If the answer cannot be determined from the facts alone, reply with: "Cannot answer based on the given facts."
//                        """ + "\n\n";
        
//    }


//    public Task<GenerationResponse> GenerateAsync(string prompt)
//    {
//        var result =  _client.GenerateAsync(new GenerateRequest()
//        {
//            Prompt = prompt,
//            Stream = false,
//            Options = new RequestOptions()
//            {
//                Temperature = (float)0.5
//            }
//        });

//        return Task.FromResult(result);
//    }
//}

