using System.Runtime.CompilerServices;
using Weather.Worker.Api;

namespace Weather.Worker;

public partial class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly GridpointClient _client;

    public Worker(GridpointClient client, ILogger<Worker> logger)
    {
        _client = client;
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
            logCriticalFail(ex);
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
            var forecast = await _client.ForecastAsync(NWSForecastOfficeId.SEW,124,69,stoppingToken);
            var json = System.Text.Json.JsonSerializer.Serialize(forecast.Properties.Periods.First());

            logReceivedOk(json);
        }
        catch (Exception ex)
        {
            logFail(ex);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "{Location}: Received OK {Result}", EventId = 1000)]
    public partial void logReceivedOk(string result, [CallerMemberName] string? location = null);

    [LoggerMessage(Level = LogLevel.Error, Message = "{Location}: Failed", EventId = 1008)]
    public partial void logFail(Exception ex, [CallerMemberName] string? location = null);

    [LoggerMessage(Level = LogLevel.Critical, Message = "{Location}: Critical Failure", EventId = 1009)]
    public partial void logCriticalFail(Exception ex, [CallerMemberName] string? location = null);
}
