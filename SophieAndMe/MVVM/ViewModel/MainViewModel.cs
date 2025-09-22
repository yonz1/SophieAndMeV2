using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using SophieAndMe.MVVM.View;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel ;

    public class MainViewModel : ObservableRecipient, INotifyPropertyChanged
    { 
        public ICommand ChoisirNomCommand {  get; }
        private readonly IDataService _dataService;
        public event EventHandler? RequestClose;
        public ObservableCollection<SubjectItem> Pages { get; set; }
        public ICommand ShowQuizzCommand { get; }
        public ICommand ShowMarkedCommand { get; }
        public ICommand ShowCustomCommand { get; }
        public ICommand ShowSettingCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand MaximizeCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand ShowProfileContent { get; }
        public ICommand AcceuilClick { get; }
        public List<string> Chapter { get; } = new();
        private string _selecteditem;

        public string SelectedItem
        {
            get => _selecteditem;
            set
            {
                if (_selecteditem != value)
                {
                    _selecteditem = value;
                    OnPropertyChanged();
                    CallQuizz(value);
                }
            }
        }
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
            LoadThemeWeb.LoadThemeWebval();
            NavigationService.Instance.Navigate("MainContent",new VLanding(this));
            CurrentMessage = "Acceuil";
            _dataService =  App.DataService;
            _dataService.IdCard = new IdCard();
            _dataService.IdCard.Number = 0;
            Chapter = DbInteraction.GetAllName();
            WebInteraction.GetProgKholle();
            Pages = new ObservableCollection<SubjectItem>
            {
                new SubjectItem {Name = "Quizz", IconVal = IconChar.UserGraduate, Navigation = new VQuizz(this), Value = "A"},        
                new SubjectItem {Name = "Marquer", IconVal = IconChar.BookBookmark, Navigation = new VMarked(this), Value = "B"},
                new SubjectItem {Name = "Personnaliser", IconVal = IconChar.UserPen, Navigation = new VCustom(this), Value = "C"},
                new SubjectItem {Name = "Agenda", IconVal = IconChar.Calendar, Navigation = new VAgenda(this), Value = "D"},
                new SubjectItem {Name = "Notes", IconVal = IconChar.Edit, Navigation = new VNotes(this), Value = "E"}
            };
            AcceuilClick = new RelayCommand(o =>
            {
                foreach (var s in Pages)
                {
                    s.IsSelected = false;
                }
                NavigationService.Instance.Navigate("MainContent", new VLanding(this));
            });
            ShowProfileContent = new RelayCommand(o =>
            {
                NavigationService.Instance.Navigate("MainContent", new Settings(this));
            });
            ExitCommand = new RelayCommand(o =>
            {
                OnClose();
                double DeltaT =   (DateTime.Now - _dataService.Start).TotalMinutes;
                SaveTimeDb(DeltaT);
            });
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
                    Console.WriteLine(subject.Name);
                    NavigationService.Instance.Navigate("MainContent",subject.Navigation);
                    CurrentMessage = localSubject.Name;
                });
            }


        }

        public void SaveTimeDb(double DeltaT)
        {
            double val = 0;
            string UserSource = "Data Source=..//..//..//Database//user_value.db";
            using SQLiteConnection c = new SQLiteConnection(UserSource);
            c.Open();
            SQLiteCommand command;
            string query = $"SELECT TimePassed FROM Time WHERE Date = {DateTime.Now.ToString("yyyy-MM-dd")}";
            command = new SQLiteCommand(query, c);  
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string Valtemp = reader.GetString(0);
                    Console.WriteLine(Valtemp);
                     val = double.Parse(Valtemp);
                }
            }
            query = val == 0 ? $"INSERT INTO Time (Date,TimePassed) VALUES ({DateTime.Now.ToString("yyyy-MM-dd")},\"{DeltaT}\")" : $"UPDATE Time SET TimePassed = \"{val + DeltaT}\" WHERE DATE = {DateTime.Now.ToString("yyyy-MM-dd")}";
            using (SQLiteCommand insertCmd = new SQLiteCommand(query, c))
            {
                Console.WriteLine(query);
                insertCmd.ExecuteNonQuery();
            }
            // query = $"SELECT Time FROM Time WHERE Date = {DateTime.Now.ToString("yyyy-MM-dd")}";

        }
        public void CallQuizz(string value)
        {
            _dataService.QuizzId.Nameindex = value;
            _dataService.QuizzId.IsAll = false;
            _dataService.QuizzId.options = "";
            CurrentMessage = value;
            _dataService.QuizzId.Matier = DbInteraction.GetMat(value);
            NavigationService.Instance.Navigate("MainContent",new QuizzLogic(this));
        }
        
        private void OnClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    
    
