using System.Data;
using AgileGatekeeper.Models.Auth;
using AgileGatekeeper.Models.Users;
using AgileGatekeeper.Services.Database;
using Dapper;
using Microsoft.Extensions.Options;

namespace AgileGatekeeper.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly AuthenticationConfig _authConfig;

    private UserProfile _currentUser = new();
    private bool _isAuthenticated = false;
    private string? _currentToken;

    public UserProfile CurrentUser => _currentUser;
    public bool IsAuthenticated => _isAuthenticated;
    public string? CurrentToken => _currentToken;
    public AuthenticationConfig AuthConfig => _authConfig;

    public event Action? OnAuthStateChanged;

    public AuthService(
        IDbConnectionFactory connectionFactory,
        IJwtTokenService jwtTokenService,
        IOptions<AuthenticationConfig> authOptions)
    {
        _connectionFactory = connectionFactory;
        _jwtTokenService = jwtTokenService;
        _authConfig = authOptions.Value ?? throw new InvalidOperationException(
            "La sección 'Authentication' no está configurada en appsettings.json.");
    }

    public async Task<AuthResult> LoginAsync(string email, string password, bool rememberMe = false)
    {
        if (!_authConfig.IsLocalEnabled)
        {
            return AuthResult.Failed("La autenticación local está deshabilitada en la configuración del sistema (appsettings.json).");
        }

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return AuthResult.Failed("El correo y la contraseña son requeridos.");
        }

        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@vc_correo", email.Trim(), DbType.AnsiString);
            parameters.Add("@vc_password_hash", password, DbType.AnsiString);

            var userRecord = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "security.sp_login_local",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (userRecord == null)
            {
                return AuthResult.Failed("Credenciales incorrectas o la cuenta no está activa.");
            }

            int userId = (int)userRecord.in_usuario_id;
            string userEmail = (string)userRecord.vc_correo;
            string fullName = $"{userRecord.vc_nombre} {userRecord.vc_apellido}".Trim();
            string roleName = (string)(userRecord.vc_rol_nombre ?? "Administrador");
            string roleCode = (string)(userRecord.vc_rol_codigo ?? "admin");
            string? tenantId = (string?)userRecord.vc_tenant_id;

            var token = _jwtTokenService.GenerateToken(userId, userEmail, fullName, roleCode, tenantId);

            _currentUser = new UserProfile
            {
                Id = userId,
                Name = fullName,
                Email = userEmail,
                Role = roleName,
                RoleCode = roleCode,
                TenantId = tenantId,
                Location = userRecord.vc_ubicacion ?? "Sede Central",
                AvatarUrl = userRecord.vc_avatar_url ?? "/images/avatar.jpg",
                CreatedBy = (string?)userRecord.vc_usuario_creacion,
                CreatedAt = (DateTime?)userRecord.dt_fecha_registro,
                UpdatedBy = (string?)userRecord.vc_usuario_actualizacion,
                UpdatedAt = (DateTime?)userRecord.dt_fecha_actualizacion,
                DeliveredParcelsCount = 1,
                UnreadMessagesCount = 2,
                UnreadNotificationsCount = 3
            };

            _currentToken = token;
            _isAuthenticated = true;
            OnAuthStateChanged?.Invoke();

            return AuthResult.Success(_currentUser, token);
        }
        catch (Exception ex)
        {
            return AuthResult.Failed($"Error de base de datos: {ex.Message}");
        }
    }

    public async Task<AuthResult> LoginWithMicrosoftAsync(string? microsoftEmail = null, string? microsoftTenantId = null)
    {
        if (!_authConfig.IsMicrosoftEnabled)
        {
            return AuthResult.Failed("La autenticación con Microsoft está deshabilitada en la configuración del sistema (appsettings.json).");
        }

        if (string.IsNullOrWhiteSpace(microsoftEmail))
        {
            return AuthResult.Failed("No se recibió una cuenta de correo válida de Microsoft.");
        }

        string emailToValidate = microsoftEmail.Trim();
        string? tenantToValidate = !string.IsNullOrWhiteSpace(microsoftTenantId)
            ? microsoftTenantId.Trim()
            : (!string.IsNullOrWhiteSpace(_authConfig.Microsoft.TenantId) ? _authConfig.Microsoft.TenantId.Trim() : null);

        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@vc_correo", emailToValidate, DbType.AnsiString);
            parameters.Add("@vc_tenant_id", tenantToValidate, DbType.AnsiString);

            var userRecord = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "security.sp_validar_usuario_microsoft",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // Regla de Negocio: Validar existencia, estado activo y coincidencia de Tenant ID
            if (userRecord == null)
            {
                return AuthResult.Failed($"Acceso denegado: El correo '{emailToValidate}' no está registrado, se encuentra inactivo, o su Tenant ID no coincide con la organización en security.usuario.");
            }

            int userId = (int)userRecord.in_usuario_id;
            string userEmail = (string)userRecord.vc_correo;
            string fullName = $"{userRecord.vc_nombre} {userRecord.vc_apellido}".Trim();
            string roleName = (string)(userRecord.vc_rol_nombre ?? "Administrador");
            string roleCode = (string)(userRecord.vc_rol_codigo ?? "admin");
            string? tenantId = (string?)userRecord.vc_tenant_id;

            var token = _jwtTokenService.GenerateToken(userId, userEmail, fullName, roleCode, tenantId);

            _currentUser = new UserProfile
            {
                Id = userId,
                Name = fullName,
                Email = userEmail,
                Role = roleName,
                RoleCode = roleCode,
                TenantId = tenantId,
                Location = userRecord.vc_ubicacion ?? "Sede Central",
                AvatarUrl = userRecord.vc_avatar_url ?? "/images/avatar.jpg",
                CreatedBy = (string?)userRecord.vc_usuario_creacion,
                CreatedAt = (DateTime?)userRecord.dt_fecha_registro,
                UpdatedBy = (string?)userRecord.vc_usuario_actualizacion,
                UpdatedAt = (DateTime?)userRecord.dt_fecha_actualizacion,
                DeliveredParcelsCount = 1,
                UnreadMessagesCount = 2,
                UnreadNotificationsCount = 3
            };

            _currentToken = token;
            _isAuthenticated = true;
            OnAuthStateChanged?.Invoke();

            return AuthResult.Success(_currentUser, token);
        }
        catch (Exception ex)
        {
            return AuthResult.Failed($"Error al validar con Microsoft y SQL Server: {ex.Message}");
        }
    }

    public async Task LogoutAsync()
    {
        await Task.Yield();
        _currentUser = new UserProfile();
        _currentToken = null;
        _isAuthenticated = false;
        OnAuthStateChanged?.Invoke();
    }
}
