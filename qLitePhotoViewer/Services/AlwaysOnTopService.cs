using qLitePhotoViewer.Contracts.Services;

namespace qLitePhotoViewer.Services;

public class AlwaysOnTopService : IAlwaysOnTopService
{
    private const string SettingsKey = "AppAlwaysOnTop";
    public event Action? AlwaysOnTopChanged;

    public AlwaysOnTop IsAlwaysOnTop
    {
        get;
        private set;
    }

    private readonly ILocalSettingsService _localSettingsService;
    public AlwaysOnTopService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        IsAlwaysOnTop = await LoadAlwaysOnTopFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetAlwaysOnTopAsync(AlwaysOnTop value)
    {
        IsAlwaysOnTop = value;
        AlwaysOnTopChanged?.Invoke();

        await SetRequestedAlwaysOnTopAsync();
        await SaveAlwaysOnTopInSettings(value);
    }

    public async Task SetRequestedAlwaysOnTopAsync()
    {
        switch (IsAlwaysOnTop)
        {
            case AlwaysOnTop.Enabled:
                App.MainWindow.SetIsAlwaysOnTop(true);
                break;
            case AlwaysOnTop.Disabled:
                App.MainWindow.SetIsAlwaysOnTop(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        await Task.CompletedTask;
    }

    private async Task SaveAlwaysOnTopInSettings(AlwaysOnTop value)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, value.ToString());
    }

    private async Task<AlwaysOnTop> LoadAlwaysOnTopFromSettingsAsync()
    {
        var alwaysOnTop = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        return Enum.TryParse(alwaysOnTop, out AlwaysOnTop alwaysOnTopResult) ? alwaysOnTopResult : AlwaysOnTop.Disabled;
    }

    public enum AlwaysOnTop
    {
        Enabled,
        Disabled
    }
}