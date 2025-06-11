using WebApplication2.Models;

namespace WebApplication2.TtsServices;

using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading.Tasks;

public class MicrosoftTTSService : ITTSService
{
    private readonly string _outputPath;

    public MicrosoftTTSService(string outputPath)
    {
        _outputPath = outputPath;
    }

    public async Task<string> SynthesizeSpeechAsync(TTSRequest request)
    {
        return await Task.Run(() =>
        {
            using (var synthesizer = new SpeechSynthesizer())
            using (var memoryStream = new MemoryStream())
            {
                // Устанавливаем голос (если указан)
                if (!string.IsNullOrEmpty(request.Voice))
                {
                    synthesizer.SelectVoice(request.Voice);
                }

                // Устанавливаем вывод в MemoryStream
                synthesizer.SetOutputToWaveStream(memoryStream);

                // Озвучиваем текст
                synthesizer.Speak(request.Text);

                // Сбрасываем позицию потока в начало
                memoryStream.Position = 0;

                // Читаем данные из потока
                byte[] audioBytes = memoryStream.ToArray();

                // Возвращаем Base64-кодированный результат
                return Convert.ToBase64String(audioBytes);
            }
        });
    }
}