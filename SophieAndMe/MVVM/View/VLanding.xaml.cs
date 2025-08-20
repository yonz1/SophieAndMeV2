
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    public partial class VLanding : UserControl
    {
        public VLanding(MainViewModel mainVm)
        {
            InitializeComponent();
            
            WeakReferenceMessenger.Default.Register<MediatorLanding.JsCallMessage>(this, (r, m) =>
            {
                WebViewAll.CoreWebView2.PostWebMessageAsJson(m.Value);
            });
            Loaded += async (s, e) =>
            {
                await WebViewAll.EnsureCoreWebView2Async();
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\HTML_Const\Landing\Landing.html");
                var uri = new Uri(Path.GetFullPath(path));
                WebViewAll.Source = uri;
                WebViewAll.CoreWebView2.NavigationCompleted += (sender, args) => {             var vm = new VLandingModel(mainVm);
                    this.DataContext = vm;};
            };
            
            

            
            Unloaded += (s, e) =>
            {
                WeakReferenceMessenger.Default.Unregister<MediatorLanding.JsCallMessage>(this);
            };
        }
    }
}
