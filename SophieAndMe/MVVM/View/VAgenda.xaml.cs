using System.Windows.Controls;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View;

public partial class VAgenda : UserControl
{
    public VAgenda(MainViewModel mainVm )
    {
        
        InitializeComponent();
        Loaded += async (s, e) =>
        {
            await webviewall.EnsureCoreWebView2Async();
            webviewall.CoreWebView2.Settings.IsStatusBarEnabled = false;
            webviewall.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            this.DataContext = new VAgendaModel(mainVm);
        };
    }
}