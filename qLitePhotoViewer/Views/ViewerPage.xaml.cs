using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using qLitePhotoViewer.ViewModels;

namespace qLitePhotoViewer.Views;

public sealed partial class ViewerPage : Page
{
    public ViewerViewModel ViewModel
    {
        get;
    }

    public ViewerPage()
    {
        ViewModel = App.GetService<ViewerViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
