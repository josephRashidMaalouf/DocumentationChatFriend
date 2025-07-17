namespace DocumentationChatFriend.Backend.Domain.Models;

public record EmbeddedChunkModel(string Text, List<float> Vector);