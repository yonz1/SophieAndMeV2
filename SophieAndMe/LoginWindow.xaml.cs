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
        if (DataContext is LoginWindoowViewModel vm2)
        {
            vm2.RequestClose += (_, __) => this.Close();
        }
    }
}