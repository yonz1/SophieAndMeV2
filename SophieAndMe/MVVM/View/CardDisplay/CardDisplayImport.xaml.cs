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
    private CardDisplayImportModel _model;
    public CardDisplayImport(VCustomModel Vm, string action)
    {
        InitializeComponent();

        Loaded += async (s, e) =>
        {
            await WebViewAllCard.EnsureCoreWebView2Async();
            WebViewAllCard.CoreWebView2.Settings.IsStatusBarEnabled = false;
            WebViewAllCard.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            WebViewAllCard.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
            string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
            urif = urif.Replace("\\", "/");
            System.Uri uri1 = new System.Uri(urif);
            WebViewAllCard.Source = uri1 as System.Uri;
            WebViewAllCard.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                // _model = new CardDisplayImportModel(Vm, action);
                // this.DataContext = _model;
                Console.WriteLine(WebViewAllCard.Source);
                WeakReferenceMessenger.Default.Register<MediatorDisplayImport.JsCallImportMessage>(this, (r, m) =>
                {
                    Console.WriteLine("import");
                    if (m.Value.Contains("{\"level\":"))
                    {
                        WebViewAllCard.CoreWebView2.PostWebMessageAsJson(m.Value);
                    }
                    else
                    {
                        Console.WriteLine("import2");
                        WebViewAllCard.CoreWebView2.ExecuteScriptAsync(m.Value);   
                    }
                });
                _model = new CardDisplayImportModel(Vm, action);
                this.DataContext = _model;

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
                WeakReferenceMessenger.Default.Send(new MediatorDisplayImport.JstoAppMessageImport(msgSoR.action, msgSoR.matier, msgSoR.name, msgSoR.question, msgSoR.imgQuestion, msgSoR.rep, msgSoR.imgRep));
            }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur JS - Import: " + ex.Message);
        }
    }
    
}