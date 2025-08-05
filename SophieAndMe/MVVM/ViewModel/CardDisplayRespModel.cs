using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.View;
using NavigationService = SophieAndMe.Core.NavigationService;

namespace SophieAndMe.MVVM.ViewModel;

public class CardDisplayRespModel 
{
    private List<string> _level;
    private List<string> _course;
    private List<string> _question;
    private List<string> _repnse;
    private List<string> _urlQuestion;
    private List<string> _urlRep;
    private List<string> _difficulty;
    private readonly Func<string, Task> _invokejs;
    private string _jscall = null!;
    private readonly MainViewModel _mainViewModel;
    public ICommand Back_quizz_Click { get; }
    private string _message = null!;
    public string Message
    {
        get => _message;set{        _message = value;        OnPropertyChanged();    }
    }

    public CardDisplayRespModel(List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse,MainViewModel mainVm)
    {
        _mainViewModel = mainVm;
        // WeakReferenceMessenger.Default.Send(new MediatorDisplayResp.JsCallMessage("ClearCard('value')"));
        Message = App.Current.Properties["nameindex"].ToString();
        var q = QuizzUtilities.Miseneformelist(question) ?? new List<string>();
        var r = QuizzUtilities.Miseneformelist(reponse) ?? new List<string>();
        var uq = QuizzUtilities.Miseneformelist(urlQuestion) ?? new List<string>();
        var ur = QuizzUtilities.Miseneformelist(urlReponse) ?? new List<string>();
        Console.WriteLine("Mise en forme : ");
        ShowCard(q, r, uq, ur);
        Back_quizz_Click = new RelayCommand(o =>
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

    private async void ShowCard(List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse)
    {
        Console.WriteLine("Clear appelée");
        Dictionary<string, string> dico = new Dictionary<string, string>();
        for (int i = 0; i < question.Count; i++)
        {
            dico["level"] = "";
            dico["course"] = "";
            dico["question"] = question[i];
            dico["repnse"] = reponse[i];
            dico["urlQuestion"] = urlQuestion[i];
            dico["urlRep"] = urlReponse[i];
            dico["difficulty"] = "";
            dico["len"] = question.Count.ToString();
            string jscode = JsonSerializer.Serialize(dico);
            Console.WriteLine("Jscode construit");
            WeakReferenceMessenger.Default.Send(new MediatorDisplayResp.JsCallMessage(jscode));
        }
    }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}