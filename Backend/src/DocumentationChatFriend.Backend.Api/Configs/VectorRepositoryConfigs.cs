using DocumentationChatFriend.Backend.Api.Helpers;
using DocumentationChatFriend.Backend.Domain.Interfaces;

namespace DocumentationChatFriend.Backend.Api.Configs;

public class VectorRepositoryConfigs : IVectorRepositoryConfigs
{
    public VectorRepositoryConfigs(IConfiguration config)
    {
        var section = config.GetSection(Name);

        Limit = ulong.Parse(ConfigHelper.MustBeSet(section["Limit"], "VectorRepositoryConfigs:Limit"));
        MinScore = float.Parse(ConfigHelper.MustBeSet(section["MinScore"], "VectorRepositoryConfigs:MinScore"));
    }

    public static string Name => nameof(VectorRepositoryConfigs);
    public ulong Limit { get; }
    public float MinScore { get; }
}