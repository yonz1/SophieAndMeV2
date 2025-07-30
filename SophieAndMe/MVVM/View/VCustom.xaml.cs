using System.Reflection;
using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Web.WebView2.Core;
using SophieAndMe.Core;
using SophieAndMe.MVVM.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace SophieAndMe.MVVM.View;

public partial class VCustom : UserControl
{
    public VCustom(MainViewModel mainVm)
    {
        InitializeComponent();
        string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Custom\\Custom.html";
        urif = urif.Replace("\\", "/");
        System.Uri uri1 = new System.Uri(urif);
        WebViewCustom.Source = uri1 as System.Uri;
        urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
        urif = urif.Replace("\\", "/");
        uri1 = new System.Uri(urif);
        WebViewCard.Source = uri1 as System.Uri;
        Loaded += async (s, e) =>
        {
            await WebViewCustom.EnsureCoreWebView2Async();
            await WebViewCard.EnsureCoreWebView2Async();
            WebViewCustom.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
            WebViewCard.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
            WebViewCard.CoreWebView2.OpenDevToolsWindow();
            WebViewCustom.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                this.DataContext = new VCustomModel(mainVm);
                WeakReferenceMessenger.Default.Register<MediatorCustom.JsCallMessage>(this, (r, m) =>
                {
                    if (m.Value.Contains("Card"))
                    {
                        WebViewCard.CoreWebView2.ExecuteScriptAsync(m.Value);
                        
                    }
                    else if (m.Value.Contains("{\"level\":"))
                    {
                        WebViewCard.CoreWebView2.PostWebMessageAsJson(m.Value);
                    }
                    else
                    {
                        WebViewCustom.CoreWebView2.ExecuteScriptAsync(m.Value);
                    }
                });
            };
        };
    }

    private void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            var msgSoR = JsonSerializer.Deserialize<MediatorCustom.WebJsMessage>(e.WebMessageAsJson);
            if (msgSoR != null)
            {
                WeakReferenceMessenger.Default.Send(new MediatorCustom.JstoAppMessage(msgSoR.action, msgSoR.matier, msgSoR.name, msgSoR.question, msgSoR.imgQuestion, msgSoR.rep, msgSoR.imgRep));
            }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur JS: " + ex.Message);
        }
    }
}