using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Bookings.Shared.DTOs;
using static Bookings.Shared.ServiceResponses;
namespace Bookings.Client.Services;

public class AuthenticationHttpClient
{
    private readonly HttpClient _httpClient;


    public AuthenticationHttpClient(HttpClient client)
    {
        _httpClient = client;
        client.BaseAddress = new Uri("http://localhost:5176/");


    }

    public async Task<HttpResponseMessage> Register(RegisterUserDTO registerUser)
    {
        var serializedUser = JsonContent.Create(registerUser);
        var result = await _httpClient.PostAsync("/register", serializedUser);
        return result;
    }

    public async Task<LoginResponse> Login(LoginDTO loginUser)
    {
        var serializedUser = JsonContent.Create(loginUser);
        var result = await _httpClient.PostAsync("/login", serializedUser);
        // get the token from the response and store it in the local storage
        if (!result.IsSuccessStatusCode) return new LoginResponse(false, null!, "Error occured. Try again later...");
        var content = await result.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<string>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new LoginResponse(true, token, "Login Successful");
    }
}