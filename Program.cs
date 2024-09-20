using Weather.Worker;
using Weather.Worker.Api;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient<GridpointClient>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

await host.RunAsync();