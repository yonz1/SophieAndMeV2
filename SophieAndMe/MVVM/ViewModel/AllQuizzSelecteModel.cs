using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.VisualStyles;
using SophieAndMe.MVVM.Model;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using SophieAndMe.Core;
using SophieAndMe.MVVM.View;
using NavigationService = SophieAndMe.Core.NavigationService;

namespace SophieAndMe.MVVM.ViewModel;

public class AllQuizzSelecteModel  : INotifyPropertyChanged
{
    private readonly IDataService  _dataService;
    public ObservableCollection<CheckBoxItem> Items { get; set; }
    private readonly MainViewModel _mainViewModel;
    public ICommand BackQuizzClick { get; }
    public ICommand Fill { get; }
    public ICommand Clear { get; }
    public ICommand StartQuizz { get; }
    public ICommand Reinitialiser { get; }
    public ICommand Continuer { get; }
    private bool _ischeckedvalue;
    public bool IsCheckedValue
    {
        get =>  _ischeckedvalue;
        set
        {
            if (_ischeckedvalue != value)
            {
                _ischeckedvalue = value;
                OnPropertyChanged(nameof(IsCheckedValue));
            }
        }
    }

    private string _progvalue;

    public string ProgValue
    {
        get => _progvalue;
        set
        {
            _progvalue = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<string> Chapitres { get; set; } = [];
    public AllQuizzSelecteModel(MainViewModel mainVm)
    {
        _dataService = App.DataService;
        Items = new ObservableCollection<CheckBoxItem>();
        _mainViewModel = mainVm;
        _dataService.QuizzId.Matier = Application.Current.Properties["nameindex"].ToString();
        var name = DbInteraction.GetName(Application.Current.Properties["nameindex"]);
        foreach (var value in name)
        {
            Console.WriteLine(value);
            var Temps = new CheckBoxItem();
            Temps.IsChecked = true;
            Temps.Name = value;
            Items.Add(Temps);
        }
        MessageLogic();
        Fill = new RelayCommand(o => FillLogic());
        Clear = new RelayCommand(o => ClearLogic());
        BackQuizzClick = new RelayCommand(o => NavigationService.Instance.Navigate("MainContent", new VQuizz(mainVm)));
        StartQuizz = new RelayCommand(o =>
        {
            _dataService.QuizzId.options = "";
            SaveAllChecked();
            NavigationService.Instance.Navigate("MainContent", new QuizzLogic(mainVm));
        });
        Reinitialiser = new RelayCommand(o =>
        {
            DeleteLogic();
            MessageLogic();
        });
        Continuer = new RelayCommand(o =>
        {
            _dataService.QuizzId.options = "Progressif";
            if (ProgValue == "Démarrer")
            {
                SaveAllChecked();
                SaveDataProgress(mainVm);
                NavigationService.Instance.Navigate("MainContent", new QuizzLogic(mainVm));
            }
            else
            {
                NavigationService.Instance.Navigate("MainContent", new QuizzLogic(mainVm));
            }
        });
    }


    private void MessageLogic()
    {
        if (DbInteraction.VerifStart())
        {
             ProgValue =  "Continuer";
        }
        else
        {
            ProgValue = "Démarrer";
        }
    }
    private void DeleteLogic()
    {
        DbInteraction.DeleteProg();

    }
    private void SaveDataProgress(MainViewModel mainVm)
    {
        DbInteraction.RegisterProgression(mainVm);
    }
    private void ClearLogic()
    {
        foreach (var item in Items)
        {
            item.IsChecked = false;
        }   
    }
    private void FillLogic()
    {
        foreach (var item in Items)
        {
            item.IsChecked = true;
        }
    }
    private void SaveAllChecked()
    {
        var listChecked = Items.Where(item => item.IsChecked).ToList();
        _dataService.SharedListChapter =  listChecked.Select(item => item.Name).ToList()!;
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}