using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FontAwesome.Sharp;

namespace SophieAndMe.MVVM.Model;

public class SubjectItem : INotifyPropertyChanged
{
    public string Name { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; OnPropertyChanged(); }
    }

    public ICommand SelectCommand { get; set; }
    public object IconVal { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}