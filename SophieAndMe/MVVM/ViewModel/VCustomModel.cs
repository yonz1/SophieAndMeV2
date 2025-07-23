using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;


namespace SophieAndMe.MVVM.ViewModel;

public class VCustomModel : ObservableRecipient, INotifyPropertyChanged
{
    public ICommand Create { get; }
    public ICommand Created { get; }
    public ICommand Back_quizz_Click { get; }
    public RelayCommand ChoisirMatierCommand { get; }
    public RelayCommand ChoisirNomCommand { get;  }
    private List<string> _mat = ["Physique", "Mathématiques", "Français", "Anglais", "Erreurs", "SI"];
    public ObservableCollection<string> Noms { get; set; } = new();
    public ObservableCollection<string> Matier { get; set; } = new();
    private List<string> _question;
    private List<string> _repnse;
    private List<string> _urlQuestion;
    private List<string> _urlRep;
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
    
    
    public VCustomModel()
    {
        IsActive = true;
        IsView = false;
        IsViewCard = false;
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
                    (IsView,IsViewCard) = (true,false);
                    break;
                case "Replace":
                    DBInteraction.ReplaceQuizz(matier,name,question,imgQuestion,rep,imgRep);
                    var data = DBInteraction.GetAllName();
                    var jscall = WebviewInteraction.Initcustom(data, "Add");
                    WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscall));
                    break;
            }
        });
        
        Back_quizz_Click = new RelayCommand(o =>
        {
            (IsView,IsViewCard) = (false,false);
            ClearLogic();
            var name = DBInteraction.GetName("All");
            foreach (var value in name) { Matier.Add(value);}
        });
        Create = new RelayCommand(o =>
        {
            (IsView,IsViewCard) = (true,false);
            ClearLogic();
            var data = DBInteraction.GetAllName();
            var jscode = WebviewInteraction.Initcustom(data, "Add");
            WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
        });
        Created = new RelayCommand(o =>
        {
            (IsView,IsViewCard) = (false,false);
            ClearLogic();
            var name = DBInteraction.GetName("All");
            foreach (var value in name) { Matier.Add(value);}
        });
        
        ChoisirMatierCommand = new RelayCommand(matier =>
        {
            ClearLogic();
            Console.WriteLine(matier);
            if (_mat.Contains(matier))
            {
                App.Current.Properties["matier"] = matier;
                var name = DBInteraction.GetNameCreated(matier.ToString());
                foreach (var value in name) { Matier.Add(value);} }
            else
            {
                App.Current.Properties["nameindex"] = matier;
                (_question, _repnse, _urlQuestion,_urlRep) = DBInteraction.Retrievequizz(matier.ToString(),"Created");
                var jscode = WebviewInteraction.send_data_Card_Created(QuizzUtilities.Miseneformelist(_question),QuizzUtilities.Miseneformelist(_repnse),_urlQuestion,_urlRep);
                Console.WriteLine(jscode);
                WeakReferenceMessenger.Default.Send(new MediatorCustom.JsCallMessage(jscode));
                (IsView,IsViewCard) = (false,true);
            }
        });
    }

    public void ClearLogic()
    {
        Matier.Clear();
        Noms.Clear();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}