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
