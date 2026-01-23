using System.Security.Claims;
using BlazorSimpleAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorSimpleAuth.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NavigationManager _navMan;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthenticationService(IHttpContextAccessor httpContextAccessor,
                                 NavigationManager navMan,
                                 AuthenticationStateProvider authStateProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _navMan = navMan;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(LoginUserModel loginUser)
    {
        ArgumentNullException.ThrowIfNull(loginUser);
        ArgumentNullException.ThrowIfNull(loginUser.Username);
        ArgumentNullException.ThrowIfNull(loginUser.Password);

        // This code here would NEVER fly in production, and is for demo purposes only. This is
        // the spot you would farm out your authentication to a proper identity provider.
        if (!loginUser.Username.Equals("admin", StringComparison.InvariantCultureIgnoreCase)
            || !loginUser.Password.Equals("password", StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        // Use common expiration time for local authentication
        DateTimeOffset tokenExpiry = DateTimeOffset.UtcNow.AddMinutes(65);

        // Create claims for local authentication
        IEnumerable<Claim> claims = [
            new Claim(ClaimTypes.Name, $"{Environment.UserName}@{Environment.MachineName}.local")
        ];

        // Create principal for ASP.NET Core
        ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new(identity);

        // Signin - Notify ASP.NET Core
        if (_httpContextAccessor.HttpContext != null)
        {
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                });
        }

        // Signin - Notify Blazor
        await ((SimpleAuthStateProvider)_authStateProvider).NotifyUserAuthenticationAsync();

        // Navigate to home page
        _navMan.NavigateTo("", true);

        return true;
    }

    public async Task LogoutAsync()
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is not available.");

        // Sign out - Notify ASP.NET Core
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Signout - Notify Blazor
        ((SimpleAuthStateProvider)_authStateProvider).NotifyUserLogout();

        // Navigate to home page
        _navMan.NavigateTo("", true);
    }
}
