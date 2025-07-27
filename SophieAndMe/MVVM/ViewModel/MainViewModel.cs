using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SophieAndMe.MVVM.View;
using SophieAndMe.Core;

namespace SophieAndMe.MVVM.ViewModel ;

    public class MainViewModel : ObservableRecipient, INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); } 
        }

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
        
        public MainViewModel()
        {

            NavigationService.Instance.NavigateAction = view => CurrentView = view;
            CurrentMessage = "Acceuil";
            NavigationService.Instance.Navigate(new VLanding());
            ShowQuizzCommand = new RelayCommand(o =>
            {
                    NavigationService.Instance.Navigate(new VQuizz());
                    CurrentMessage = "Quizzs";
            });
            ShowMarkedCommand = new RelayCommand(o =>
            {
                NavigationService.Instance.Navigate(new VMarked());
                CurrentMessage = "Marquer";
            });
            ShowCustomCommand = new RelayCommand(o =>
            {
                NavigationService.Instance.Navigate(new VCustom());
                CurrentMessage = "Personnaliser";
            });
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    
    
