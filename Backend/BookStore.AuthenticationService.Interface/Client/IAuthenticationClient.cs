using BookStore.AuthenticationService.Interface.Requests;
using BookStore.AuthenticationService.Interface.Responses;

namespace BookStore.AuthenticationService.Interface.Client;

public interface IAuthenticationClient
{
    Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);

    Task<LoginUserResponse> Login(LoginUserRequest request);

    Task<LogoutUserResponse> Logout(LogoutUserRequest request);

    Task<GetUserResponse> GetUser(GetUserRequest request);
}
