using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using SophieAndMe.Core;
using SophieAndMe.MVVM.Model;

namespace SophieAndMe.MVVM.ViewModel;

public class ImportAddViewModel : INotifyPropertyChanged
{
    private readonly IDataService _dataService;
    public ICommand AjouterAuQuizz { get; set; }
    public ICommand ExitCommand { get; set; }
    public Action CloseAction { get; set; }
    public List<string> Chapter2 { get; } = new();
    private string _selecteditem2;
    public Action Close { get; set; }
    private bool? _dialogResult;

    public bool? DialogResult
    {
        get =>  _dialogResult;
        set { _dialogResult = value;  OnPropertyChanged(); }
    }

    public string SelectedItem2
    {
        get => _selecteditem2;
        set
        {
            if (_selecteditem2 != value)
            {
                _selecteditem2 = value;
                OnPropertyChanged();
            }
        }
    }

    public ImportAddViewModel(VCustomModel vm,string question,string imgQuestion,string rep, string imgRep)
    {
        _dataService = App.DataService;
        string jscall = WebviewInteraction.send_data("", QuizzUtilities.Miseneformetext(question), QuizzUtilities.Miseneformetext(rep),imgQuestion,imgRep);
        WeakReferenceMessenger.Default.Send(new MediatorImportAdd.JsCallMessage(jscall));
        Chapter2 = DbInteraction.GetNameCreated("All");
        ExitCommand = new RelayCommand(o =>
        {
            _dataService.WinBin = false;
            Console.WriteLine(_dataService.WinBin.ToString());
            DialogResult = false;

        });
        AjouterAuQuizz = new RelayCommand(o =>
        {
            _dataService.WinBin = false;
            Console.WriteLine(_dataService.WinBin.ToString());
            DialogResult = true;
        });
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
