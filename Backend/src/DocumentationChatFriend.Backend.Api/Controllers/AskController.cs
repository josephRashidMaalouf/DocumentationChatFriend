using DocumentationChatFriend.Backend.Domain.CustomResults;
using DocumentationChatFriend.Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Api.Controllers;

[ApiController]
[Route("api/ask/")]
public class AskController : ControllerBase
{
    private readonly IRagService _ragService;

    public AskController(IRagService ragService)
    {
        _ragService = ragService;
    }

    [HttpPost]
    [Route("answer")]
    public async Task<IActionResult> PostQuestion_GetAnswer([FromBody]PostQuestionDto dto)
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

        if (result is NoFactsFoundResult)
        {
            return Ok("I don't have the facts to answer your question");
        }

        return Ok(success.Data);
    }
    [HttpPost]
    [Route("answer/stream")]
    public async IAsyncEnumerable<StreamAnswerDto> PostQuestion_GetStreamAnswer([FromBody] PostQuestionDto dto)
    {
        var responses = _ragService.StreamAnswerQuestionAsync(dto.Question, dto.CollectionName);

        await foreach (var response in responses)
        {
            yield return new StreamAnswerDto(response.Response, response.Done);
        }
    }

    [HttpPost]
    [Route("facts")]
    public async Task<IActionResult> PostQuestions_GetFacts(
        [FromBody] PostQuestionDto dto, 
        [FromQuery] float minScore = (float)0.7,
        [FromQuery] ulong limit = 3)
    {
        var result = await _ragService.GetFactsAsync(dto.Question, dto.CollectionName, minScore, limit);

        if (result is NotFoundErrorResult notFound)
        {
            return NotFound(notFound.Reason);
        }

        if (result is NoFactsFoundResult)
        {
            return Ok(new List<(float, string)>());
        }

        if (result is not SuccessResult<List<(float, string)>> success)
        {
            return StatusCode(503);
        }

        return Ok(success.Data.Select(x => new GetFactsDto(x.Item1, x.Item2)).ToArray());
    }
}

public record StreamAnswerDto(string Answer, bool Done);
public record PostQuestionDto(string Question, string CollectionName);
public record GetFactsDto(float Accuracy, string Fact);