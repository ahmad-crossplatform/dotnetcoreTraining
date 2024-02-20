using Bookings.Shared.DTOs;

namespace Bookings.Shared;

public interface IAccountService
{
    Task<ServiceResponses.GeneralResponse> CreateAccount(RegisterUserDTO registerUserDto);
    Task<ServiceResponses.LoginResponse> LoginAccount(LoginDTO loginDTO);
}