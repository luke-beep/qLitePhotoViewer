using Microsoft.UI.Xaml;

using qLitePhotoViewer.Contracts.Services;
using qLitePhotoViewer.ViewModels;

namespace qLitePhotoViewer.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(ViewerViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
