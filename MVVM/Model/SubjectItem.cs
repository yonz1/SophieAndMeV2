using System.ComponentModel;
using System.Windows.Input;
using FontAwesome.Sharp;

namespace SophieAndMe.MVVM.Model;

public class SubjectItem : INotifyPropertyChanged
{
    public string Name { get; set; }
    public IconChar  IconVal { get; set; }
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
        
    public ICommand SelectCommand { get; set; }
}