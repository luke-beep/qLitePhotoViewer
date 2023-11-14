using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using qLitePhotoViewer.Contracts.Services;
using qLitePhotoViewer.Helpers;

using Windows.ApplicationModel;
using Windows.System;
using static qLitePhotoViewer.Services.AlwaysOnTopService;
using static qLitePhotoViewer.Services.BackdropSelectorService;

namespace qLitePhotoViewer.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly Uri _githubUri = new("https://github.com/luke-beep/Velocity");
    private readonly Uri _licenseUri = new("https://github.com/luke-beep/Velocity/blob/master/LICENSE");
    private readonly Uri _issueUri = new("https://github.com/luke-beep/Velocity/issues");

    private readonly IThemeSelectorService _themeSelectorService;
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IBackdropSelectorService _backdropSelectorService;
    private readonly IAlwaysOnTopService _alwaysOnTopService;

    public BackdropType Mica => BackdropType.Mica;
    public BackdropType MicaAlt => BackdropType.MicaAlt;
    public BackdropType DesktopAcrylic => BackdropType.DesktopAcrylic;

    public AlwaysOnTop AlwaysOnTopEnabled => AlwaysOnTop.Enabled;
    public AlwaysOnTop AlwaysOnTopDisabled => AlwaysOnTop.Disabled;

    [ObservableProperty] private ElementTheme _elementTheme;
    [ObservableProperty] private BackdropType _backDrop;
    [ObservableProperty] private AlwaysOnTop _alwaysOnTop;
    [ObservableProperty] private string _versionDescription;

    public ICommand SwitchThemeCommand
    {
        get;
    }
    public ICommand SwitchBackdropCommand
    {
        get;
    }
    public ICommand SwitchAlwaysOnTopCommand
    {
        get;
    }
    public ICommand OpenSettingsFolderCommand
    {
        get;
    }
    public ICommand OpenGitHubCommand
    {
        get;
    }

    public ICommand OpenLicenseCommand
    {
        get;
    }

    public ICommand OpenIssueCommand
    {
        get;
    }

    public string CurrentTheme => _themeSelectorService.Theme.ToString();
    public string CurrentBackdrop => _backdropSelectorService.Backdrop.ToString();
    public string CurrentAlwaysOnTop => _alwaysOnTopService.IsAlwaysOnTop.ToString();

    public SettingsViewModel(IThemeSelectorService themeSelectorService, ILocalSettingsService localSettingsService,
        IBackdropSelectorService backdropSelectorService, IAlwaysOnTopService alwaysOnTopService)
    {
        _themeSelectorService = themeSelectorService;
        _backdropSelectorService = backdropSelectorService;
        _alwaysOnTopService = alwaysOnTopService;
        _localSettingsService = localSettingsService;

        _backDrop = _backdropSelectorService.Backdrop;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        _themeSelectorService.ThemeChanged += () => OnPropertyChanged(nameof(CurrentTheme));
        _themeSelectorService.ThemeChanged += () => OnPropertyChanged(nameof(ThemeGlyph));
        _backdropSelectorService.BackdropChanged += () => OnPropertyChanged(nameof(CurrentBackdrop));
        _alwaysOnTopService.AlwaysOnTopChanged += () => OnPropertyChanged(nameof(CurrentAlwaysOnTop));

        SwitchThemeCommand = new RelayCommand<ElementTheme>(ChangeTheme);
        SwitchBackdropCommand = new RelayCommand<BackdropType>(ChangeBackdrop);
        SwitchAlwaysOnTopCommand = new RelayCommand<AlwaysOnTop>(ChangeAlwaysOnTop);
        OpenSettingsFolderCommand = new RelayCommand(OpenSettings);
        OpenGitHubCommand = new RelayCommand(OpenGitHub);
        OpenLicenseCommand = new RelayCommand(OpenLicense);
        OpenIssueCommand = new RelayCommand(OpenIssue);
    }

    private async void ChangeTheme(ElementTheme theme)
    {
        if (ElementTheme == theme)
        {
            return;
        }

        ElementTheme = theme;
        await _themeSelectorService.SetThemeAsync(theme);
    }
    public string ThemeGlyph =>
        ElementTheme switch
        {
            ElementTheme.Light => "\uE793",
            ElementTheme.Dark => "\uE708",
            ElementTheme.Default => "\uE713",
            _ => "\uE713"
        };
    private async void ChangeBackdrop(BackdropType backdrop)
    {
        if (BackDrop == backdrop)
        {
            return;
        }

        await _backdropSelectorService.SetBackdropAsync(backdrop);
        BackDrop = backdrop;
    }

    private async void ChangeAlwaysOnTop(AlwaysOnTop param)
    {
        if (AlwaysOnTop == param)
        {
            return;
        }
        await _alwaysOnTopService.SetAlwaysOnTopAsync(param);
        AlwaysOnTop = param;
    }

    private async void OpenGitHub() => await Launcher.LaunchUriAsync(_githubUri);
    private async void OpenLicense() => await Launcher.LaunchUriAsync(_licenseUri);
    private async void OpenIssue() => await Launcher.LaunchUriAsync(_issueUri);
    private async void OpenSettings() => await _localSettingsService.OpenSettings();

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMsix)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build,
                packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
