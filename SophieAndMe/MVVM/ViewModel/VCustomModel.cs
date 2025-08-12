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
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.View.CardDisplay;


namespace SophieAndMe.MVVM.ViewModel;

public class VCustomModel : ObservableRecipient, INotifyPropertyChanged
{
    public ICommand Create { get; }
    public ICommand Created { get; }
    public ICommand Import { get; }
    public RelayCommand ChoisirMatierCommand { get; }
    public RelayCommand Return { get; }
    public RelayCommand Advance { get; }
    private object _currenviewCard;
    public object CurrentViewCard
    {
        get => _currenviewCard;
        set { _currenviewCard = value; OnPropertyChanged(); } 
    }
    
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
    private readonly IDataService  _dataService;
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
        _dataService = App.DataService;
        _dataService.QuizzId.options = "";
        IsActive = true;
        _mainViewModel = mainVm;

        Return = new RelayCommand(o =>
        {
            switch (App.Current.Properties["old"])
            {
                case "FirstLayer":
                    App.Current.Properties["old"] = "CreatedLogic";
                    FirstLayer(_dataService.QuizzId.Matier);
                    break;
                case "CreatedLogic":
                    CreatedLogic();
                    break;
            }
        });
        Create = new RelayCommand(o =>
        {
            ClearLogic(true,false,false);
            var data = DbInteraction.GetAllName();
            var jscode = WebviewInteraction.Initcustom(data, "Add");
            WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        });
        Created = new RelayCommand(o =>
        {
            CreatedLogic();
        });

        Import = new RelayCommand(o =>
        {
            _currenviewCard = new CardDisplayImportModel(this,"Import");
            ClearLogic(false,true,false);
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
                App.Current.Properties["nameindex"] = matier;
                _currenviewCard = new CardDisplayImportModel(this,"Created");
                ClearLogic(false,true,true);
            }
        });
    }
    public void ReplaceLogic(string matier,string name,string question,string rep,string imgQuestion,string imgRep)
    {
        DbInteraction.ReplaceQuizz(matier,name,question,imgQuestion,rep,imgRep);
        var data = DbInteraction.GetAllName();
        var jscall = WebviewInteraction.Initcustom(data, "Add");
        Console.WriteLine(jscall);
        WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscall));
    }
    public void EditLogic(string question)
    {
        App.Current.Properties["old_quest"] = question;
        string? matier;
        string? name;
        string? rep;
        string? imgQuestion;
        string? imgRep;
        (matier, name, question, rep, imgQuestion, imgRep) = DbInteraction.SearchQuizzCreated(question);
        var jscode = WebviewInteraction.EdtiQuizz(matier,name,question,imgQuestion,rep,imgRep);
        WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        ClearLogic(true,false,false);
    }
    
    public  void FirstLayer(object matier)
    {
        ClearLogic(false,false,true);
        _dataService.QuizzId.Matier = (string)matier;
        var name = DbInteraction.GetNameCreated(matier.ToString());
        foreach (var value in name) { Matier.Add(value);} 
    }


    public void CreatedLogic()
    {
        ClearLogic(false,false,false);
        var name = DbInteraction.GetName("All");
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