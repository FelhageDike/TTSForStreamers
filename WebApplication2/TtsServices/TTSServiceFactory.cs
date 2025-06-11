using WebApplication2.TtsServices;

public class TTSServiceFactory
{
    public static ITTSService CreateService(string serviceType, IConfiguration config)
    {
        switch (serviceType.ToLower())
        {
            // case "azure":
            //     return new AzureTTSService(
            //         config["Azure:TTS:ApiKey"],
            //         config["Azure:TTS:Region"]
            //     );
            // case "silero":
            //     return new SileroTTSService(
            //         config["Silero:TTS:ApiUrl"],
            //         config["Silero:TTS:ApiToken"]
            //     );
            // case "google":
            //     return new GoogleTTSService(
            //         config["Google:TTS:ApiKey"],
            //         config["Google:TTS:ProjectId"]
            //     );
            case "microsoft":
                return new MicrosoftTTSService(
                    config["Microsoft:TTS:OutputPath"] // Путь для временных файлов
                );
            default:
                throw new ArgumentException($"Unsupported TTS service: {serviceType}");
        }
    }
}
