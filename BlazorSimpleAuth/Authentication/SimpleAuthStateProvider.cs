using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorSimpleAuth.Authentication;

public sealed class SimpleAuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _anonymous;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SimpleAuthStateProvider(IHttpContextAccessor httpContextAccessor,
                                   NavigationManager navMan)
    {
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
            return Task.FromResult(_anonymous);

        ClaimsPrincipal? user = httpContext.User;

        // Ensure we only treat cookie-authenticated identities as signed in
        if (user?.Identity?.IsAuthenticated == true
            && (user.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme
                || string.Equals(user.Identity.AuthenticationType, "Cookies", StringComparison.Ordinal)))
        {
            return Task.FromResult(new AuthenticationState(user));
        }

        return Task.FromResult(_anonymous);
    }

    public async Task NotifyUserAuthenticationAsync()
    {
        try
        {
            ClaimsIdentity identity = new(
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name,
                ClaimTypes.Role);

            ClaimsPrincipal principal = new(identity);
            Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(principal));
            NotifyAuthenticationStateChanged(authState);
            await Task.CompletedTask;
        }
        catch (Exception)
        {
            NotifyUserLogout();
            throw;
        }
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }
}
