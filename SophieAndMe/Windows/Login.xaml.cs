using SophieAndMe.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SophieAndMe.Windows
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            LoginViewModel  vm = new LoginViewModel();
            this.DataContext = vm;
            if (DataContext is LoginViewModel vm2)
            {
                vm2.RequestClose += (_, __) => this.Close();
            }
        }
    }
}
