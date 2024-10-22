using BookStore.Shared.Interface;

namespace BookStore.AuthenticationService.Interface;

public class LoginUserResponse : IHttpResponse
{
    public bool IsSuccess { get; set; }
}