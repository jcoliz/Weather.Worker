using Weather.Worker;
using Weather.Worker.Api;
using Weather.Worker.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient<GridpointClient>();
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<WeatherOptions>(
            builder.Configuration.GetSection("Weather")
);

var host = builder.Build();

await host.RunAsync();