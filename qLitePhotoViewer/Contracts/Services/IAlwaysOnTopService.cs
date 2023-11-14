using static qLitePhotoViewer.Services.AlwaysOnTopService;

namespace qLitePhotoViewer.Contracts.Services;

public interface IAlwaysOnTopService
{
    AlwaysOnTop IsAlwaysOnTop
    {
        get;
    }
    Task SetAlwaysOnTopAsync(AlwaysOnTop value);
    Task SetRequestedAlwaysOnTopAsync();
    event Action AlwaysOnTopChanged;
    Task InitializeAsync();

}