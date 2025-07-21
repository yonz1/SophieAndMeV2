using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using FontAwesome.Sharp;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel;

public class QuizzLogicModel  : INotifyPropertyChanged  
{
    private int _i = 0;
    private readonly Stopwatch _stopwatch;
    private readonly System.Timers.Timer _timer;
    // ########################################### Initialisation
    private readonly List<string> _id;
    private readonly List<string> _question;
    private readonly List<string> _repnse;
    private readonly List<string> _urlQuestion;
    private readonly List<string> _urlRep;
    private readonly List<string> _difficulty;
    public ICommand Back_quizz_Click { get; }
    public ICommand Respaper { get; } = null!;
    public ICommand DirectResp { get; }

    private readonly Func<string, Task> _invokejs;
    private string _time = "";
    
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
                if (_i > _id.Count - 1)
                {
                    FinDeQuizz();
                }
                else
                {
                    ShowQuestion();
                    QuestionCounter = $"{_i + 1}/{_id.Count}";
                }

                ActionText = "Reponse";
                UnSetSilent();
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
    public QuizzLogicModel(Func<string, Task> invokeJs)
    {
        Console.WriteLine("Quizz Logic Loaded");
        _stopwatch = new Stopwatch();
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimerElapse;
        _stopwatch.Start();
        _timer.Start();
        _invokejs = invokeJs;
        (_id,_question,_repnse,_urlQuestion,_urlRep,_difficulty) = DBInteraction.Retrievequizz(App.Current.Properties["nameindex"].ToString());
        (_id, _question, _repnse, _urlQuestion, _urlRep, _difficulty) = QuizzUtilities.Shuffle(_id, _question, _repnse, _urlQuestion, _urlRep, _difficulty);
        ActionText = "Response";
        QuestionCounter = (_i + 1).ToString() + "/" + _id.Count; 
        ShowQuestion();
        Back_quizz_Click = new RelayCommand(o => NavigationService.Instance.Navigate(new VQuizz(new VQuizzModel())));
        DirectResp = new RelayCommand(o => NavigationService.Instance.Navigate(new CardDisplay(_question,_repnse,_urlQuestion,_urlRep)));
    }
    
    
    //############################################################# Fonction secondaire


    private void UnSetSilent()
    {
        _markedvalue = false;
        OnPropertyChanged(nameof(MarkedValue));
        CurrentIcon = IconFont.Regular;
    }
    
    private void OnMarkedValueChanged(bool value)
    {
        if (value)
        {
            CurrentIcon = IconFont.Solid;
            DBInteraction.MarkData(_question[_i],_repnse[_i],_urlQuestion[_i],_urlRep[_i]);
            Console.WriteLine("Marque");
        }
        else
        {
            CurrentIcon = IconFont.Regular;
            DBInteraction.UnMark(_question[_i]);
            Console.WriteLine("Supprime");
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
            await _invokejs(jscall);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    private void MarkedChecked()
    {
        
    }
    
    private void FinDeQuizz()
    {
        NavigationService.Instance.Navigate(new EndQuizz(new EndQuizzModel(_question,_repnse,_urlQuestion,_urlRep)));
    }
    
}