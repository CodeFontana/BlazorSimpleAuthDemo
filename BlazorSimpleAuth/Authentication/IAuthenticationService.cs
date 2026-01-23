using BlazorSimpleAuth.Models;

namespace BlazorSimpleAuth.Authentication;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(LoginUserModel loginUser);
    Task LogoutAsync();
}