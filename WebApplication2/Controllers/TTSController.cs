using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.TtsServices;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class TTSController : ControllerBase
{
    private readonly ITTSService _ttsService;
    private static string _latestAudioBase64 = string.Empty;
    public TTSController(ITTSService ttsService)
    {
        _ttsService = ttsService;
    }
    

    [HttpGet("get-latest-audio")]
    public IActionResult GetLatestAudio()
    {
        if (string.IsNullOrEmpty(_latestAudioBase64))
        {
            return NotFound("No audio available.");
        }

        return Ok(new { audio = _latestAudioBase64 });
    }

    [HttpPost("update-audio")]
    public IActionResult UpdateAudio([FromBody] TTSResponse response)
    {
        if (response == null || string.IsNullOrEmpty(response.Audio))
        {
            return BadRequest("Invalid audio data.");
        }

        _latestAudioBase64 = response.Audio; // Обновляем последнее аудио
        return Ok();
    }
    
    [HttpPost("synthesize")]
    public async Task<IActionResult> SynthesizeSpeech([FromBody] TTSRequest request)
    {
        try
        {
            string audio = await _ttsService.SynthesizeSpeechAsync(request);
            return Ok(new TTSResponse { Audio = audio });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при синтезе речи: {ex.Message}");
        }
    }
}

