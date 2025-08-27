using System.Windows;

namespace SophieAndMe.Core;

public class DialogCloser
{
    public static readonly DependencyProperty DialogResultProperty =
        DependencyProperty.RegisterAttached(
            "DialogResult",
            typeof(bool?),
            typeof(DialogCloser),
            new PropertyMetadata(null, OnDialogResultChanged));

    private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window && e.NewValue is bool result)
        {
            window.DialogResult = result;
        }
    }

    public static void SetDialogResult(Window target, bool? value)
        => target.SetValue(DialogResultProperty, value);

    public static bool? GetDialogResult(Window target)
        => (bool?)target.GetValue(DialogResultProperty);
}