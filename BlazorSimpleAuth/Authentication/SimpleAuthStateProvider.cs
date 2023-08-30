using BlazorSimpleAuth.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorSimpleAuth.Authentication;

public sealed class SimpleAuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _anonymous;
    private ClaimsIdentity _identity = new();

    public SimpleAuthStateProvider()
    {
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal user = new(_identity);
        return Task.FromResult(new AuthenticationState(user));
    }

    public void NotifyLoginUser(LoginUserModel loginUser)
    {
        ArgumentNullException.ThrowIfNull(loginUser);
        ArgumentNullException.ThrowIfNull(loginUser.Username);
        ArgumentNullException.ThrowIfNull(loginUser.Password);

        _identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, loginUser.Username) }, "SimpleDemo");
        ClaimsPrincipal user = new(_identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        _identity = new ClaimsIdentity();
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }
}
