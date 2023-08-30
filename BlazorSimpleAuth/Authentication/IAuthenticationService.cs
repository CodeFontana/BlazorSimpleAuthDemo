using BlazorSimpleAuth.Models;

namespace BlazorSimpleAuth.Authentication;
public interface IAuthenticationService
{
    bool Login(LoginUserModel loginUser);
    void Logout();
}