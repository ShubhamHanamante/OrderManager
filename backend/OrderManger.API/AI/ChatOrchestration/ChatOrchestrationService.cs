using System.Text.Json;
using OrderManager.API.AI.Ollama;
using OrderManager.API.AI.Tools;
using OrderManager.API.DTOs.Chat;

namespace OrderManager.API.AI.ChatOrchestration;

public class ChatOrchestrationService : IChatOrchestrationService
{
    private readonly IOllamaClient _ollamaClient;
    private readonly IOrderToolsRegistry _toolsRegistry;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChatOrchestrationService> _logger;

    private const string SystemPrompt = """
        You are an expert AI customer support assistant for an e-commerce order management system.
        Your goal is to answer customer questions about their orders, shipments, delivery tracking, payment status, and order history accurately and politely.

        CRITICAL OPERATIONAL RULES:
        1. Always rely ONLY on the verified data returned by backend tools. NEVER guess, estimate, or hallucinate order numbers, amounts, dates, tracking numbers, or statuses.
        2. When a customer asks about a specific order (e.g., 'ORD-2026-1002'), select the appropriate tool (e.g., get_order_details, get_shipment_tracking, or get_payment_details).
        3. When a customer asks about recent orders or order status (e.g., 'What are my recent orders?', 'Do I have pending orders?'), call get_recent_orders or get_orders_by_status.
        4. When a customer asks what products they purchased, call get_order_items or get_order_details.
        5. Format all monetary values with the Indian Rupee symbol (₹) using two decimal places (e.g. ₹5,996.64).
        6. If a tool returns an error or an order is not found, explain this politely and ask the customer to check the order number.
        7. Keep your explanations clear, structured, and helpful. Highlight key facts like status, carrier, tracking number, and delivery date using bullet points where helpful.
        """;

    public ChatOrchestrationService(
        IOllamaClient ollamaClient,
        IOrderToolsRegistry toolsRegistry,
        IConfiguration configuration,
        ILogger<ChatOrchestrationService> logger)
    {
        _ollamaClient = ollamaClient;
        _toolsRegistry = toolsRegistry;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ChatResponseDto> ProcessChatAsync(ChatRequestDto request, CancellationToken cancellationToken = default)
    {
        var model = _configuration["Ollama:Model"] ?? "qwen2.5:14b";
        var tools = _toolsRegistry.GetToolDefinitions();

        var messages = new List<OllamaMessage>
        {
            new() { Role = "system", Content = SystemPrompt }
        };

        if (request.History != null && request.History.Count > 0)
        {
            foreach (var historyMsg in request.History.TakeLast(10))
            {
                messages.Add(new OllamaMessage
                {
                    Role = historyMsg.Role,
                    Content = historyMsg.Content
                });
            }
        }

        messages.Add(new OllamaMessage
        {
            Role = "user",
            Content = request.Message
        });

        var toolsUsed = new List<ToolExecutionRecordDto>();
        object? latestStructuredData = null;

        var chatRequest = new OllamaChatRequest
        {
            Model = model,
            Messages = messages,
            Tools = tools,
            Stream = false
        };

        var initialResponse = await _ollamaClient.ChatAsync(chatRequest, cancellationToken);
        var assistantMsg = initialResponse.Message;

        if (assistantMsg.ToolCalls != null && assistantMsg.ToolCalls.Count > 0)
        {
            _logger.LogInformation("Ollama proposed {Count} tool call(s)", assistantMsg.ToolCalls.Count);

            messages.Add(assistantMsg);

            foreach (var toolCall in assistantMsg.ToolCalls)
            {
                var toolName = toolCall.Function.Name;
                var arguments = toolCall.Function.Arguments;

                var result = await _toolsRegistry.ExecuteToolAsync(
                    toolName,
                    arguments,
                    request.CustomerId,
                    cancellationToken);

                toolsUsed.Add(new ToolExecutionRecordDto
                {
                    ToolName = toolName,
                    ArgumentsJson = arguments.GetRawText(),
                    Success = result.Success,
                    ErrorMessage = result.Success ? null : result.Content
                });

                if (result.StructuredData != null)
                {
                    latestStructuredData = result.StructuredData;
                }

                messages.Add(new OllamaMessage
                {
                    Role = "tool",
                    Content = result.Content
                });
            }

            var followUpRequest = new OllamaChatRequest
            {
                Model = model,
                Messages = messages,
                Stream = false
            };

            var finalResponse = await _ollamaClient.ChatAsync(followUpRequest, cancellationToken);

            return new ChatResponseDto
            {
                Message = finalResponse.Message.Content,
                ToolsUsed = toolsUsed,
                StructuredData = latestStructuredData
            };
        }

        return new ChatResponseDto
        {
            Message = assistantMsg.Content,
            ToolsUsed = toolsUsed,
            StructuredData = null
        };
    }
}

