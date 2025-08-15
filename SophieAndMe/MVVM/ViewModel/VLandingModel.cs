using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        (var metaNotes, var dataNotes) = DbInteraction.GetNotesWeb();
        Console.WriteLine("avancée");
        List<string> jsCode = WebviewInteraction.LandingNotes(metaNotes, dataNotes);
        foreach (var call in jsCode)
        { 
            Console.WriteLine(call);
            WeakReferenceMessenger.Default.Send(new MediatorLanding.JsCallMessage(call));    
        } 
        var quizzName = DbInteraction.GetQuizzWeb();
        jsCode = WebviewInteraction.LandingQuizz(quizzName);
        foreach (var call in jsCode)
        {
            Console.WriteLine(call);
            WeakReferenceMessenger.Default.Send(new MediatorLanding.JsCallMessage(call));    
        }
        
    }
    
    public event PropertyChangedEventHandler PropertyChanged = null!;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

