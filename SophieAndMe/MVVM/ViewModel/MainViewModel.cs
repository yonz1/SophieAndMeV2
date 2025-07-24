using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SophieAndMe.MVVM.View;
using SophieAndMe.Core;

namespace SophieAndMe.MVVM.ViewModel ;

    public class MainViewModel : INotifyPropertyChanged
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
        private List<string> val = ["Maths", "Physique", "Si"];
        private List<string> Test;
        public MainViewModel()
        {
            NavigationService.Instance.NavigateAction = view => CurrentView = view;
            NavigationService.Instance.Navigate(new VLanding());
            // foreach (var i in val)
            // {
            //     Test.Add(i);          
            // }
            ShowQuizzCommand = new RelayCommand(o => NavigationService.Instance.Navigate(new VQuizz()));
            ShowMarkedCommand = new RelayCommand(o => NavigationService.Instance.Navigate(new VMarked()));
            ShowCustomCommand = new RelayCommand(o => NavigationService.Instance.Navigate(new VCustom()));
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    
    
