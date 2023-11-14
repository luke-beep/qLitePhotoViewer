using Windows.Storage;
using qLitePhotoViewer.Core.Models;

namespace qLitePhotoViewer.Contracts.Services;

public interface IPhotoDataService
{
    Task<IEnumerable<PhotoItem>> GetPhotoDataAsync();
    Task UploadPhotoToLocalFolderAsync(StorageFile photoFile);
    Task DeletePhotoAsync(StorageFile photoFile);
}