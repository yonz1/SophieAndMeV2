using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FontAwesome.Sharp;
using SophieAndMe.MVVM.View;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel ;

    public class MainViewModel : ObservableRecipient, INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); } 
        }
        public ICommand ChoisirNomCommand {  get; }
        private readonly IDataService _dataService;
        public ObservableCollection<SubjectItem> Pages { get; set; }
        public ICommand ShowQuizzCommand { get; }
        public ICommand ShowMarkedCommand { get; }
        public ICommand ShowCustomCommand { get; }
        public ICommand ShowSettingCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand MaximizeCommand { get; }
        public ICommand MinimizeCommand { get; }
        private bool _isview;
        public bool  IsView
        {
            get => _isview;
            set { _isview = value;
                OnPropertyChanged();
            }
        }
        private string _currentmessage = null!;
        public string CurrentMessage
        {
            get => _currentmessage;set{        _currentmessage = value;        OnPropertyChanged();    }
        }
        private string _selectedValue;
        public string SelectedValue
        {
            get => _selectedValue;
            set
            {
                if (_selectedValue != value)
                {
                    _selectedValue =  value;
                    OnPropertyChanged();
                }
            }
        }
        public MainViewModel()
        {
            _dataService =  App.DataService;
            _dataService.IdCard = new IdCard();
            _dataService.IdCard.Number = 0;
            CurrentMessage = "Acceuil";
            NavigationService.Instance.Navigate("MainContent",new VLanding());
            Pages = new ObservableCollection<SubjectItem>
            {
                new SubjectItem {Name = "Quizz", IconVal = IconChar.UserGraduate, Navigation = new VQuizz(this), Value = "A"},        
                new SubjectItem {Name = "Marquer", IconVal = IconChar.BookBookmark, Navigation = new VMarked(this), Value = "B"},
                new SubjectItem {Name = "Personnaliser", IconVal = IconChar.UserPen, Navigation = new VCustom(this), Value = "C"},
                new SubjectItem {Name = "Agenda", IconVal = IconChar.Calendar, Navigation = new VAgenda(this), Value = "D"},
                new SubjectItem {Name = "Notes", IconVal = IconChar.Edit, Navigation = new VNotes(this), Value = "E"}
            };
            foreach (var subject in Pages)
            {
                var localSubject = subject;
                subject.SelectCommand = new RelayCommand(param =>
                {
                    foreach (var s in Pages)
                    {
                        s.IsSelected = false;
                    }
                    localSubject.IsSelected = true;
                    NavigationService.Instance.Navigate("MainContent",subject.Navigation);
                    CurrentMessage = localSubject.Name;
                });
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    
    
