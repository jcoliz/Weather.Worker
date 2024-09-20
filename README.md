# Weather Worker Sample

This is a very simple sample demonstrating a [.NET Worker Service](https://learn.microsoft.com/en-us/dotnet/core/extensions/workers) connecting to the U.S. [National Weather Service API](https://www.weather.gov/documentation/services-web-api) to retrieve and log  forecasts.

## Configuration

Out of the box, the sample requests a weather forecast for the area surrounding the [Space Needle](https://www.spaceneedle.com/),
checking once every 5 seconds.

This can be configured using standard [.NET Configuration](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration)
mechanisms. You can find these values in `appsettings.json`.

```json
  "Weather": {
    "Office": "SEW",
    "GridX": 124,
    "GridY": 69,
    "Frequency": "00:00:05"
  }
```

The weather office, and grid x,y positions are specific to the NWS grid system. You can find values by calling the `/points/{lat,long}`
endpoint. The NWS has a handy Swagger UI on its API page, so you can try these out diretly.

Frequency is described in in Hours:Minutes:Seconds.
