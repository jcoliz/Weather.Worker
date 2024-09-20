using System.Net.Http.Headers;
namespace Weather.Worker.Api;

public partial class GridpointClient
{
    partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
    {
        request.Headers.UserAgent.Add(ProductInfoHeaderValue.Parse("github.com-jcoliz/0.0.0"));
    }
}