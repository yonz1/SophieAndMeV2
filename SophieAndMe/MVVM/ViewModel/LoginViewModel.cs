using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SophieAndMe.Core;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using CommunityToolkit.Mvvm.ComponentModel;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel
{
    class LoginViewModel
    {
        public ICommand ExitCommand { get; }
        public ICommand MaximizeCommand { get; }

        public ICommand MinimizeCommand { get; }
        public event EventHandler? RequestClose;
        public LoginViewModel() 
        {
            ExitCommand = new RelayCommand(o => OnClose());
        }
        private void OnClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
