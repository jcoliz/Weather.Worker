using System.Runtime.CompilerServices;
using Weather.Worker.Api;

namespace Weather.Worker;

public partial class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly GridpointClient client = new(new HttpClient());

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await FetchForecastAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed");
        }
    }

    /// <summary>
    /// Fetch forecast from Weather Service
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    private async Task FetchForecastAsync(CancellationToken stoppingToken)
    {
        try
        {
            var forecast = await client.ForecastAsync(NWSForecastOfficeId.SEW,124,69,stoppingToken);
            var json = System.Text.Json.JsonSerializer.Serialize(forecast);

            _logger.LogInformation("Forecast: OK {Forecast}",json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error");
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "{Location}: OK", EventId = 1000)]
    public partial void logOk([CallerMemberName] string? location = null);
}
