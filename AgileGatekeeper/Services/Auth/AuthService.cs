using AgileGatekeeper.Models.Users;

namespace AgileGatekeeper.Services.Auth;

public class AuthService : IAuthService
{
    private UserProfile _currentUser = new();
    private bool _isAuthenticated = true;

    public UserProfile CurrentUser => _currentUser;
    public bool IsAuthenticated => _isAuthenticated;

    public event Action? OnAuthStateChanged;

    public async Task<bool> LoginAsync(string email, string password, bool rememberMe = false)
    {
        await Task.Delay(300);

        _currentUser = new UserProfile
        {
            Name = ParseNameFromEmail(email),
            Email = email,
            Role = "Administrator",
            Location = "New York, NY",
            AvatarUrl = "/images/avatar.jpg",
            DeliveredParcelsCount = 1,
            UnreadMessagesCount = 2,
            UnreadNotificationsCount = 3
        };

        _isAuthenticated = true;
        OnAuthStateChanged?.Invoke();
        return true;
    }

    public async Task<bool> LoginWithMicrosoftAsync()
    {
        await Task.Delay(300);

        _currentUser = new UserProfile
        {
            Name = "Jack Ryad",
            Email = "jack.ryad@agilegatekeeper.io",
            Role = "Enterprise Admin",
            Location = "New York, NY",
            AvatarUrl = "/images/avatar.jpg",
            DeliveredParcelsCount = 1,
            UnreadMessagesCount = 2,
            UnreadNotificationsCount = 3
        };

        _isAuthenticated = true;
        OnAuthStateChanged?.Invoke();
        return true;
    }

    public async Task LogoutAsync()
    {
        await Task.Yield();
        _isAuthenticated = false;
        OnAuthStateChanged?.Invoke();
    }

    private static string ParseNameFromEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return "Jack Ryad";
        var parts = email.Split('@')[0].Split('.', '_', '-');
        var formatted = string.Join(" ", parts
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(p => char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p[1..] : "")));
        return string.IsNullOrWhiteSpace(formatted) ? "Usuario" : formatted;
    }
}
