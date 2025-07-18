using DocumentationChatFriend.Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace DocumentationChatFriend.Backend.Api.Controllers;

[ApiController]
[Route("completions")]
public class CompletionsController : ControllerBase
{
    private readonly IRagService _ragService;

    public CompletionsController(IRagService ragService)
    {
        _ragService = ragService;
    }

    [HttpPost]
    public async Task<IActionResult> PostQuestion([FromBody]string question)
    {
        //IOllamaApiClient ollama = new OllamaApiClient(new Uri("http://localhost:11434"));

        //ollama.SelectedModel = "nomic-embed-text";
        //var embedding = await ollama.EmbedAsync(question);

        //return Ok(embedding);

        var response = await _ragService.AnswerQuestionAsync(question);

        return Ok(response);
    }
}