namespace OrderManager.API.AI.Ollama;

public interface IOllamaClient
{
    Task<OllamaChatResponse> ChatAsync(OllamaChatRequest request, CancellationToken cancellationToken = default);
}

