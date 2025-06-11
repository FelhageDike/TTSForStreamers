using System.Text;
using System.Text.Json;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using WebApplication2.Models;
using WebApplication2.TtsServices;

public class TwitchBot
{
    private readonly TwitchClient _client;
    private readonly ITTSService _ttsService; // Добавляем TTS-сервис

    public TwitchBot(string username, string oauthToken, string channel, ITTSService ttsService)
    {
        var credentials = new ConnectionCredentials(username, oauthToken);
        _client = new TwitchClient();
        _client.Initialize(credentials, channel);

        _ttsService = ttsService; // Инициализируем TTS-сервис

        _client.OnMessageReceived += OnMessageReceived;
        _client.Connect();
    }

    private async void OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        // Обработка входящих сообщений
        Console.WriteLine($"{e.ChatMessage.Username}: {e.ChatMessage.Message}");

        // Триггерим озвучку текста
        await SynthesizeAndPlaySpeechAsync(e.ChatMessage.Message);
    }

    private async Task SynthesizeAndPlaySpeechAsync(string text)
    {
        try
        {
            // Создаем запрос для TTS
            var ttsRequest = new TTSRequest
            {
                Text = text,
                Voice = "Microsoft Irina Desktop", // Укажите голос по умолчанию
            };

            // Синтезируем речь
            string base64Audio = await _ttsService.SynthesizeSpeechAsync(ttsRequest);

            // Отправляем аудио на сервер
            await SendAudioToServerAsync(base64Audio);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при синтезе речи: {ex.Message}");
        }
    }

    private async Task SendAudioToServerAsync(string base64Audio)
    {
        try
        {
            // Формируем JSON-запрос
            var audioUpdateRequest = new
            {
                Audio = base64Audio
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(audioUpdateRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Отправляем POST-запрос на сервер
            string serverUrl = "https://localhost:7113/TTS/update-audio"; // URL вашего API
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsync(serverUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Ошибка при отправке аудио на сервер.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    public void SendMessage(string message)
    {
        _client.SendMessage(_client.JoinedChannels[0], message);
    }
}