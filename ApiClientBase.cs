using System.Net.Http.Headers;
namespace Weather.Worker.Api;

public partial class WeatherClient
{
    partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
    {
        request.Headers.UserAgent.Add(ProductInfoHeaderValue.Parse("Weather.Worker/0.0.0"));
    }
}