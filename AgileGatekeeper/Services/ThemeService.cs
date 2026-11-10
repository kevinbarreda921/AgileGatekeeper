namespace AgileGatekeeper.Services;

using Microsoft.JSInterop;

public class ThemeService : IThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isDarkMode = false;
    private bool _initialized = false;

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
            // Silently fallback if JS is not available during prerendering
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
            // Ignore during prerender
        }
        OnThemeChanged?.Invoke();
    }
}
