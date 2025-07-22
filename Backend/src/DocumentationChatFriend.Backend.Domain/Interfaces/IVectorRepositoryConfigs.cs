namespace DocumentationChatFriend.Backend.Domain.Interfaces;

public interface IVectorRepositoryConfigs
{
    ulong Limit { get; }
    float MinScore { get; }
}