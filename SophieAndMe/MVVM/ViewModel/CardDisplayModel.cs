using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.View;
using NavigationService = SophieAndMe.Core.NavigationService;

namespace SophieAndMe.MVVM.ViewModel;

public class CardDisplayModel 
{
    private readonly Func<string, Task> _invokejs;
    private string _jscall = null!;
    public ICommand Back_quizz_Click { get; }
    private string _message = null!;
    public string Message
    {
        get => _message;set{        _message = value;        OnPropertyChanged();    }
    }

    public CardDisplayModel(Func<string, Task> invokeJs,List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse)
    {
        Message = App.Current.Properties["nameindex"].ToString();
        _invokejs = invokeJs;
        var q = QuizzUtilities.Miseneformelist(question) ?? new List<string>();
        var r = QuizzUtilities.Miseneformelist(reponse) ?? new List<string>();
        var uq = QuizzUtilities.Miseneformelist(urlQuestion) ?? new List<string>();
        var ur = QuizzUtilities.Miseneformelist(urlReponse) ?? new List<string>();
        Console.WriteLine("Mise en forme : ");
        ShowCard(q, r, uq, ur);
        Back_quizz_Click = new RelayCommand(o => NavigationService.Instance.Navigate(new VQuizz()));
    }

    private async void ShowCard(List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse)
    {
        try 
        {
            _jscall = WebviewInteraction.send_data_Card_Resp(question, reponse, urlQuestion, urlReponse);
            await _invokejs(_jscall);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}