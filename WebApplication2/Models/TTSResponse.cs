namespace WebApplication2.Models;

using System.Text.Json.Serialization;

public class TTSResponse
{
    /// <summary>
    /// Base64-кодированное аудио, представляющее озвученный текст.
    /// </summary>
    [JsonPropertyName("audio")]
    public string Audio { get; set; }

    /// <summary>
    /// Формат аудио (например, "wav", "mp3", "raw").
    /// </summary>
    [JsonPropertyName("format")]
    public string? Format { get; set; }

    /// <summary>
    /// Длительность аудио в миллисекундах (если доступно).
    /// </summary>
    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; set; }

    /// <summary>
    /// Статус операции (например, "success", "error").
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; } = "success";

    /// <summary>
    /// Сообщение об ошибке (если произошла ошибка).
    /// </summary>
    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; set; }
}