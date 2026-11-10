namespace AgileGatekeeper.Models;

public class UserProfile
{
    public string Name { get; set; } = "Jack Ryad";
    public string Email { get; set; } = "jack.ryad@agilegatekeeper.io";
    public string Role { get; set; } = "Enterprise Administrator";
    public string Location { get; set; } = "New York, NY";
    public string AvatarUrl { get; set; } = "/images/avatar.jpg";
    public int DeliveredParcelsCount { get; set; } = 1;
    public int UnreadMessagesCount { get; set; } = 2;
    public int UnreadNotificationsCount { get; set; } = 3;
}
