using WebApplication2.Models;

namespace WebApplication2.TtsServices;

public interface ITTSService
{
    Task<string> SynthesizeSpeechAsync(TTSRequest request);
}