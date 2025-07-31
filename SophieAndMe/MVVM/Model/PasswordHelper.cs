using System.Windows;
using System.Windows.Controls;

namespace SophieAndMe.MVVM.Model;

public static class PasswordHelper
{
    public static readonly DependencyProperty BoundPassword =
        DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordHelper),
            new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

    public static string GetBoundPassword(DependencyObject obj) =>
        (string)obj.GetValue(BoundPassword);

    public static void SetBoundPassword(DependencyObject obj, string value) =>
        obj.SetValue(BoundPassword, value);

    private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PasswordBox passwordBox)
        {
            passwordBox.PasswordChanged -= PasswordChanged;
            passwordBox.Password = e.NewValue?.ToString() ?? string.Empty;
            passwordBox.PasswordChanged += PasswordChanged;
        }
    }

    private static void PasswordChanged(object sender, RoutedEventArgs e)
    {
        var passwordBox = sender as PasswordBox;
        SetBoundPassword(passwordBox, passwordBox.Password);
    }
}