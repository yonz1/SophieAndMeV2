using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Web.WebView2.Core;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.ViewModel;
using UserControl = System.Windows.Controls.UserControl;

namespace SophieAndMe.MVVM.View.CardDisplay;

public partial class CardDisplayImport : UserControl
{
    public CardDisplayImport(VCustomModel Vm, string action)
    {
        InitializeComponent();
        string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
        urif = urif.Replace("\\", "/");
        System.Uri uri1 = new System.Uri(urif);
        WebViewAllCard.Source = uri1 as System.Uri;
        Loaded += async (s, e) =>
        {
            await WebViewAllCard.EnsureCoreWebView2Async();
            WebViewAllCard.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
            WebViewAllCard.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                this.DataContext = new CardDisplayImportModel(Vm,action);
                WeakReferenceMessenger.Default.Register<MediatorDisplayImport.JsCallMessage>(this, (r, m) =>
                {
                    Console.WriteLine("Bon trigger");
                    Console.WriteLine(m.Value);
                    WebViewAllCard.CoreWebView2.ExecuteScriptAsync(m.Value);
                });
            };
        };
    }

    private void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            var msgSoR = JsonSerializer.Deserialize<MediatorDisplayImport.WebJsMessage>(e.WebMessageAsJson);
            if (msgSoR != null)
            {
                WeakReferenceMessenger.Default.Send(new MediatorDisplayImport.JstoAppMessage(msgSoR.action, msgSoR.matier, msgSoR.name, msgSoR.question, msgSoR.imgQuestion, msgSoR.rep, msgSoR.imgRep));
            }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur JS: " + ex.Message);
        }
    }
    
}