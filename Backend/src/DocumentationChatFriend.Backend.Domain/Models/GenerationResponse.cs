namespace DocumentationChatFriend.Backend.Domain.Models;

public record GenerationResponse(string Response);
public record StreamGenerationResponse(string Response, bool Done);