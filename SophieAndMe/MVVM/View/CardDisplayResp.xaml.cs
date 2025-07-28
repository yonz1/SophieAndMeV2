using System.Reflection;
using System.Windows.Controls;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View;

public partial class CardDisplayResp : UserControl
{
    private readonly string _jscode = null!;

    public CardDisplayResp(List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse,MainViewModel mainVm)
    {
        InitializeComponent();
        string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
        urif = urif.Replace("\\", "/");
        System.Uri uri1 = new System.Uri(urif);
        WebViewAll.Source = uri1 as System.Uri;
        Setup(question, reponse, urlQuestion, urlReponse,mainVm);

    }

    private async void Setup(List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse,MainViewModel mainVm)
    {
        await WebViewAll.EnsureCoreWebView2Async();
        WebViewAll.CoreWebView2.Settings.IsStatusBarEnabled = false;
        WebViewAll.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
        WebViewAll.CoreWebView2.NavigationCompleted += (sender, args) =>
        {
            WebViewAll.CoreWebView2.ExecuteScriptAsync("console.log('fonctionne');");
            var vm = new CardDisplayModel(js => WebViewAll.ExecuteScriptAsync(js),question,reponse,urlQuestion,urlReponse,mainVm);
            this.DataContext = vm;
        };
    }
}