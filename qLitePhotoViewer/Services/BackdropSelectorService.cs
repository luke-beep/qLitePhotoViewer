using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;
using qLitePhotoViewer.Contracts.Services;

namespace qLitePhotoViewer.Services;

public class BackdropSelectorService : IBackdropSelectorService
{
    private const string SettingsKey = "AppRequestedBackdrop";
    public event Action? BackdropChanged;

    public BackdropType Backdrop
    {
        get;
        private set;
    } = BackdropType.Mica;

    private readonly ILocalSettingsService _localSettingsService;
    public BackdropSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        Backdrop = await LoadBackdropFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetBackdropAsync(BackdropType type)
    {
        Backdrop = type;
        BackdropChanged?.Invoke();

        await SetRequestedBackDropAsync();
        await SaveBackdropInSettingsAsync(type);
    }

    public async Task SetRequestedBackDropAsync()
    {
        App.MainWindow.SystemBackdrop = Backdrop switch
        {
            BackdropType.Mica => new MicaBackdrop { Kind = MicaKind.Base },
            BackdropType.MicaAlt => new MicaBackdrop { Kind = MicaKind.BaseAlt },
            BackdropType.DesktopAcrylic => new DesktopAcrylicBackdrop(),
            _ => throw new ArgumentOutOfRangeException()
        };

        await Task.CompletedTask;
    }


    private async Task SaveBackdropInSettingsAsync(BackdropType type)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, type.ToString());
    }

    private async Task<BackdropType> LoadBackdropFromSettingsAsync()
    {
        var backDrop = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
        return Enum.TryParse(backDrop, out BackdropType backdropType) ? backdropType : BackdropType.Mica;
    }

    public enum BackdropType
    {
        Mica,
        MicaAlt,
        DesktopAcrylic
    }
}   