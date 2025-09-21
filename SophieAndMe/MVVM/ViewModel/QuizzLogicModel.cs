using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Input;
using FontAwesome.Sharp;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.View.CardDisplay;

namespace SophieAndMe.MVVM.ViewModel;

public class QuizzLogicModel  : INotifyPropertyChanged  
{
    private int _i = 0;
    private readonly Stopwatch _stopwatch;
    private readonly System.Timers.Timer _timer;
    // ########################################### Initialisation
    private readonly List<string> _question;
    private readonly List<string> _repnse;
    private readonly List<string> _urlQuestion;
    private readonly List<string> _urlRep;
    public List<string> MarkedQuestion;
    private readonly MainViewModel _mainViewModel;
    public ICommand Back_quizz_Click { get; }
    public ICommand Respaper { get; } = null!;
    public ICommand DirectResp { get; }
    private readonly List<string> _viewedQuestion = [];

    private readonly Func<string, Task> _invokejs;
    private string _time = "";
    private IDataService  _dataService;
    
    //########################################### ToggleButton 
    
    private bool _markedvalue;
    public bool MarkedValue
    {
        get =>  _markedvalue;
        set{    if (_markedvalue != value)    {  _markedvalue = value;OnPropertyChanged();
                OnMarkedValueChanged(value);
            }
        }
    }
    
    
    private bool _timervalue;
    public bool TimerValue
    {
        get => _timervalue;
        set  { if (_timervalue != value) {_timervalue = value;OnPropertyChanged(); } if (value)  { Message = _stopwatch.Elapsed.ToString(@"mm\:ss"); } else {  Message=string.Empty; } }
    }
    
    private bool _action;
    public bool Action
    {
        get => _action;
        set
        {
            if (_action != value)
            {
                _action = value;
                OnPropertyChanged();
            }

            if (_action)
            {
                ActionText = "Next";
                ShowResponse();
            }
            else
            {
                _i++;
                if (_i > _question.Count - 1)
                {
                    FinDeQuizz();
                }
                else
                {
                    ShowQuestion();
                    QuestionCounter = $"{_i + 1}/{_question.Count}";
                }

                ActionText = "Reponse";
                UnSetSilent();
                if (MarkedQuestion.Contains(_question[_i]))
                {
                    SetSilent();
                };
            }

        }
    }
    
    //###################################################### Text

    private string _actiontext = null!;
    public string ActionText
    {
        get => _actiontext;set{    _actiontext = value; OnPropertyChanged();    }
    }

    private string _questioncounter = null!;
    public string QuestionCounter
    {
        get => _questioncounter;set{    _questioncounter = value; OnPropertyChanged();}    
    }
    
    private string _message = null!;
    public string Message
    {
        get => _message;set{        _message = value;        OnPropertyChanged();    }
    }
    
    
    //####################################################### Icon

    private IconFont _currentIcon = IconFont.Regular;
    public IconFont CurrentIcon
    {
        get => _currentIcon;
        set
        {
            if (_currentIcon != value)
            {
                _currentIcon = value;
                OnPropertyChanged();
            }
        }
    }
    
    //###################################################### Function primaire
    public QuizzLogicModel(Func<string, Task> invokeJs,MainViewModel mainVm)
    {
        _dataService = App.DataService;
        _mainViewModel = mainVm;
        MarkedQuestion = DbInteraction.GetMarkedForQUizz(_dataService.QuizzId.Matier.ToString());
        _stopwatch = new Stopwatch();
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimerElapse;
        _stopwatch.Start();
        _timer.Start();
        _invokejs = invokeJs;
        Console.WriteLine("Lancée");
        switch (_dataService.QuizzId.options)
        {
            case "":
                Console.WriteLine("Normale");
                (_question,_repnse,_urlQuestion,_urlRep) = DbInteraction.Retrievequizz(_dataService.QuizzId.Nameindex.ToString(),"",mainVm);
                break;
            case "Progressif":
                Console.WriteLine("Prog");
                (_question,_repnse,_urlQuestion,_urlRep) = DbInteraction.RetrievequizzToProg();
                break;
        }
        ( _question, _repnse, _urlQuestion, _urlRep) = QuizzUtilities.Shuffle(_question, _repnse, _urlQuestion, _urlRep);
        ActionText = "Response";
        QuestionCounter = (_i + 1).ToString() + "/" + _question.Count; 
        if (MarkedQuestion.Contains(_question[_i]))
        {
            SetSilent();
        };
        ShowQuestion();
        Back_quizz_Click = new RelayCommand(o =>
        {
            if (_dataService.QuizzId.IsAll) { DeleteData(); }
            if (_dataService.QuizzId.Nameindex.ToString().Contains("Marked"))
            {
                NavigationService.Instance.Navigate("MainContent",new VMarked(mainVm));    
            }
            else
            {
                NavigationService.Instance.Navigate("MainContent",new VQuizz(mainVm));
            }
            
        });
        DirectResp = new RelayCommand(o =>
        {
            if (_dataService.QuizzId.IsAll) { DeleteData(); }
            NavigationService.Instance.Navigate("MainContent",
                new CardDisplayResp(_question, _repnse, _urlQuestion, _urlRep, mainVm));
        });
    }
    
    
    //############################################################# Fonction secondaire


    private void UnSetSilent()
    {
        _markedvalue = false;
        OnPropertyChanged(nameof(MarkedValue));
        CurrentIcon = IconFont.Regular;
    }

    private void SetSilent()
    {
        _markedvalue = true;
        OnPropertyChanged(nameof(MarkedValue));
        CurrentIcon = IconFont.Solid;
    }
    
    private void OnMarkedValueChanged(bool value)
    {
        if (value)
        {
            CurrentIcon = IconFont.Solid;
            DbInteraction.MarkData(_question[_i],_repnse[_i],_urlQuestion[_i],_urlRep[_i]);
        }
        else
        {
            CurrentIcon = IconFont.Regular;
            DbInteraction.UnMark(_question[_i]);
        }
    }
    
    private void OnTimerElapse (object? sender, ElapsedEventArgs e)
    {
        if (TimerValue)
        {
            App.Current.Dispatcher.Invoke(() =>  Message = _stopwatch.Elapsed.ToString(@"mm\:ss" ));
        }
    }

    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    
    private async void ShowQuestion()
    {
        try 
        {
            string jscall = WebviewInteraction.send_data("question",QuizzUtilities.Miseneformetext(_question[_i]),"",_urlQuestion[_i],"");
            await _invokejs(jscall);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    private async void ShowResponse()
    {
        try
        {
            string jscall = WebviewInteraction.send_data("reponse", QuizzUtilities.Miseneformetext(_question[_i]), QuizzUtilities.Miseneformetext(_repnse[_i]),_urlQuestion[_i],_urlRep[_i]);
            _viewedQuestion.Add(QuizzUtilities.Miseneformetext(_question[_i]));
            await _invokejs(jscall);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void DeleteData()
    {
        DbInteraction.DeleteQuestion(_viewedQuestion);
    }
    
    
    private void FinDeQuizz()
    {
        _viewedQuestion.Add(QuizzUtilities.Miseneformetext(_question[_i-1]));
        if (_dataService.QuizzId.IsAll) { DeleteData(); }
        DbInteraction.AddCompletion(App.Current.Properties["nameindex"].ToString(),_dataService.QuizzId.Matier.ToString());
        NavigationService.Instance.Navigate("MainContent",new EndQuizz(new EndQuizzModel(_question,_repnse,_urlQuestion,_urlRep,_mainViewModel)));
    }
    
}