using Microsoft.UI.Xaml.Data;
using qLitePhotoViewer.Services;

namespace qLitePhotoViewer.Helpers;

public class BackdropTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not BackdropSelectorService.BackdropType backdrop)
        {
            return string.Empty;
        }

        return backdrop switch
        {
            BackdropSelectorService.BackdropType.Mica => "Mica",
            BackdropSelectorService.BackdropType.MicaAlt => "Mica Alt",
            BackdropSelectorService.BackdropType.DesktopAcrylic => "Acrylic",
            _ => "Mica"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}