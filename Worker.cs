using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Weather.Worker.Api;
using Weather.Worker.Options;

namespace Weather.Worker;

public partial class Worker : BackgroundService
{
    /// <summary>
    /// API client to use to connect with weather service
    /// </summary>
    private readonly WeatherClient _client;

    /// <summary>
    /// Options describing where we want the weather
    /// </summary>
    private readonly WeatherOptions _options;

    /// <summary>
    /// Where to log results
    /// </summary>
    private readonly ILogger<Worker> _logger;

    public Worker(WeatherClient client, IOptions<WeatherOptions> options, ILogger<Worker> logger)
    {
        _client = client;
        _options = options.Value;
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
            var forecast = await _client.Gridpoint_forecastAsync(_options.Office, _options.GridX, _options.GridY, stoppingToken);
            var json = JsonSerializer.Serialize(forecast.Properties.Periods.First());

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
