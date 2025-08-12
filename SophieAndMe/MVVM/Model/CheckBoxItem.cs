using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SophieAndMe.MVVM.Model;

public class CheckBoxItem : INotifyPropertyChanged
{
    public string? Name { get; set; }
    private bool _isChecked;
    public bool IsChecked
    {
        get =>  _isChecked;
        set
        {
            if (_isChecked != value )
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}