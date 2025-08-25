using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.View;

namespace SophieAndMe.MVVM.ViewModel;

public class VLandingModel : INotifyPropertyChanged
{
    private readonly Func<string, Task> _invokejs;
    private readonly List<string> _question;
    private readonly List<string> _repnse;
    private readonly List<string> _urlQuestion;
    private readonly List<string> _urlRep;
    private string _jscode = null!;
    private readonly MainViewModel _mainViewModel;
    private List<string> _meta = [];
    private List<string> _data = [];
    private readonly IDataService _dataService;
    private bool _isview;
    public bool  IsView
    {
        get => _isview;
        set { _isview = value;
            OnPropertyChanged();
        }
    }
    
    public VLandingModel(MainViewModel mainVm)
    {
        _mainViewModel = mainVm;
        _mainViewModel.CurrentMessage = "Acceuil";
        _dataService = App.DataService;
        IsView = true;
        WeakReferenceMessenger.Default.Register<MediatorLanding.JstoAppMessage>(this, (r, m) =>
        {
            var (action, matier, name, question, imgQuestion, rep, imgRep) = m.Value;
            string[] value = question.Split("-");
            Application.Current.Properties["nameindex"] = value[1];
            _dataService.QuizzId.IsAll = false;
            _dataService.QuizzId.options = "";
            _mainViewModel.CurrentMessage = value[1];
            _dataService.QuizzId.Matier = value[0];
            NavigationService.Instance.Navigate("MainContent",new QuizzLogic(mainVm));
        });
        
        Dictionary<string, string> dico = new Dictionary<string, string>();
        (var metaNotes, var dataNotes) = DbInteraction.GetNotesWeb();
        for (int i = 0; i < metaNotes.Count; i++)
        { 
            dico["Meta"] =  metaNotes[i];
            dico["Data"] =  dataNotes[i];
            dico["Position"] = "Notes";
            string jscode = JsonSerializer.Serialize(dico);
            WeakReferenceMessenger.Default.Send(new MediatorLanding.JsCallMessage(jscode)); 
        } 
        var quizzName = DbInteraction.GetQuizzWeb();
        List<string> Mat = new List<string>();
        foreach (var name in quizzName)
        {
            Mat.Add(DbInteraction.GetMat(name));
        }
        for (int i = 0; i < Mat.Count; i++)
        {
            dico["Meta"] =  Mat[i];
            dico["Data"] =  quizzName[i];
            dico["Position"] = "Quizz";
            string jscode = JsonSerializer.Serialize(dico);
            WeakReferenceMessenger.Default.Send(new MediatorLanding.JsCallMessage(jscode));    
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged = null!;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

