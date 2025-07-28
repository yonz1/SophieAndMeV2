using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        IsActive = true;
        WeakReferenceMessenger.Default.Register<MediatorMarked.JsToAppMessage>(this, (r, m) =>
        {
            var (action,question) = m.Value;
            question = question.Replace("\\large", "").Replace("\\(", "$").Replace("\\)", "$");
            Console.WriteLine("trigger");
            Console.WriteLine(question);
            DBInteraction.UnMark(question);
        });
        
        
        Subjects = new ObservableCollection<SubjectItem>
        {   
            new SubjectItem {Name = "Mathématiques", IconVal = IconChar.Superscript},        
            new SubjectItem {Name = "Physique", IconVal = IconChar.Atom},
            new SubjectItem {Name = "SI", IconVal = IconChar.Gears},
            new SubjectItem {Name = "Français", IconVal = IconChar.Book},
            new SubjectItem {Name = "Anglais", IconVal = IconChar.EarthAmerica},
            new SubjectItem {Name = "Erreurs", IconVal = IconChar.Superpowers},
            new SubjectItem {Name = "All", IconVal = IconChar.Landmark},
            new SubjectItem {Name = "Quizz", IconVal = IconChar.FilePen},
        };
        
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
                    var name = DBInteraction.GetName("All");
                    foreach (var value in name) { Noms.Add(value);}
                    App.Current.Properties["matier"]  = localSubject.Name.ToString();
                }
                
            });
        }
        
        ChoisirNomCommand = new RelayCommand(nom =>
        {
            App.Current.Properties["nameindex"] = "Marked-" + nom;
            App.Current.Properties["matier"] = nom;
            NavigationService.Instance.Navigate(new QuizzLogic(mainVm));
        });
    }

    public async void LoadMark(string mat)
    {
        (_question, _repnse, _urlQuestion, _urlRep) = DBInteraction.GetMarked(mat);
        var q = QuizzUtilities.Miseneformelist(_question) ?? new List<string>();
        var r = QuizzUtilities.Miseneformelist(_repnse) ?? new List<string>();
        var uq = QuizzUtilities.Miseneformelist(_urlQuestion) ?? new List<string>();
        var ur = QuizzUtilities.Miseneformelist(_urlRep) ?? new List<string>();

        try
        {
            _jscall = WebviewInteraction.send_data_Card_Marked(q, r, uq, ur);
            WeakReferenceMessenger.Default.Send(new MediatorMarked.JsCallMessage(_jscall));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            MessageBox.Show("An error occured while loading your Data, please retry later");
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


}