using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using qLitePhotoViewer.Activation;
using qLitePhotoViewer.Contracts;
using qLitePhotoViewer.Contracts.Services;
using qLitePhotoViewer.Views;

namespace qLitePhotoViewer.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IBackdropSelectorService _backdropSelectorService;
    private readonly IAlwaysOnTopService _alwaysOnTopService;

    private UIElement? _shell;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, IThemeSelectorService themeSelectorService, IBackdropSelectorService backdropSelectorService, IAlwaysOnTopService alwaysOnTopService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
        _backdropSelectorService = backdropSelectorService;
        _alwaysOnTopService = alwaysOnTopService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        await InitializeAsync();

        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        await HandleActivationAsync(activationArgs);

        App.MainWindow.Activate();

        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await _backdropSelectorService.InitializeAsync().ConfigureAwait(false);
        await _alwaysOnTopService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await _alwaysOnTopService.SetRequestedAlwaysOnTopAsync();
        await _backdropSelectorService.SetRequestedBackDropAsync();
        await Task.CompletedTask;
    }
}
