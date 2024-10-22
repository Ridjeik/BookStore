using BookStore.AuthenticationService.Interface;
using BookStore.AuthenticationService.Interface.Requests;
using BookStore.AuthenticationService.Interface.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.AuthenticationService.Controllers;

[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    [HttpGet("user")]
    public Task<GetUserResponse> GetUser(GetUserRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("login")]
    public Task<LoginUserResponse> Login(LoginUserRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("logout")]
    public Task<LogoutUserResponse> Logout(LogoutUserRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("register")]
    public Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
    {
        throw new NotImplementedException();
    }
}
