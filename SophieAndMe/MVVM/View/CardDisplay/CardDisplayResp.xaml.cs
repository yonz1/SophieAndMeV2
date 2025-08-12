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


    
    



    //     InitializeComponent();
    //     string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
    //     urif = urif.Replace("\\", "/");
    //     System.Uri uri1 = new System.Uri(urif);
    //     WebViewAll.Source = uri1 as System.Uri;
    //     Setup(question, reponse, urlQuestion, urlReponse,mainVm);
    //
    // }
    //
    // private async void Setup(List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse,MainViewModel mainVm)
    // {
    //     await WebViewAll.EnsureCoreWebView2Async();
    //     WebViewAll.CoreWebView2.Settings.IsStatusBarEnabled = false;
    //     WebViewAll.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
    //     WebViewAll.CoreWebView2.NavigationCompleted += (sender, args) =>
    //     {
    //         WebViewAll.CoreWebView2.ExecuteScriptAsync("console.log('fonctionne');");
    //         var vm = new CardDisplayRespModel(js => WebViewAll.ExecuteScriptAsync(js),question,reponse,urlQuestion,urlReponse,mainVm);
    //         this.DataContext = vm;
    //     };
    // }
}