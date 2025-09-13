using System.Reflection;
using System.Windows.Controls;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour QuizzLogic.xaml
    /// </summary>
    public partial class QuizzLogic : UserControl
    {
        public QuizzLogic(MainViewModel mainVm)
        {
            InitializeComponent();
            Setup(mainVm);
        }

        private async void Setup(MainViewModel mainVm)
        {           
            await webviewquizz.EnsureCoreWebView2Async();
            string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\..\\..\\..\\HTML_Const\\Quizz\\quizz.html";
            urif = urif.Replace("\\", "/");
            System.Uri uri1 = new System.Uri(urif);
            webviewquizz.Source = uri1 as System.Uri;
            webviewquizz.CoreWebView2.Settings.IsStatusBarEnabled = false;
            webviewquizz.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webviewquizz.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                webviewquizz.CoreWebView2.ExecuteScriptAsync("console.log('fonctionne');");
                var vm = new QuizzLogicModel(js => webviewquizz.ExecuteScriptAsync(js),mainVm);
                this.DataContext = vm;
            };
        }
    }
}
