using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Markup;
using FontAwesome.Sharp;

namespace SophieAndMe.MVVM.Model;

public class SubjectItem : INotifyPropertyChanged
{
    public string Name { get; set; }
    public object  Navigation { get; set; }
    public string Value { get; set; }
    public ICommand SelectCommand { get; set; }
    public object IconVal { get; set; }
   
    
    
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; OnPropertyChanged(); }
    }
    
    private bool _isToggled;
    public bool IsToggled
    {
        get => _isToggled;
        set
        {
            if (_isToggled != value)
            {
                _isToggled = value;
                OnPropertyChanged(nameof(IsToggled));
            }
        }
    }
    


    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}