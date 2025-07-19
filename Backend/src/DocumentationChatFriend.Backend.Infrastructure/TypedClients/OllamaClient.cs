using System.Net.Http.Json;
using System.Text.Json;
using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Errors;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Infrastructure.TypedClients;

public class OllamaClient : IChatAdapter
{
    private readonly HttpClient _httpClient;
    IOllamaClientConfigs _config;
    private string _systemPrompt;

    public OllamaClient(HttpClient httpClient, IOllamaClientConfigs config)
    {
        _httpClient = httpClient;
        _config = config;
        _httpClient.BaseAddress = config.Uri;

        _systemPrompt = """
                                                You are a strict fact-based assistant.
                                                You will be given a list of facts and a question.
                                                Answer the question using only the provided facts.
                                                Do not use external knowledge.
                                                If the answer can be clearly derived from the facts, provide the answer followed by a brief explanation.
                                                If the answer cannot be determined from the facts alone, reply with: "Cannot answer based on the given facts."
                                                """ + "\n\n";
    }

    public async Task<Result> GenerateAsync(string question)
    {
        try
        {
            var prompt = _systemPrompt += question;

            var req = new OllamaGenerateRequest(
                _config.Model,
                prompt,
                _config.MaxTokens,
                _config.Temperature);

            var result = await _httpClient.PostAsJsonAsync("generate", req);

            if (!result.IsSuccessStatusCode)
            {
                return new ErrorResult(
                    new ThirdPartyError($"The request to the Ollama API failed. Status code: {result.StatusCode}"));
            }

            var generationResponse = await result.Content.ReadFromJsonAsync<GenerationResponse>();

            if (generationResponse is null)
            {
                return new ErrorResult(new ThirdPartyError("Ollama API returned no data."));
            }

            return new SuccessResult<GenerationResponse>(generationResponse);
        }
        catch (OperationCanceledException ex)
        {
            return new ErrorResult(new TimeoutError("Could not process the request to the Ollama API."));
        }
        catch (JsonException ex)
        {
            return new ErrorResult(
                new InternalError("Encountered an error while trying to deserialize the response from the Ollama API"));
        }
        catch (Exception ex)
        {
            return new ErrorResult(
                new ThirdPartyError(
                    "Something unexpected happened while trying to process the request to the Ollama API"));
        }
    }
}

file record OllamaGenerateRequest(
    string Model, 
    string Prompt, 
    int MaxTokens = 512, 
    double Temperature = 0.6, 
    bool Stream = false);