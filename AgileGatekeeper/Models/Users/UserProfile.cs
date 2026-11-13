namespace AgileGatekeeper.Models.Users;

public class UserProfile
{
    public int Id { get; set; } = 1;
    public string Name { get; set; } = "Administrador Agile";
    public string Email { get; set; } = "admin@agile.com";
    public string Role { get; set; } = "Administrador";
    public string RoleCode { get; set; } = "admin";
    public string? TenantId { get; set; } = "common";
    public string Location { get; set; } = "Sede Central";
    public string AvatarUrl { get; set; } = "/images/avatar.jpg";
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int DeliveredParcelsCount { get; set; } = 1;
    public int UnreadMessagesCount { get; set; } = 2;
    public int UnreadNotificationsCount { get; set; } = 3;
}
