using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SophieAndMe.Core;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel;

public class LoginWindoowViewModel : ObservableRecipient,INotifyPropertyChanged
{
    private readonly IDataService _dataService;
    private readonly IWindowService _windowService;
    public ICommand Exit_Click { get; }
    public ICommand Connect_Click { get; }
    private string _username;
    public string Username
    {
        get =>  _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public LoginWindoowViewModel(IWindowService windowService)
    {
        Console.WriteLine("charger");
        _windowService = windowService;
        _dataService =  App.DataService;
        VerifyKeepAlive();
        Exit_Click = new RelayCommand(o => Application.Current.Shutdown());
        Connect_Click = new RelayCommand(o =>
        {
            ConnectLogic();
        });
    }

    private void ConnectLogic()
    {
        var username = _username;
        var  password = _password;
        Console.WriteLine(username);
        Console.WriteLine(password);
        password = EncryptShA(password);
        if (DbInteraction.VerifyUser(username, password))
        {
            FinishCompletion(username);
        }
    }
    private void VerifyKeepAlive()
    {
        (int keepalive, string username) = DbInteraction.VerifyKeepAlive(); 
        if (keepalive == 1)
        {
            FinishCompletion(username);
        }
    }
    private void FinishCompletion(string username)
    {
        App.DataService.CurrentUser = new User();
        _dataService.CurrentUser.Username = username;
        (_dataService.CurrentUser.Email, _dataService.CurrentUser.photo) = DbInteraction.RetrieveUserData(username);
        Console.WriteLine("charger");
        _windowService.CloseWindow<LoginWindoowViewModel>();
        _windowService.ShowWindow<MainViewModel>();
    }
    
    private string EncryptShA(string password)
    {
        var crypt = new SHA256Managed();
        var hash = new StringBuilder();
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
        foreach (byte theByte in crypto)
        {
            hash.Append(theByte.ToString("x2"));
        }
        return hash.ToString();
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) 
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}