using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using qLitePhotoViewer.Contracts.Services;
using qLitePhotoViewer.Contracts.ViewModels;
using qLitePhotoViewer.Core.Contracts.Services;
using qLitePhotoViewer.Core.Models;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace qLitePhotoViewer.ViewModels;

public partial class ViewerViewModel : ObservableRecipient, INavigationAware
{


    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private readonly IPhotoDataService _photoDataService;

    [ObservableProperty]
    private PhotoItem? _selected;

    public ICommand UploadPhotoCommand
    {
        get;
    }
    public ICommand DeletePhotoCommand
    {
        get;
    }

    public ObservableCollection<PhotoItem> PhotoItems { get;
    } = new();

    public ViewerViewModel(IPhotoDataService photoDataService)
    {
        _photoDataService = photoDataService;

        UploadPhotoCommand = new RelayCommand(async () => await UploadPhoto());
        DeletePhotoCommand = new RelayCommand(async () => await DeletePhoto());
    }

    public async void OnNavigatedTo(object parameter)
    {
        await RefreshPhotosAsync();
    }

    public async Task RefreshPhotosAsync()
    {
        PhotoItems.Clear();

        var data = await _photoDataService.GetPhotoDataAsync();
        foreach (var item in data)
        {
            PhotoItems.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public async Task UploadPhoto()
    {
        var picker = new FileOpenPicker();
        var hwnd = GetActiveWindow();

        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".png");

        InitializeWithWindow.Initialize(picker, hwnd);

        var files = await picker.PickMultipleFilesAsync();
        if (files is { Count: > 0 })
        {
            foreach (var photoFile in files)
            {
                await _photoDataService.UploadPhotoToLocalFolderAsync(photoFile);
            }
            await RefreshPhotosAsync();
        }
    }

    public async Task DeletePhoto()
    {
        if (Selected != null)
        {
            var file = await StorageFile.GetFileFromPathAsync(Selected.ImagePath);
            await _photoDataService.DeletePhotoAsync(file);
        }

        await RefreshPhotosAsync();
    }

    public void EnsureItemSelected()
    {
        Selected ??= PhotoItems.FirstOrDefault();
    }
}
