using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Web.WebView2.Core;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour VMarked.xaml
    /// </summary>
    public partial class VMarked : UserControl
    {
        public VMarked(MainViewModel mainVm)
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
                DataContext = new VMarkedModel(mainVm);
                
                // webviewall.CoreWebView2.OpenDevToolsWindow();
                
                webviewall.CoreWebView2.NavigationCompleted += (sender, args) =>
                {
                    webviewall.CoreWebView2.ExecuteScriptAsync("console.log('fonctionne')");
                    WeakReferenceMessenger.Default.Register<MediatorMarked.JsCallMessage>(this, (r, m) =>
                    {
                        webviewall.CoreWebView2.ExecuteScriptAsync(m.Value);
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
}
