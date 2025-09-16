using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;

namespace SophieAndMe;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
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


        public MainWindow()
        {
            InitializeComponent();
            NavigationService.Instance.Register("MainContent", view => MainContentControl.Content = view);
            MVVM.ViewModel.MainViewModel vm = new MVVM.ViewModel.MainViewModel();
            this.DataContext = vm;
            if (DataContext is MVVM.ViewModel.MainViewModel vm2)
            {
                vm2.RequestClose += (_, __) => Application.Current.Shutdown();
            }
        }
    

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        // private void Exit_Click(object sender, RoutedEventArgs e)
        // {
        //     string query = "";
        //     string Sourceuser = "Data Source=..\\..\\..\\Database\\user_value.db";
        //     System.Diagnostics.Debug.WriteLine(App.Current.Properties["Timer"]);
        //     Application.Current.Shutdown();
        // }
        
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wParam, int wMsg, int lParam);


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Userbtncontent.Text = App.Current.Properties["username"] as string;
            //Userbtncontent.Text = "Admin";
            App.Current.Properties["photo"] = "";
            if (App.Current.Properties["photo"].ToString() == "")
            {
                App.Current.Properties["photo"] = "../../../images/Ryan-Gosling_0.jpg";
                ProfilePict.ImageSource = new BitmapImage(new Uri(App.Current.Properties["photo"].ToString(), UriKind.Relative));
            }
            else
            {
                ProfilePict.ImageSource = new BitmapImage(new Uri(App.Current.Properties["photo"].ToString(), UriKind.Relative));
            }
            var hwnd = new WindowInteropHelper(this).Handle;
            var margins = new Margins()
            {
                cxLeftWidth = 1,
                cxRightWidth = 1,
                cyTopHeight = 1,
                cyBottomHeight = 1
            };
            DwmExtendFrameIntoClientArea(hwnd, ref margins);

            // Supprimer le coin blanc autour
            int attrValue = 2; // DWMWA_USE_IMMERSIVE_DARK_MODE
            DwmSetWindowAttribute(hwnd, 20, ref attrValue, sizeof(int));
        }
}