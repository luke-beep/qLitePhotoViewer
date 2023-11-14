using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using qLitePhotoViewer.Core.Models;

namespace qLitePhotoViewer.Views;

public sealed partial class ViewerDetailControl
{
    public PhotoItem PhotoDetails
    {
        get => (PhotoItem)GetValue(PhotoDetailsProperty);
        set => SetValue(PhotoDetailsProperty, value);
    }

    public static readonly DependencyProperty PhotoDetailsProperty = DependencyProperty.Register(
        nameof(PhotoDetails),
        typeof(PhotoItem),
        typeof(ViewerDetailControl),
        new PropertyMetadata(null));

    public ViewerDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ViewerDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}