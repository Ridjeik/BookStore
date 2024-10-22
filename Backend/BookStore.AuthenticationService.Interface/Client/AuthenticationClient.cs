using BookStore.AuthenticationService.Interface.Requests;
using BookStore.AuthenticationService.Interface.Responses;
using System.Net.Http.Json;

namespace BookStore.AuthenticationService.Interface.Client
{
    internal class AuthenticationClient(HttpClient httpClient) : IAuthenticationClient
    {
        private HttpClient HttpClient { get; } = httpClient;

        public Task<GetUserResponse> GetUser(GetUserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginUserResponse> Login(LoginUserRequest request)
        {
            var response = await this.HttpClient.PostAsJsonAsync("api/authentication/login", request);

            if (response is null || !response.IsSuccessStatusCode)
            {
                return new LoginUserResponse() { IsSuccess = false };
            }

            return await response.Content.ReadFromJsonAsync<LoginUserResponse>() ?? new LoginUserResponse() { IsSuccess = false };
        }

        public async Task<LogoutUserResponse> Logout(LogoutUserRequest request)
        {
            var response = await this.HttpClient.PostAsJsonAsync("api/authentication/logout", request);

            if (response is null || !response.IsSuccessStatusCode)
            {
                return new LogoutUserResponse() { IsSuccess = false };
            }

            return await response.Content.ReadFromJsonAsync<LogoutUserResponse>() ?? new LogoutUserResponse() { IsSuccess = false };
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            var response = await this.HttpClient.PostAsJsonAsync("api/authentication/register", request);

            if (response is null || !response.IsSuccessStatusCode)
            {
                return new RegisterUserResponse() { IsSuccess = false };
            }

            return await response.Content.ReadFromJsonAsync<RegisterUserResponse>() ?? new RegisterUserResponse() { IsSuccess = false };
        }
    }
}