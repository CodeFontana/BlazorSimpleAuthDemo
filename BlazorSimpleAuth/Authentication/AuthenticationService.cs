using BlazorSimpleAuth.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorSimpleAuth.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthenticationService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public bool Login(LoginUserModel loginUser)
    {
        ArgumentNullException.ThrowIfNull(loginUser);
        ArgumentNullException.ThrowIfNull(loginUser.Username);
        ArgumentNullException.ThrowIfNull(loginUser.Password);

        // This code here would never fly in production, and is for demo purposes only. This is
        // the spot you would farm out your authentication to a proper identity provider.
        if (loginUser.Username.Equals("admin", StringComparison.InvariantCultureIgnoreCase)
            && loginUser.Password.Equals("password", StringComparison.InvariantCultureIgnoreCase))
        {
            ((SimpleAuthStateProvider)_authStateProvider).NotifyLoginUser(loginUser);
            return true;
        }

        return false;
    }

    public void Logout()
    {
        ((SimpleAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
}
