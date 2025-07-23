
using System.Reflection;
using System.Windows.Controls;

using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour VLanding.xaml
    /// </summary>
    public partial class VLanding : UserControl
    {
        public VLanding()
        {
            InitializeComponent();
            string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\..\\..\\..\\HTML_Const\\Landing\\Landing.html";
            urif = urif.Replace("\\", "/");
            System.Uri uri1 = new System.Uri(urif);
            WebViewAll.Source = uri1 as System.Uri;
            var vm = new VLandingModel(js => WebViewAll.ExecuteScriptAsync(js));
            Start();
            this.DataContext = vm;
        }
        private async void Start()
        {           
            await WebViewAll.EnsureCoreWebView2Async();
            WebViewAll.CoreWebView2.Settings.IsStatusBarEnabled = false;
            WebViewAll.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            WebViewAll.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                WebViewAll.CoreWebView2.ExecuteScriptAsync("console.log('fonctionne');");
            };
        }
    }
}
