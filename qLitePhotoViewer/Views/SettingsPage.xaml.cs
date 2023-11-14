using Microsoft.UI.Xaml.Controls;

using qLitePhotoViewer.ViewModels;

namespace qLitePhotoViewer.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }
}
