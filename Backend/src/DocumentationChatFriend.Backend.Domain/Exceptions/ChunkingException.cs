namespace DocumentationChatFriend.Backend.Domain.Exceptions;

public class ChunkingException(string message) : ArgumentException(message)
{
    
}