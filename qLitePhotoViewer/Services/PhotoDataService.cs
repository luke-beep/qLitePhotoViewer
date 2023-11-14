using Windows.Storage;
using ABI.Microsoft.UI.Xaml.Controls;
using qLitePhotoViewer.Contracts.Services;
using qLitePhotoViewer.Core.Models;
using qLitePhotoViewer.Models;

namespace qLitePhotoViewer.Services;

public class PhotoDataService : IPhotoDataService
{
    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private const string DefaultApplicationDataFolder = "qLitePhotoViewer\\ApplicationData\\Photos";
    

    public async Task<IEnumerable<PhotoItem>> GetPhotoDataAsync()
    {
        var applicationDataFolder = Path.Combine(_localApplicationData, DefaultApplicationDataFolder);
        var photoItems = new List<PhotoItem>();
        if(!Directory.Exists(applicationDataFolder))
        {
            Directory.CreateDirectory(applicationDataFolder);
        }
        var photoFolder = await StorageFolder.GetFolderFromPathAsync(applicationDataFolder);
        var photoFiles = await photoFolder.GetFilesAsync();

        foreach (var file in photoFiles)
        {
            var properties = await file.GetBasicPropertiesAsync();
            var photoItem = new PhotoItem
            {
                FileName = file.Name,
                ImagePath = file.Path,
                FileSize = (long)properties.Size
            };

            photoItems.Add(photoItem);
        }

        return photoItems;
    }

    public async Task UploadPhotoToLocalFolderAsync(StorageFile photoFile)
    {
        var applicationDataFolder = Path.Combine(_localApplicationData, DefaultApplicationDataFolder);

        if (!Directory.Exists(applicationDataFolder))
        {
            Directory.CreateDirectory(applicationDataFolder);
        }

        await photoFile.CopyAsync(await StorageFolder.GetFolderFromPathAsync(applicationDataFolder));
    }

    public async Task DeletePhotoAsync(StorageFile photoFile)
    {
        var applicationDataFolder = Path.Combine(_localApplicationData, DefaultApplicationDataFolder);
        if(File.Exists(Path.Combine(applicationDataFolder, photoFile.Name)))
        {
            await photoFile.DeleteAsync();
        }
    }
}