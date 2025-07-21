using System.Windows.Input;
using SophieAndMe.MVVM.View;
using SophieAndMe.Core;

namespace SophieAndMe.MVVM.ViewModel;

public class EndQuizzModel 
{
    
    public ICommand ViewResponse { get; }
    public ICommand RestartQuizz { get; }
    public ICommand ReturnSelection { get; }

    public EndQuizzModel(List<string> question, List<string> repnse, List<string> urlQuestion, List<string> urlRep)
    {
        ViewResponse = new RelayCommand(o =>
            NavigationService.Instance.Navigate(new CardDisplayResp(question, repnse, urlQuestion, urlRep)));
        RestartQuizz = new RelayCommand(o => NavigationService.Instance.Navigate(new QuizzLogic()));
        ReturnSelection = new RelayCommand(o => NavigationService.Instance.Navigate(new VQuizz()));
    }
}