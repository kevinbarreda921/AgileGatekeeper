using Microsoft.JSInterop;

namespace AgileGatekeeper.Services.Theme;

public class ThemeService : IThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isDarkMode;
    private bool _initialized;

    public bool IsDarkMode => _isDarkMode;
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        try
        {
            _isDarkMode = await _jsRuntime.InvokeAsync<bool>("themeManager.isDark");
            _initialized = true;
            OnThemeChanged?.Invoke();
        }
        catch
        {
            // Ignorar durante prerenderizado estático
        }
    }

    public async Task ToggleThemeAsync()
    {
        await SetThemeAsync(!_isDarkMode);
    }

    public async Task SetThemeAsync(bool isDark)
    {
        _isDarkMode = isDark;
        try
        {
            await _jsRuntime.InvokeVoidAsync("themeManager.setTheme", isDark);
        }
        catch
        {
            // Ignorar si JS aún no está disponible
        }
        OnThemeChanged?.Invoke();
    }
}
