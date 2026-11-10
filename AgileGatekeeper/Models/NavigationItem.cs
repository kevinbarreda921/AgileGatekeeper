namespace AgileGatekeeper.Models;

public class NavigationItem
{
    public string Title { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int? Badge { get; set; }
    public bool MatchPrefix { get; set; }
    public List<NavigationItem>? SubItems { get; set; }
    public bool IsExpanded { get; set; }
}
