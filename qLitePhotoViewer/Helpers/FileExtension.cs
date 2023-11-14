using Windows.Storage;
using Windows.System;

namespace qLitePhotoViewer.Helpers;

public static class FileExtension
{
    public static async Task OpenFolder(string path)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(path);
        if (folder != null)
        {
            await Launcher.LaunchFolderAsync(folder);
        }
    }
}