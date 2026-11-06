using System.Text.Json.Serialization;

namespace OrderManager.API.DTOs.Chat;

public class ChatMessageDto
{
    public string Role { get; set; } = "user"; // "user", "assistant", "system", "tool"
    public string Content { get; set; } = string.Empty;
}

public class ChatRequestDto
{
    public string Message { get; set; } = null!;
    public int CustomerId { get; set; }
    public List<ChatMessageDto>? History { get; set; }
}

public class ToolExecutionRecordDto
{
    public string ToolName { get; set; } = null!;
    public string ArgumentsJson { get; set; } = null!;
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ChatResponseDto
{
    public string Message { get; set; } = string.Empty;
    public List<ToolExecutionRecordDto> ToolsUsed { get; set; } = [];
    public object? StructuredData { get; set; }
}

