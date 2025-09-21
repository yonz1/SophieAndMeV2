using System.Windows;
using System.Windows.Controls;

namespace SophieAndMe.Core;

public class WebBrowserHelper
{
    public static readonly DependencyProperty BindableSourceProperty =
        DependencyProperty.RegisterAttached(
            "BindableSource",
            typeof(string),
            typeof(WebBrowserHelper),
            new UIPropertyMetadata(null, BindableSourceChanged));

    public static string GetBindableSource(DependencyObject obj)
    {
        return (string)obj.GetValue(BindableSourceProperty);
    }

    public static void SetBindableSource(DependencyObject obj, string value)
    {
        obj.SetValue(BindableSourceProperty, value);
    }

    private static void BindableSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var browser = d as WebBrowser;
        if (browser != null)
        {
            string uri = e.NewValue as string;
            if (!string.IsNullOrEmpty(uri))
                browser.Source = new Uri(uri);
            else
                browser.Source = null;
        }
    }
}