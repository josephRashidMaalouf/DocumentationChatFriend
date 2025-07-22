using System.Net.Http.Json;
using System.Text.Json;
using DocumentationChatFriend.Backend.Domain.Interfaces;
using DocumentationChatFriend.Backend.Domain.Models;
using ResultPatternJoeget.Results;

namespace DocumentationChatFriend.Backend.Infrastructure.TypedClients;

public class OllamaClient : IChatAdapter
{
    private readonly HttpClient _httpClient;
    private readonly IOllamaClientConfigs _config;
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
                                                If the answer can be clearly derived from the facts, provide the answer in a full sentence response.
                                                Remember that the person asking you will not be aware that you are provided with a list of facts."
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
                return new InternalErrorResult(
                    $"The request to the Ollama API failed. Status code: {result.StatusCode}");
            }

            var generationResponse = await result.Content.ReadFromJsonAsync<GenerationResponse>();

            if (generationResponse is null)
            {
                return new InternalErrorResult("Ollama API returned no data.");
            }

            return new SuccessResult<GenerationResponse>(generationResponse);
        }
        catch (OperationCanceledException ex)
        {
            return new InternalErrorResult($"Could not process the request to the Ollama API: {ex.Message}");
        }
        catch (JsonException ex)
        {
            return new InternalErrorResult(
                $"Encountered an error while trying to deserialize the response from the Ollama API: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new InternalErrorResult(
                $"Something unexpected happened while trying to process the request to the Ollama API: {ex.Message}");
        }
    }
}

file record OllamaGenerateRequest(
    string Model, 
    string Prompt, 
    int MaxTokens = 512, 
    double Temperature = 0.9, 
    bool Stream = false);