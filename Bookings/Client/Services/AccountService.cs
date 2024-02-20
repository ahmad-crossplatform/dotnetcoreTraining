using System.Net.Http;
using Bookings.Shared;
using Bookings.Shared.DTOs;

namespace Bookings.Client.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ServiceResponses.GeneralResponse> CreateAccount(RegisterUserDTO registerUserDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponses.LoginResponse> LoginAccount(LoginDTO loginDTO)
    {
        
        throw new NotImplementedException();
        // var response = await HttpClient
        //       .PostAsync($"{BaseUrl}/login",
        //                      Generics.GenerateStringContent(
        //                                     Generics.SerializeObj(loginDTO)));
        //
        // //Read Response
        // if (!response.IsSuccessStatusCode)
        //     return new LoginResponse(false, null!, "Error occured. Try again later...");
        //
        // var apiResponse = await response.Content.ReadAsStringAsync();
        // return Generics.DeserializeJsonString<LoginResponse>(apiResponse); 

    }
}