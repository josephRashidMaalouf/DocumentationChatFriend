using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface ICollectionManagementService
{
    Task<Result> GetCollectionNamesAsync();
}