namespace DocumentationChatFriend.Backend.Domain.Interfaces.Configs;

public interface IVectorRepositoryConfigs
{
    ulong Limit { get; }
    float MinScore { get; }
}