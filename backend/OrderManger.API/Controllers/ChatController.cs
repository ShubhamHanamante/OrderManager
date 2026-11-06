using Microsoft.AspNetCore.Mvc;
using OrderManager.API.AI.ChatOrchestration;
using OrderManager.API.DTOs.Chat;

namespace OrderManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatOrchestrationService _chatOrchestrationService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IChatOrchestrationService chatOrchestrationService, ILogger<ChatController> logger)
    {
        _chatOrchestrationService = chatOrchestrationService;
        _logger = logger;
    }

    /// <summary>
    /// Processes a customer natural-language query using local Ollama model with backend tool calling.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ChatResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ChatResponseDto>> Chat(
        [FromBody] ChatRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new { message = "Message cannot be empty." });
        }

        if (request.CustomerId <= 0)
        {
            return BadRequest(new { message = "A valid customerId is required." });
        }

        try
        {
            var response = await _chatOrchestrationService.ProcessChatAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to communicate with local Ollama service");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                message = "The local AI service (Ollama) is currently unavailable or busy. Please ensure Ollama is running.",
                details = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error processing chat message");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = "An error occurred while processing your message.",
                details = ex.Message
            });
        }
    }
}
