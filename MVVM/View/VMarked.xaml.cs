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
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour VMarked.xaml
    /// </summary>
    public partial class VMarked : UserControl
    {
        public VMarked()
        {
            InitializeComponent();
            string urif = "file:///" + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\..\\..\\..\\HTML_Const\\Card\\Card.html";
            urif = urif.Replace("\\", "/");
            System.Uri uri1 = new System.Uri(urif);
            webviewall.Source = uri1 as System.Uri;
            Setup();
        }
        private async void Setup()
        {           
            await webviewall.EnsureCoreWebView2Async();
            webviewall.CoreWebView2.Settings.IsStatusBarEnabled = false;
            webviewall.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webviewall.CoreWebView2.NavigationCompleted += (sender, args) =>
            {
                var vm = new VMarkedModel(js => webviewall.ExecuteScriptAsync(js));
                this.DataContext = vm;
            };
        }
    }
}
