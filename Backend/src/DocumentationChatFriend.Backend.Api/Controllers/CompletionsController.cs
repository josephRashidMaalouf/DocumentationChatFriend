using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace DocumentationChatFriend.Backend.Api.Controllers;

[ApiController]
[Route("completions")]
public class CompletionsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostQuestion(string question)
    {
        IOllamaApiClient ollama = new OllamaApiClient(new Uri("http://localhost:11434"));

        ollama.SelectedModel = "nomic-embed-text";
        var embedding = await ollama.EmbedAsync(question);

        return Ok(embedding);
    }
}