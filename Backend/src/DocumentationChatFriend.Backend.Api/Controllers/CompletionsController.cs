using DocumentationChatFriend.Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace DocumentationChatFriend.Backend.Api.Controllers;

[ApiController]
[Route("completions")]
public class CompletionsController : ControllerBase
{
    private readonly IChatAdapter _chatAdapter;

    public CompletionsController(IChatAdapter chatAdapter)
    {
        _chatAdapter = chatAdapter;
    }

    [HttpPost]
    public async Task<IActionResult> PostQuestion(string question)
    {
        //IOllamaApiClient ollama = new OllamaApiClient(new Uri("http://localhost:11434"));

        //ollama.SelectedModel = "nomic-embed-text";
        //var embedding = await ollama.EmbedAsync(question);

        //return Ok(embedding);

        var response = await _chatAdapter.GenerateAsync(question);

        return Ok(response?.Response);
    }
}