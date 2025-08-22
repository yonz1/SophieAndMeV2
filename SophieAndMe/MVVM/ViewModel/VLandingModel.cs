using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.MVVM.Model;

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
        IsView = true;
        Dictionary<string, string> dico = new Dictionary<string, string>();
        (var metaNotes, var dataNotes) = DbInteraction.GetNotesWeb();
        for (int i = 0; i < metaNotes.Count; i++)
        { 
            dico["Meta"] =  metaNotes[i];
            dico["Data"] =  dataNotes[i].ToString().Split(";")[0];
            dico["Notes"] = dataNotes[i].ToString().Split(";")[1];
            // dico["Notes"] = "";
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
            dico["Notes"] = "";
            dico["Position"] = "Quizz";
            string jscode = JsonSerializer.Serialize(dico);
            WeakReferenceMessenger.Default.Send(new MediatorLanding.JsCallMessage(jscode));    
        }
        
    }
    
    public event PropertyChangedEventHandler PropertyChanged = null!;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

