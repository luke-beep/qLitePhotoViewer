using Microsoft.UI.Xaml;

namespace qLitePhotoViewer.Contracts.Services;

public interface IThemeSelectorService
{
    ElementTheme Theme
    {
        get;
    }
    event Action ThemeChanged;
    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();
}
