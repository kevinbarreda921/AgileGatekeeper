using AgileGatekeeper.Models.Users;

namespace AgileGatekeeper.Models.Auth;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
    public UserProfile? User { get; set; }

    public static AuthResult Success(UserProfile user, string token) =>
        new() { Succeeded = true, User = user, Token = token };

    public static AuthResult Failed(string message) =>
        new() { Succeeded = false, ErrorMessage = message };
}
