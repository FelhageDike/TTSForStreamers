using WebApplication2.TtsServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ITTSService, MicrosoftTTSService>(sp =>
{
    var outputPath = "C:\\Temp\\TTS"; // Укажите путь для временных файлов
    return new MicrosoftTTSService(outputPath);
});

builder.Services.AddSingleton<TwitchBot>(sp =>
{
    var username = "felhagedike";
    var oauthToken = "c62q101j5nnkgwic02emog83pb3uak";
    var channel = "felhagedike";

    // Получаем TTS-сервис из DI
    var ttsService = sp.GetRequiredService<ITTSService>();

    return new TwitchBot(username, oauthToken, channel, ttsService);
});


// Регистрируем фабрику TTS-сервисов
builder.Services.AddSingleton<ITTSService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var defaultTTS = config["DefaultTTS"];
    return TTSServiceFactory.CreateService(defaultTTS, config);
});

var app = builder.Build();
// Включаем поддержку статических файлов
app.UseStaticFiles();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.Run();