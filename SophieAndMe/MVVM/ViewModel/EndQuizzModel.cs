using System.Windows.Input;
using SophieAndMe.MVVM.View;
using SophieAndMe.Core;

namespace SophieAndMe.MVVM.ViewModel;

public class EndQuizzModel 
{
    
    public ICommand ViewResponse { get; }
    public ICommand RestartQuizz { get; }
    public ICommand ReturnSelection { get; }

    public EndQuizzModel(List<string> question, List<string> repnse, List<string> urlQuestion, List<string> urlRep,MainViewModel mainVm)
    {
        ViewResponse = new RelayCommand(o => NavigationService.Instance.Navigate(new CardDisplayResp(question, repnse, urlQuestion, urlRep,mainVm)));
        RestartQuizz = new RelayCommand(o => NavigationService.Instance.Navigate(new QuizzLogic(mainVm)));
        ReturnSelection = new RelayCommand(o =>
        {
            if (App.Current.Properties["nameindex"].ToString().Contains("Marked"))
            {
                NavigationService.Instance.Navigate(new VMarked(mainVm));    
            }
            else
            {
                NavigationService.Instance.Navigate(new VQuizz(mainVm));
            }
            
        });
    }
}