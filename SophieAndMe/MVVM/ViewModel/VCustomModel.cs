using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;


namespace SophieAndMe.MVVM.ViewModel;

public class VCustomModel : ObservableRecipient, INotifyPropertyChanged
{
    public ICommand Create { get; }
    public ICommand Created { get; }
    public ICommand Import { get; }
    public RelayCommand ChoisirMatierCommand { get; }
    public RelayCommand Return { get; }
    public RelayCommand Advance { get; }
    
    private readonly List<string> _mat = ["Physique", "Mathématiques", "Français", "Anglais", "Erreurs", "SI"];
    public ObservableCollection<string> Noms { get; set; } = [];
    private readonly MainViewModel _mainViewModel;
    public ObservableCollection<string> Matier { get; set; } = new ();
    private List<string> _level;
    private List<string> _course;
    private List<string> _question;
    private List<string> _repnse;
    private List<string> _urlQuestion;
    private List<string> _urlRep;
    private List<string> _difficulty;
    public List<Dictionary<string, string>> _all;
    private bool _isview;
    public bool  IsView
    {
        get => _isview;
        set { _isview = value;
            OnPropertyChanged();
        }
    }
    private bool _isviewcard;
    public bool  IsViewCard
    {
        get => _isviewcard;
        set { _isviewcard = value;
            OnPropertyChanged();
        }
    }
    
    private bool _isviewreturn;
    public bool IsViewReturn
    {
        get => _isviewreturn;
        set { _isviewreturn = value; OnPropertyChanged(); }
    }
    
    
    
    public VCustomModel(MainViewModel mainVm)
    {
        
        IsActive = true;
        _mainViewModel = mainVm;
        WeakReferenceMessenger.Default.Register<MediatorCustom.JstoAppMessage>(this, (r, m) =>
        {
            var (action, matier, name, question, imgQuestion, rep, imgRep) = m.Value;
            Console.WriteLine(m.Value);
            question = question.Replace("\\large", "").Replace("\\(", "$").Replace("\\)", "$");
            rep = rep.Replace("\\large", "").Replace("\\(", "$").Replace("\\)", "$");
            switch (action)
            {
                case "Delete":
                    DBInteraction.DeleteCreated(question);
                    break;
                case "save":
                    DBInteraction.SaveQuizz(matier,name,question,imgQuestion,rep,imgRep);
                    break;
                case "edit":
                    App.Current.Properties["old_quest"] = question;
                    (matier, name, question, rep, imgQuestion, imgRep) = DBInteraction.SearchQuizzCreated(question);
                    var jscode = WebviewInteraction.EdtiQuizz(matier,name,question,imgQuestion,rep,imgRep);
                    WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
                    ClearLogic(true,false,false);
                    break;
                case "Replace":
                    DBInteraction.ReplaceQuizz(matier,name,question,imgQuestion,rep,imgRep);
                    var data = DBInteraction.GetAllName();
                    var jscall = WebviewInteraction.Initcustom(data, "Add");
                    WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscall));
                    break;
            }
        });

        Return = new RelayCommand(o =>
        {
            switch (App.Current.Properties["old"])
            {
                case "FirstLayer":
                    App.Current.Properties["old"] = "CreatedLogic";
                    FirstLayer(App.Current.Properties["matier"]);
                    break;
                case "CreatedLogic":
                    CreatedLogic();
                    break;
            }
        });
        Create = new RelayCommand(o =>
        {
            ClearLogic(true,false,false);
            var data = DBInteraction.GetAllName();
            var jscode = WebviewInteraction.Initcustom(data, "Add");
            Console.WriteLine(jscode);
            WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        });
        Created = new RelayCommand(o =>
        {
            CreatedLogic();
        });

        Import = new RelayCommand(o =>
        {
            ShowLogic();
        });
        
        ChoisirMatierCommand = new RelayCommand(matier =>
        {
            ClearLogic(false,false,true);
            Console.WriteLine(matier);
            if (_mat.Contains(matier))
            {
                App.Current.Properties["old"] = "CreatedLogic";
                FirstLayer(matier);
            }

            else
            {
                App.Current.Properties["old"] = "FirstLayer";
                SecondeLayer(matier);
            }
        });
    }


    public void ShowLogic()
    {
        
        (_level, _course, _question, _urlQuestion, _repnse, _urlRep, _difficulty) = DBInteraction.GetAllPublic();
        // var jscode = WebviewInteraction.send_data_Card_Import(_level, _course,QuizzUtilities.Miseneformelist(_question),_urlQuestion,QuizzUtilities.Miseneformelist(_repnse),_urlRep,_difficulty);
        WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage("ClearCard()"));
        Dictionary<string, string> dico = new Dictionary<string, string>();
        for (int i = 0; i < _level.Count; i++)
        {
            dico["level"] = _level[i];
            dico["course"] = _course[i];
            dico["question"] = _question[i];
            dico["repnse"] = _repnse[i];
            dico["urlQuestion"] = _urlQuestion[i];
            dico["urlRep"] = _urlRep[i];
            dico["difficulty"] = _difficulty[i];
            string jscode = JsonSerializer.Serialize(dico);
            Console.WriteLine(jscode);
            WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        }

        // WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        ClearLogic(false,true,true);
    }
    
    public  void FirstLayer(object matier)
    {
        ClearLogic(false,false,true);
        App.Current.Properties["matier"] = matier;
        var name = DBInteraction.GetNameCreated(matier.ToString());
        foreach (var value in name) { Matier.Add(value);} 
    }

    public void SecondeLayer(object matier)
    {
        App.Current.Properties["nameindex"] = matier;
        (_question, _repnse, _urlQuestion,_urlRep) = DBInteraction.Retrievequizz(matier.ToString(),"Created",_mainViewModel);
        var jscode = WebviewInteraction.send_data_Card_Created(QuizzUtilities.Miseneformelist(_question),QuizzUtilities.Miseneformelist(_repnse),_urlQuestion,_urlRep);
        WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        ClearLogic(false,true,true);
    }
    public void CreatedLogic()
    {
        ClearLogic(false,false,false);
        var name = DBInteraction.GetName("All");
        foreach (var value in name) { Matier.Add(value);}
    }
    private void ClearLogic(bool b1, bool b2, bool b3)
    {
        Matier.Clear();
        Noms.Clear();
        (IsView,IsViewCard,IsViewReturn) = (b1,b2,b3);
    }
    private async void  ExecuteString(string data)
    {
        await CSharpScript.EvaluateAsync(data);
    }
    public new event PropertyChangedEventHandler? PropertyChanged;
    protected new void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}