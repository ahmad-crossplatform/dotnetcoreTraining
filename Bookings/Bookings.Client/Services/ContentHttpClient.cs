namespace Bookings.Client.Services;

public class ContentHttpClient:HttpClient
{
    public HttpClient HttpClient { get; }

    public ContentHttpClient(HttpClient client)
    {
        HttpClient = client;
        client.BaseAddress = new Uri("https://localhost:7180/"); 
    }
}