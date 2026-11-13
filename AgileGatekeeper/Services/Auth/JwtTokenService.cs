using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgileGatekeeper.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgileGatekeeper.Services.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;
    private readonly byte[] _keyBytes;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value ?? throw new InvalidOperationException(
            "La sección 'Jwt' no está configurada en appsettings.json.");

        if (string.IsNullOrWhiteSpace(_settings.SecretKey))
        {
            throw new InvalidOperationException(
                "La clave 'Jwt:SecretKey' es obligatoria y no está configurada en appsettings.json.");
        }

        if (_settings.SecretKey.Length < 32)
        {
            throw new InvalidOperationException(
                "La clave 'Jwt:SecretKey' en appsettings.json debe tener al menos 32 caracteres (256 bits) para la firma HMAC-SHA256.");
        }

        if (string.IsNullOrWhiteSpace(_settings.Issuer))
        {
            throw new InvalidOperationException(
                "El emisor 'Jwt:Issuer' no está configurado en appsettings.json.");
        }

        if (string.IsNullOrWhiteSpace(_settings.Audience))
        {
            throw new InvalidOperationException(
                "La audiencia 'Jwt:Audience' no está configurada en appsettings.json.");
        }

        _keyBytes = Encoding.UTF8.GetBytes(_settings.SecretKey);
    }

    public string GenerateToken(int userId, string email, string name, string role, string? tenantId = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(role) ? "usuario" : role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            claims.Add(new Claim("tenant_id", tenantId));
            claims.Add(new Claim("tid", tenantId));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes > 0 ? _settings.ExpiryMinutes : 60),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_keyBytes),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
