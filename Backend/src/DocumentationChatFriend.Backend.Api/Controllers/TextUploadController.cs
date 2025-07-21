using DocumentationChatFriend.Backend.Application.Factories;
using DocumentationChatFriend.Backend.Domain.Enums;
using DocumentationChatFriend.Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Api.Controllers;

[ApiController]
[Route("api/upload")]
public class TextUploadController : ControllerBase
{
    private readonly ITextUploadService _textUploadService;

    public TextUploadController(ITextUploadService textUploadService)
    {
        _textUploadService = textUploadService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadText(
        [FromBody] UploadTextDto dto, 
        [FromQuery] int chunkLength = 10,
        [FromQuery] int overlap = 0)
    {
        //TODO: Find a better way to solve this
        //TODO: Add validation to the dto.ChunkingStyle to make sure the enum can parse
        var chunkService = dto.ChunkingStyle switch
        {
            ChunkingStyle.Sentence => ChunkServiceFactory.CreateSentenceChunker(),
            ChunkingStyle.Paragraph => ChunkServiceFactory.CreateParagraphChunker(),
            ChunkingStyle.Custom => ChunkServiceFactory.CreateCustomChunker(chunkLength, overlap),
            _ => throw new NotImplementedException()
        };

        var result = await _textUploadService.UpsertAsync(dto.CollectionName, dto.Text, chunkService);

        if (result is not SuccessResult success)
        {
            return StatusCode(503);
        }

        return Ok();
    }


}

public record UploadTextDto(string CollectionName, string Text, ChunkingStyle ChunkingStyle);