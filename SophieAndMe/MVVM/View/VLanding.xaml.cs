
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour VLanding.xaml
    /// </summary>
    public partial class VLanding : UserControl
    {
        public VLanding(MainViewModel mainVm)
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                await WebViewAll.EnsureCoreWebView2Async();
                string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Landing\\Landing.html";
                urif = urif.Replace("\\", "/");
                System.Uri uri1 = new System.Uri(urif);
                WebViewAll.Source = uri1 as System.Uri;
                WebViewAll.CoreWebView2.NavigationCompleted += (sender, args) =>
                {
                    var vm = new VLandingModel(mainVm);
                    this.DataContext = vm;
                    WeakReferenceMessenger.Default.Register<MediatorLanding.JsCallMessage>(this, (r, m) =>
                    {
                            Console.WriteLine(m.Value);
                            WebViewAll.CoreWebView2?.ExecuteScriptAsync(m.Value);
                    });
                };


            };

        }
    }
}
