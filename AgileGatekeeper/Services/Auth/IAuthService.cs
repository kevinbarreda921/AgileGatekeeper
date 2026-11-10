using AgileGatekeeper.Models.Users;

namespace AgileGatekeeper.Services.Auth;

public interface IAuthService
{
    UserProfile CurrentUser { get; }
    bool IsAuthenticated { get; }
    event Action? OnAuthStateChanged;

    Task<bool> LoginAsync(string email, string password, bool rememberMe = false);
    Task<bool> LoginWithMicrosoftAsync();
    Task LogoutAsync();
}
