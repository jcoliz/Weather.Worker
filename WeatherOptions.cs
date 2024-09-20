using Weather.Worker.Api;

namespace Weather.Worker.Options;

/// <summary>
/// Options for calling into Weather Service
/// </summary>
public record WeatherOptions
{
    /// <summary>
    /// Which Weather Service office to check
    /// </summary>
    public NWSForecastOfficeId Office {
        get;
        init;
    }

    /// <summary>
    /// Grid position X coordinate
    /// </summary>
    public int GridX {
        get;
        init;
    }

    /// <summary>
    /// Grid position Y coordinate
    /// </summary>
    public int GridY {
        get;
        init;
    }
}
