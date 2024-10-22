using BookStore.Shared.Interface;

namespace BookStore.AuthenticationService.Interface;

public class RegisterUserResponse : IHttpResponse
{
    public bool IsSuccess { get; set; }
}