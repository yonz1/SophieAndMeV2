using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SophieAndMe.MVVM.Model;
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
            Setup();
            this.DataContext = vm;
        }
        private async void Setup()
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
