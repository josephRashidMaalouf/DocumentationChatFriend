using DocumentationChatFriend.Backend.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Application.Services;

public class CollectionManagementService : ICollectionManagementService
{
    private readonly IVectorRepository _vectorRepository;
    private readonly ILogger<CollectionManagementService> _logger;
    public CollectionManagementService(IVectorRepository vectorRepository, ILogger<CollectionManagementService> logger)
    {
        _vectorRepository = vectorRepository;
        _logger = logger;
    }

    public async Task<Result> GetCollectionNamesAsync()
    {
        var result = await _vectorRepository.GetCollectionNamesAsync();

        if (result is ErrorResult errorResult)
        {
            _logger.LogError("An exception was thrown while retrieving collection names: {ex}", errorResult.Reason);
        }
        return result;
    }
}