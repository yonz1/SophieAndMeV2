using System.Text;
using System.Windows;
using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;

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
            App.Current.Properties["button_color"] = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x24, 0x24, 0x24));
            App.Current.Properties["button_color_text"] = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xF5, 0xF5, 0xF5));
            
         InitializeComponent();

            App.Current.Properties["html_back"] = "161717";
            App.Current.Properties["html_back_rep"] = "242424";
            App.Current.Properties["html_text"] = "#F5F5F5";
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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            string query = "";
            string Sourceuser = "Data Source=..\\..\\..\\Database\\user_value.db";
            System.Diagnostics.Debug.WriteLine(App.Current.Properties["Timer"]);
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(Sourceuser))
                {
                    c.Open();
                    query = "UPDATE DASH SET Time = " + App.Current.Properties["Timer"] + " where Date =  \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"";
                    System.Diagnostics.Debug.WriteLine(query);
                    using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("An error occured while saving your quizz");
            }

            // DirectoryInfo d = new DirectoryInfo(@"../../../HTML");
            // FileInfo[] Files = d.GetFiles();
            // string str = "";
            // foreach ( FileInfo f in Files )
            //     File.Delete(f.FullName);
            //
            //
            // System.Threading.Thread.Sleep(300);
            Application.Current.Shutdown();
        }






        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wParam, int wMsg, int lParam);


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Userbtncontent.Text = App.Current.Properties["username"] as string;
            // if (App.Current.Properties["photo"].ToString() == ".\\images\\Ryan-Gosling_0.jpg")
            // {
            //     App.Current.Properties["photo"] = "";
            //     ProfilePict.ImageSource = new BitmapImage(new Uri(App.Current.Properties["photo"].ToString(), UriKind.Relative));
            // }
            // else
            // {
            //     ProfilePict.ImageSource = new BitmapImage(new Uri(App.Current.Properties["photo"].ToString(), UriKind.Relative));
            // }
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