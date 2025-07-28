using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.Model;


namespace SophieAndMe.MVVM.ViewModel;
    
    
    public class VQuizzModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        public ObservableCollection<string> Noms { get; set; } = new();
        public ICommand ChoisirNomCommand {  get; }
        public ObservableCollection<SubjectItem> Subjects { get; set; }
        
        public VQuizzModel(MainViewModel mainVm) 
        {
            _mainViewModel = mainVm;
            _mainViewModel.CurrentMessage = "Quizzs";
            Subjects = new ObservableCollection<SubjectItem>
            {   
                new SubjectItem {Name = "Mathématiques", IconVal = IconChar.Superscript},        
                new SubjectItem {Name = "Physique", IconVal = IconChar.Atom},
                new SubjectItem {Name = "SI", IconVal = IconChar.Gears},
                new SubjectItem {Name = "Français", IconVal = IconChar.Book},
                new SubjectItem {Name = "Anglais", IconVal = IconChar.EarthAmerica},
                new SubjectItem {Name = "Erreurs", IconVal = IconChar.Superpowers},
                new SubjectItem {Name = "All", IconVal = IconChar.Landmark},
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

                });
            }
            
            ChoisirNomCommand = new RelayCommand(nom =>
            {
                Application.Current.Properties["nameindex"] = nom;
                _mainViewModel.CurrentMessage = nom.ToString() ?? throw new InvalidOperationException();
                NavigationService.Instance.Navigate(new QuizzLogic(mainVm));
            });
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }