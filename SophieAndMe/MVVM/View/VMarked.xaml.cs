using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Web.WebView2.Core;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour VMarked.xaml
    /// </summary>
    public partial class VMarked : UserControl
    {
        public VMarked()
        {
            InitializeComponent();
            string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
            urif = urif.Replace("\\", "/");
            System.Uri uri1 = new System.Uri(urif);
            webviewall.Source = uri1 as System.Uri;
            Loaded += async (s, e) =>
            {
                await webviewall.EnsureCoreWebView2Async();

                webviewall.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
                DataContext = new VMarkedModel();
                
                // webviewall.CoreWebView2.OpenDevToolsWindow();
                
                webviewall.CoreWebView2.NavigationCompleted += (sender, args) =>
                {
                    webviewall.CoreWebView2.ExecuteScriptAsync("console.log('fonctionne')");
                    WeakReferenceMessenger.Default.Register<VMarkedModel.JsCallMessage>(this, (r, m) =>
                    {
                        Console.WriteLine(m.Value);
                        webviewall.CoreWebView2.ExecuteScriptAsync(m.Value);
                    });
                };
            };
        }

        private void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var msg = JsonSerializer.Deserialize<WebJsMessage>(e.WebMessageAsJson);
                if (msg != null)
                {
                    WeakReferenceMessenger.Default.Send(new VMarkedModel.JsToAppMessage(msg.action, msg.id));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur JS: " + ex.Message);
            }
        }
        
        public class WebJsMessage
        {
            public string action { get; set; } = string.Empty;
            public string id { get; set; } = string.Empty;
        }
        
        
    }
}
