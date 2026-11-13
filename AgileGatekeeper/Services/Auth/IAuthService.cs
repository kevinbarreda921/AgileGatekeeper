using AgileGatekeeper.Models.Auth;
using AgileGatekeeper.Models.Users;

namespace AgileGatekeeper.Services.Auth;

public interface IAuthService
{
    UserProfile CurrentUser { get; }
    bool IsAuthenticated { get; }
    string? CurrentToken { get; }
    AuthenticationConfig AuthConfig { get; }
    event Action? OnAuthStateChanged;

    Task<AuthResult> LoginAsync(string email, string password, bool rememberMe = false);
    Task<AuthResult> LoginWithMicrosoftAsync(string? microsoftEmail = null, string? microsoftTenantId = null);
    Task LogoutAsync();
}
