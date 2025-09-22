using System.Globalization;
using System.Windows.Data;

namespace SophieAndMe.Core;

public class BoolToHitTestVisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b)
            return b; // true = cliquable, false = non cliquable

        return true; // par défaut cliquable
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}