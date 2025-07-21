using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FontAwesome.Sharp;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.Model;


namespace SophieAndMe.MVVM.ViewModel
{
    
    
    public class VQuizzModel 
    {

        public ObservableCollection<string> Noms { get; set; } = new();
        public ICommand ChoisirNomCommand {  get; }
        public ObservableCollection<SubjectItem> Subjects { get; set; }
        public RelayCommand SelectSubjectCommand { get; }
        
        // public List<string> LabelName = ["LMathématiques", "LPhysique", "LSI", "LAnglais", "LFrançais", "LErreurs", "LAll"];
        
        
        
        //############################################################ Text 
        
            
        // private string _lmaths = null!;
        // public string Lmaths
        // {
        //     get => _lmaths;set{        _lmaths = value;        OnPropertyChanged();    } }
        //
        // private string _lphysique = null!;
        // public string LPhysique
        // {
        //     get => _lphysique;set{        _lphysique = value;        OnPropertyChanged();    } }
        //
        // private string _lsi = null!;
        // public string LSI
        // {
        //     get => _lsi;set{        _lsi = value;        OnPropertyChanged();    } }
        //
        // private string _langlais = null!;
        // public string LAnglais
        // {
        //     get => _langlais;set{        _langlais = value;        OnPropertyChanged();    } }
        //
        // private string _lfrançais = null!;
        // public string LFrançais
        // {
        //     get => _lfrançais;set{        _lfrançais = value;        OnPropertyChanged();    } }
        //
        // private string _lerreurs = null!;
        // public string LErreurs
        // {
        //     get => _lerreurs;set{        _lerreurs = value;        OnPropertyChanged();    } }
        //
        // private string _lAll = null!;
        // public string LAll
        // {
        //     get => _lAll;set{        _lAll = value;        OnPropertyChanged();    } }
        
        public VQuizzModel() 
        {
            
            Subjects = new ObservableCollection<SubjectItem>
            {   
                new SubjectItem {Name = "Mathématiques", IconVal = IconChar.Superscript, IsSelected = true},
                new SubjectItem {Name = "Physique", IconVal = IconChar.Atom, IsSelected = false},
                new SubjectItem {Name = "Si", IconVal = IconChar.Gears, IsSelected = false},
                new SubjectItem {Name = "Français", IconVal = IconChar.Book, IsSelected = false},
                new SubjectItem {Name = "Anglais", IconVal = IconChar.EarthAmerica, IsSelected = false},
                new SubjectItem {Name = "Erreurs", IconVal = IconChar.Superpowers, IsSelected = false},
                new SubjectItem {Name = "All", IconVal = IconChar.Landmark, IsSelected = false},
                    
            };
            
            
            
            foreach (var subject in Subjects)
            {
                var localSubject = subject;
                subject.SelectCommand = new RelayCommand(param =>
                {
                    foreach (var s in Subjects)
                        s.IsSelected = false;

                    localSubject.IsSelected = true;
                    Noms.Clear();
                    var name = DBInteraction.GetName(localSubject.Name.ToString());
                    foreach (var value in name) { Noms.Add(value);}
                    App.Current.Properties["matier"]  = localSubject.Name.ToString();
                    Console.WriteLine(localSubject.Name);
                });
            }
            
            ChoisirNomCommand = new RelayCommand(nom =>
            {
                App.Current.Properties["nameindex"] = nom;
                Console.WriteLine(nom);
                NavigationService.Instance.Navigate(new QuizzLogic());
            });
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        
    }
}