namespace qLitePhotoViewer.Contracts;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
