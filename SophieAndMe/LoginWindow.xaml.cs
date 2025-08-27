using System.Windows;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        LoginWindoowViewModel vm = new LoginWindoowViewModel();
        this.DataContext = vm;
        Loaded += LoginWindow_Loaded;
    }
    private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ICloseWindows vm)
        {
            vm.Close += () =>
            {
                this.Close();
            };
        }
    }
}