using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel;

public class SettingsProfileViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ProfileUserInfo> UserValueSettings { get; set; }
    public ICommand EditPhoto {get; }
    public ICommand Confirm { get; }
    public ICommand Cancel { get; }
    private readonly MainViewModel _mainViewModel;


    
    private bool _isviewprofile;

    public bool IsViewProfile
    {
        get => _isviewprofile;
        set
        {
            _isviewprofile = value;
            OnPropertyChanged();
        }
    }
    
    private bool _isviewprofileedit;
    public bool IsViewProfileEdit
    {
        get =>  _isviewprofileedit;
        set
        {
            _isviewprofileedit = value;
            OnPropertyChanged();
        }
    }


    public SettingsProfileViewModel(MainViewModel viewModel)
    {
        _mainViewModel = viewModel;
        _mainViewModel.CurrentMessage = "Paramètres";
        UserValueSettings = new ObservableCollection<ProfileUserInfo>
        {
            new ProfileUserInfo { RowId = 0, UserInfo = "username", UserData = "Admin" },
            new ProfileUserInfo { RowId = 1, UserInfo = "E-mail", UserData = "admin@gmail.com" },
            new ProfileUserInfo { RowId = 2, UserInfo = "Password", UserData = "......." },
            new ProfileUserInfo { RowId = 3, UserInfo = "Groupe de kholle", UserData = "Groupe 8" },
            new ProfileUserInfo { RowId = 4, UserInfo = "Groupe de classe", UserData = "Groupe A" }
        };

        foreach (var subject in UserValueSettings)
        {
            subject.IsViewEdit = true;
        }
        foreach (var subject in UserValueSettings)
        {
            var localSubject = subject;
            subject.EditValue = new RelayCommand(param =>
            {
                foreach (var s in UserValueSettings)
                {
                    (s.IsView,s.IsViewEdit) = (false, true);
                }
                (localSubject.IsView,localSubject.IsViewEdit) = (true,false);
            });
            subject.CancelValue = new RelayCommand(param =>
            {
                foreach (var s in UserValueSettings)
                {
                    (s.IsView,s.IsViewEdit) = (false,true);
                }
            });
            subject.ConfirmValue = new RelayCommand(param =>
            {
                foreach (var s in UserValueSettings)
                {
                    (s.IsView,s.IsViewEdit) = (false,true);
                }
            });
        }
        EditPhoto = new RelayCommand(o =>
        {
            IsViewProfile = true;
            IsViewProfileEdit = false;
        });
        Confirm = new RelayCommand(o =>
        {
            IsViewProfile = false;
            IsViewProfileEdit = true;
        });
        Cancel = new RelayCommand(o =>
        {
            IsViewProfile = false;
            IsViewProfileEdit = true;
        });
        
        
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

