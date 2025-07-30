using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Web.WebView2.Core;
using SophieAndMe.Core;
using SophieAndMe.MVVM.ViewModel;
using UserControl = System.Windows.Controls.UserControl;

namespace SophieAndMe.MVVM.View;

public partial class CardDisplayImport : UserControl
{
    public CardDisplayImport(VCustomModel vm)
    {
        InitializeComponent();
        string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
        urif = urif.Replace("\\", "/");
        System.Uri uri1 = new System.Uri(urif);
        WebViewAll.Source = uri1 as System.Uri;
        Loaded += async (s, e) =>
        {
            await WebViewAll.EnsureCoreWebView2Async();

            WebViewAll.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
            DataContext = new CardDisplayImportModel(vm);
                
            // webviewall.CoreWebView2.OpenDevToolsWindow();
                
            WebViewAll.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                WebViewAll.CoreWebView2.ExecuteScriptAsync("console.log('fonctionne')");
                WeakReferenceMessenger.Default.Register<MediatorMarked.JsCallMessage>(this, (r, m) =>
                {
                    WebViewAll.CoreWebView2.ExecuteScriptAsync(m.Value);
                });
            };
        };
    }

    private void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            var msg = JsonSerializer.Deserialize<MediatorMarked.WebJsMessage>(e.WebMessageAsJson);
            if (msg != null)
            {
                WeakReferenceMessenger.Default.Send(new MediatorMarked.JsToAppMessage(msg.action, msg.question));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur JS: " + ex.Message);
        }
    }
}