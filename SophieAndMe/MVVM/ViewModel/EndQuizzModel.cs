using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using SophieAndMe.MVVM.View;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View.CardDisplay;

namespace SophieAndMe.MVVM.ViewModel;

public class EndQuizzModel : INotifyPropertyChanged
{
    public IDataService _dataService;
    public ICommand ViewResponse { get; }
    public ICommand RestartQuizz { get; }
    public ICommand ReturnSelection { get; }
    private bool _isview;
    public bool  IsView
    {
        get => _isview;
        set { _isview = value;
            OnPropertyChanged();
        }
    }

    public EndQuizzModel(List<string> question, List<string> repnse, List<string> urlQuestion, List<string> urlRep,MainViewModel mainVm)
    {
        _dataService = App.DataService;
        IsView = !_dataService.QuizzId.IsAll;
        ViewResponse = new RelayCommand(o => NavigationService.Instance.Navigate("MainContent",new CardDisplayResp(question, repnse, urlQuestion, urlRep,mainVm)));
        RestartQuizz = new RelayCommand(o => NavigationService.Instance.Navigate("MainContent",new QuizzLogic(mainVm)));
        ReturnSelection = new RelayCommand(o =>
        {
            if (App.Current.Properties["nameindex"].ToString().Contains("Marked"))
            {
                NavigationService.Instance.Navigate("MainContent",new VMarked(mainVm));    
            }
            else
            {
                NavigationService.Instance.Navigate("MainContent",new VQuizz(mainVm));
            }
            
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}