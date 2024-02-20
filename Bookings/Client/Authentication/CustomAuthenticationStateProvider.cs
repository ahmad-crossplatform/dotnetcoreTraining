using System.Security.Claims;
using Blazored.LocalStorage;
using Bookings.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bookings.Client.Authentication;

public class CustomAuthenticationStateProvider:AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private ClaimsPrincipal anonymous = new(new ClaimsIdentity()); 
    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var stringToken = await _localStorageService.GetItemAsStringAsync("token");
            if (string.IsNullOrEmpty(stringToken))
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

            var claims = Generics.GetClaimsFromToken(stringToken);
            var claimPrincipal = Generics.SetClaimsPrincipal(claims);
            return await Task.FromResult(new AuthenticationState(claimPrincipal));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return await Task.FromResult(new AuthenticationState(anonymous));
        } 
    }

    public async Task UpdateAuthenticationState(string token)
    {
        ClaimsPrincipal claimsPrincipal = new();
        if (string.IsNullOrEmpty(token))
        {
            claimsPrincipal = anonymous;
            await _localStorageService.RemoveItemAsync("token"); 
        }
        else
        {
            var userSession = Generics.GetClaimsFromToken(token);
            claimsPrincipal = Generics.SetClaimsPrincipal(userSession);
            await _localStorageService.SetItemAsStringAsync("token", token); 
        }

        NotifyAuthenticationStateChanged(Task.FromResult((new AuthenticationState(claimsPrincipal ))));
    }
}