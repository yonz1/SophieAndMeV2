using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Web.WebView2.Wpf;

namespace SophieAndMe.MVVM.Model;

public static class ContentControlHelper
{
    public static void ClearContentControl(ContentControl contentControl)
    {
        if (contentControl?.Content is FrameworkElement element)
        {
            if (element.DataContext is IDisposable disposableVm)
            {
                disposableVm.Dispose();
            }

            var webview = FindChild<WebView2>(element);
            if (webview != null)
            {
                try
                {
                    webview.CoreWebView2?.Stop();
                    webview.Dispose();
                }
                catch (Exception e)
                {
                }
                
            }
        }
        contentControl.Content = null;
    }

    private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
    {
        if (parent == null) return null;
        for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
            if (child is T foundChild)
            {
                return foundChild;
            }
            var childofchild = FindChild<T>(child);
            if (childofchild != null) return childofchild;
        }
        return null;
    }
}