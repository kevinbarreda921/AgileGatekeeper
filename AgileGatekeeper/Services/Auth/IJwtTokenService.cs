using System.Security.Claims;

namespace AgileGatekeeper.Services.Auth;

public interface IJwtTokenService
{
    string GenerateToken(int userId, string email, string name, string role, string? tenantId = null);
    ClaimsPrincipal? ValidateToken(string token);
}
