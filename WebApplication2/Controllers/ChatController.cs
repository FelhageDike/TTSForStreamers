using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly TwitchBot _bot;

    public ChatController(TwitchBot bot)
    {
        _bot = bot;
    }

    [HttpPost("send")]
    public IActionResult SendMessage([FromBody] MessageRequest request)
    {
        if (string.IsNullOrEmpty(request.Message))
        {
            return BadRequest("Message is required.");
        }

        _bot.SendMessage(request.Message);
        return Ok(new { message = "Message sent successfully." });
    }
}

public class MessageRequest
{
    public string Message { get; set; }
}