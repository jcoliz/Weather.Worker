using Weather.Worker.Api;

namespace Weather.Worker.Options;

public record WeatherOptions
{
    public NWSForecastOfficeId Office {
        get;
        init;
    }

    public int GridX {
        get;
        init;
    }

    public int GridY {
        get;
        init;
    }
}