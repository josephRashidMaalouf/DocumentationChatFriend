using DocumentationChatFriend.Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Api.Controllers;

[ApiController]
[Route("api/completions")]
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

        if (result is NotFoundErrorResult notFound)
        {
            return NotFound(notFound.Reason);
        }

        if (result is not SuccessResult<string> success)
        {
            return StatusCode(503);
        }

        return Ok(success.Data);
    }
}

public record PostQuestionDto(string Question, string CollectionName);