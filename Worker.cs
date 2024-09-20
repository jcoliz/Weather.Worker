using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Weather.Worker.Api;
using Weather.Worker.Options;

namespace Weather.Worker;

/// <summary>
/// Background worker service
/// </summary>
/// <remarks>
/// Keeps everything running!
/// </remarks>
/// <param name="client">API client to use to connect with weather service</param>
/// <param name="options">Options describing where we want the weather</param>
/// <param name="logger">Where to log results</param>
public partial class Worker(
    WeatherClient client, 
    IOptions<WeatherOptions> options, 
    ILogger<Worker> logger
    ) : BackgroundService
{
    /// <summary>
    /// Where to log results
    /// </summary>
    /// <seealso href="https://adamstorr.co.uk/blog/primary-constructor-and-logging-dont-mix/">
    private readonly ILogger<Worker> _logger = logger;

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await FetchForecastAsync(stoppingToken);
                await Task.Delay(options.Value.Frequency, stoppingToken);
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
            var forecast = await client.Gridpoint_ForecastAsync(
                options.Value.Office, 
                options.Value.GridX, 
                options.Value.GridY, 
                stoppingToken
            );
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
