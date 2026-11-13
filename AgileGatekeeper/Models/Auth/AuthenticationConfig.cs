namespace AgileGatekeeper.Models.Auth;

public class AuthenticationConfig
{
    /// <summary>
    /// Modo de autenticación permitido: "Local", "Microsoft" o "Both".
    /// </summary>
    public string Provider { get; set; } = "Both";

    public MicrosoftAuthSettings Microsoft { get; set; } = new();

    public bool IsLocalEnabled => 
        string.Equals(Provider, "Local", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Provider, "Both", StringComparison.OrdinalIgnoreCase);

    public bool IsMicrosoftEnabled => 
        string.Equals(Provider, "Microsoft", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Provider, "Both", StringComparison.OrdinalIgnoreCase);
}

public class MicrosoftAuthSettings
{
    public string Instance { get; set; } = "https://login.microsoftonline.com/";
    public string Domain { get; set; } = string.Empty;
    public string TenantId { get; set; } = "common";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string CallbackPath { get; set; } = "/signin-oidc";
}
