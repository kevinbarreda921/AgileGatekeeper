namespace AgileGatekeeper.Services.Theme;

public interface IThemeService
{
    bool IsDarkMode { get; }
    event Action? OnThemeChanged;
    Task InitializeAsync();
    Task ToggleThemeAsync();
    Task SetThemeAsync(bool isDark);
}
