using BookStore.Shared.Interface;

namespace BookStore.AuthenticationService.Interface.Responses;

public class LogoutUserResponse : IHttpResponse
{
    public bool IsSuccess { get; init; }
}