using DocumentationChatFriend.Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Api.Controllers;

[ApiController]
[Route("api/collections")]
public class CollectionManagementController : ControllerBase
{
    private readonly ICollectionManagementService _collectionManagementService;

    public CollectionManagementController(ICollectionManagementService collectionManagementService)
    {
        _collectionManagementService = collectionManagementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCollectionNames()
    {
        var result = await _collectionManagementService.GetCollectionNamesAsync();

        if (result is not SuccessResult<List<string>> successResult)
        {
            return StatusCode(503);
        }

        return Ok(successResult.Data);
    }
}