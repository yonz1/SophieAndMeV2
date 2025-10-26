using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FontAwesome.Sharp;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.View;

namespace SophieAndMe.MVVM.ViewModel;

public class VMarkedModel : ObservableRecipient, INotifyPropertyChanged
{
    
    List<string>? _question; 
    List<string>? _repnse; 
    List<string>? _urlQuestion; 
    private List<string>? _urlRep;
    private readonly MainViewModel _mainViewModel;
    public ObservableCollection<string> Noms { get; set; } = new();
    public ObservableCollection<SubjectItem> Subjects { get; set; }
    public RelayCommand ChoisirNomCommand { get; }
    private readonly IDataService _dataService;

    private string _jscall;
    private bool _isview;
    public bool  IsView
    {
        get => _isview;
        set { _isview = value;
            OnPropertyChanged();
        }
    }
    
    public VMarkedModel(MainViewModel mainVm)
    {
        _mainViewModel = mainVm;
        _dataService =  App.DataService;
        _dataService.QuizzId.options = "";
        _dataService.QuizzId.IsAll = false;
        IsActive = true;
        WeakReferenceMessenger.Default.Register<MediatorMarked.JsToAppMessage>(this, (r, m) =>
        {
            var (action,question) = m.Value;
            question = question.Replace("\\large", "").Replace("\\(", "$").Replace("\\)", "$");
            Console.WriteLine("trigger");
            Console.WriteLine(question);
            DbInteraction.UnMark(question);
        });
        
        
        Subjects = new ObservableCollection<SubjectItem>
        {   
            new SubjectItem {Name = "Tous", IconVal = IconChar.Landmark},
            new SubjectItem {Name = "Mathématiques", IconVal = IconChar.Superscript},        
            new SubjectItem {Name = "Physique", IconVal = IconChar.Atom},
            new SubjectItem {Name = "SI", IconVal = IconChar.Gears},
            new SubjectItem {Name = "Français", IconVal = IconChar.Book},
            new SubjectItem {Name = "Anglais", IconVal = IconChar.EarthAmerica},
            new SubjectItem {Name = "Erreurs", IconVal = IconChar.Superpowers},
            new SubjectItem {Name = "Quizz", IconVal = IconChar.FilePen},
        };
        
        _dataService.QuizzId.IsAll = true;
        (_question, _repnse, _urlQuestion, _urlRep) = DbInteraction.GetMarked("Tous");
        var q = QuizzUtilities.Miseneformelist(_question) ?? new List<string>();
        var r = QuizzUtilities.Miseneformelist(_repnse) ?? new List<string>();
        var uq = QuizzUtilities.Miseneformelist(_urlQuestion) ?? new List<string>();
        var ur = QuizzUtilities.Miseneformelist(_urlRep) ?? new List<string>();
        ShowCard(q, r, uq, ur);
        
        foreach (var subject in Subjects)
        {
            var localSubject = subject;
            subject.SelectCommand = new RelayCommand(param =>
            {
                foreach (var s in Subjects)
                    s.IsSelected = false;

                localSubject.IsSelected = true;
                if (localSubject.Name.ToString() != "Quizz")
                {
                    if (!IsView)
                    {
                        IsView = true;
                    }
                    Noms.Clear();
                    LoadMark(localSubject.Name.ToString());    
                }
                else
                {
                    IsView = false;
                    Noms.Clear();
                    LoadMark("");    
                    var name = DbInteraction.GetName("All");
                    foreach (var value in name.Keys ) { Noms.Add(value);}
                    _dataService.QuizzId.Matier  = localSubject.Name.ToString();
                }
                
            });
        }
        
        ChoisirNomCommand = new RelayCommand(nom =>
        {
            _dataService.QuizzId.Nameindex = "Marked-" + nom;
            _dataService.QuizzId.Matier = (string)nom;
            NavigationService.Instance.Navigate("MainContent",new QuizzLogic(mainVm));
        });
    }
    public void LoadMark(string mat)
    {
        if (mat == "Tous")
        {
            _dataService.QuizzId.IsAll = true;
        }
        else
        {
            _dataService.QuizzId.IsAll = false;
        }
        (_question, _repnse, _urlQuestion, _urlRep) = DbInteraction.GetMarked(mat);
        var q = QuizzUtilities.Miseneformelist(_question) ?? new List<string>();
        var r = QuizzUtilities.Miseneformelist(_repnse) ?? new List<string>();
        var uq = QuizzUtilities.Miseneformelist(_urlQuestion) ?? new List<string>();
        var ur = QuizzUtilities.Miseneformelist(_urlRep) ?? new List<string>();
        ShowCard(q, r, uq, ur);
    }
    private void ShowCard(List<string> question,List<string> reponse,List<string> urlQuestion,List<string> urlReponse)
    {
        _dataService.IdCard.Number += 1;
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
            dico["Action"] = "Marked";
            dico["Id"] = _dataService.IdCard.Number.ToString();
            string jscode = JsonSerializer.Serialize(dico);
            WeakReferenceMessenger.Default.Send(new MediatorMarked.JsCallMessage(jscode));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


}