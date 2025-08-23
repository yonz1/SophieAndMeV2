using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.Windows;

public partial class ImportAdd : Window
{
    
    [DllImport("dwmapi.dll")]
    private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins pMarInset);

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
    private struct Margins
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    public ImportAdd(string question,string imgQuestion,string rep, string imgRep)
    {
        
        InitializeComponent();
        ImportAddViewModel vm =  new ImportAddViewModel(question,imgQuestion,rep,imgRep);
        this.DataContext = vm;
        var hwnd = new WindowInteropHelper(this).Handle;
        var margins = new Margins()
        {
            cxLeftWidth = 1,
            cxRightWidth = 1,
            cyTopHeight = 1,
            cyBottomHeight = 1
        };
        DwmExtendFrameIntoClientArea(hwnd, ref margins);
        int attrValue = 2; 
        DwmSetWindowAttribute(hwnd, 20, ref attrValue, sizeof(int));
        WeakReferenceMessenger.Default.Register<MediatorImportAdd.JsCallMessage>(this, (r, m) =>
        {
            WebView2.CoreWebView2.ExecuteScriptAsync("console.log('fonctionne');");
            Console.WriteLine(m.Value);
            WebView2.CoreWebView2.PostWebMessageAsJson(m.Value);
        });
        Loaded += async (s, e) =>
        {
            await WebView2.EnsureCoreWebView2Async();
            WebView2.CoreWebView2.OpenDevToolsWindow();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\HTML_Const\Quizz\quizz.html");
            var uri = new Uri(Path.GetFullPath(path));
            WebView2.Source = uri;
            WebView2.CoreWebView2.NavigationCompleted += (sender, args) => 
            {         

                Console.WriteLine("Charger");
            };
        };
        Unloaded += (s, e) =>
        {
            WeakReferenceMessenger.Default.Unregister<MediatorImportAdd.JsCallMessage>(this);
        };
    }
}