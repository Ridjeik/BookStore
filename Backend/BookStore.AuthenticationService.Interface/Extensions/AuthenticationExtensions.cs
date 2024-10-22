using BookStore.AuthenticationService.Interface.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.AuthenticationService.Interface.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationServiceClient(this IServiceCollection services)
    {
        services.AddHttpClient<IAuthenticationClient, AuthenticationClient>((provider, client) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var authenticationServiceUrl = configuration.GetRequiredSection("Services:AuthenticationService").Value ?? throw new InvalidOperationException("Authentication service URL must be set");

            client.BaseAddress = new Uri(authenticationServiceUrl);
        });

        return services;
    }
}
