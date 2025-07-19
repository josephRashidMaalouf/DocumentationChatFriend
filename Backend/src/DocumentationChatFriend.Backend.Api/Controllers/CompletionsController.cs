using DocumentationChatFriend.Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;
using ResultPatternJoeget.Errors;
using ResultPatternJoeget.Results;

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
    public async Task<IActionResult> PostQuestion([FromBody]PostQuestionDto dto)
    {
        var result = await _ragService.AnswerQuestionAsync(dto.Question, dto.CollectionName);

        if (result is ErrorResult error)
        {
            if (error.Errors.Any(x => x.GetType() is NotFoundError))
            {
                return x
            }
        }

        return Ok(result);
    }
}

public record PostQuestionDto(string Question, string CollectionName);