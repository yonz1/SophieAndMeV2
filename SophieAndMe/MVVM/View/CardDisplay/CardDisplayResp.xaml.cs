using System.IO;
using System.Reflection;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View.CardDisplay;

public partial class CardDisplayResp : UserControl
{
    private readonly string _jscode = null!;
    private bool _vmInitialized = false;

    public CardDisplayResp(List<string> question, List<string> reponse, List<string> urlQuestion,
        List<string> urlReponse, MainViewModel mainVm)
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<MediatorDisplayResp.JsCallMessage>(this, (r, m) =>
        {
            Console.WriteLine("Resp");
            if (m.Value.Contains("\"question\":"))
                WebViewAll.CoreWebView2?.PostWebMessageAsJson(m.Value);
            else
                WebViewAll.CoreWebView2?.ExecuteScriptAsync(m.Value);
        });

        Loaded += async (s, e) =>
        {
            await WebViewAll.EnsureCoreWebView2Async();
            WebViewAll.CoreWebView2.Settings.IsStatusBarEnabled = false;
            WebViewAll.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            WebViewAll.DefaultBackgroundColor = System.Drawing.Color.Transparent;
            string urif = "file:///" + Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
            urif = urif.Replace("\\", "/");
            WebViewAll.Source = new Uri(urif);

            WebViewAll.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                if (!_vmInitialized)
                {
                    this.DataContext = new CardDisplayRespModel(question, reponse, urlQuestion, urlReponse, mainVm);
                    _vmInitialized = true;
                }
            };
        };
    }
}