using DocumentationChatFriend.Backend.Application.Factories;
using DocumentationChatFriend.Backend.Application.Queues;
using DocumentationChatFriend.Backend.Domain.Enums;
using DocumentationChatFriend.Backend.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentationChatFriend.Backend.Api.Controllers;

[ApiController]
[Route("api/upload")]
public class TextUploadController : ControllerBase
{
    private readonly TextUploadQueue _queue;

    public TextUploadController(TextUploadQueue queue)
    {
        _queue = queue;
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

        var job = new TextUploadJob()
        {
            CollectionName = dto.CollectionName,
            Text = dto.Text,
            ChunkService = chunkService
        };

        await _queue.EnqueueAsync(job);

        return Ok();
    }


}

public record UploadTextDto(string CollectionName, string Text, ChunkingStyle ChunkingStyle);