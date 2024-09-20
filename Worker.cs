using System.Runtime.CompilerServices;
using Weather.Worker.Api;

namespace Weather.Worker;

public partial class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = new GridpointClient(new HttpClient());

        while (!stoppingToken.IsCancellationRequested)
        {
            var forecast = await client.ForecastAsync(NWSForecastOfficeId.SEW,124,69,stoppingToken);
            var json = System.Text.Json.JsonSerializer.Serialize(forecast);

            _logger.LogInformation("Forecast: {Forecast}",json);

            logOk();
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "{Location}: OK", EventId = 1000)]
    public partial void logOk([CallerMemberName] string? location = null);
}
