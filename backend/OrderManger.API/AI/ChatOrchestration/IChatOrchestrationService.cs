using OrderManager.API.DTOs.Chat;

namespace OrderManager.API.AI.ChatOrchestration;

public interface IChatOrchestrationService
{
    Task<ChatResponseDto> ProcessChatAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
}
