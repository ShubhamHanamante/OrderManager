using System.Net.Http.Json;
using System.Text.Json;

namespace OrderManager.API.AI.Ollama;

public class OllamaClient : IOllamaClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OllamaClient> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public OllamaClient(HttpClient httpClient, ILogger<OllamaClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<OllamaChatResponse> ChatAsync(OllamaChatRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending chat request to Ollama model {Model} with {MessageCount} messages",
            request.Model, request.Messages.Count);

        var response = await _httpClient.PostAsJsonAsync("/api/chat", request, JsonOptions, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Ollama returned error status code {StatusCode}: {ErrorBody}",
                response.StatusCode, errorBody);
            throw new HttpRequestException($"Ollama API error ({response.StatusCode}): {errorBody}");
        }

        var result = await response.Content.ReadFromJsonAsync<OllamaChatResponse>(JsonOptions, cancellationToken);
        if (result is null)
        {
            throw new InvalidOperationException("Failed to deserialize Ollama chat response.");
        }

        return result;
    }
}
